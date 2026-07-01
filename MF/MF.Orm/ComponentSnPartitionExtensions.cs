using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MF.Orm
{
    public static class ComponentSnPartitionExtensions
    {
        // 配置表名，用于跳过自身查询避免递归
        private const string ConfigTable = "component_sn_time_range";

        /// <summary>
        /// 注册拦截器：检测到 component_sn 条件时，从配置表查询对应时间范围，
        /// 自动补全 create_time 双边分区键（>= 和 <=），避免全表扫描。
        /// </summary>
        public static void UseAutoPartitionKeyByComponentSn(
            this ISqlSugarClient db,
            ILogger log)
        {
            SqlSugarInterceptorManager.RegisterInterceptor((sql, pars) =>
            {
                var trimSql = sql.TrimStart();

                // 只拦截 SELECT
                if (!trimSql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    return KeyValuePair.Create(sql, pars);

                // 跳过对配置表自身的查询，防止递归
                if (trimSql.Contains(ConfigTable, StringComparison.OrdinalIgnoreCase))
                    return KeyValuePair.Create(sql, pars);

                // 没有 component_sn 条件则放行
                if (!trimSql.Contains("component_sn", StringComparison.OrdinalIgnoreCase))
                    return KeyValuePair.Create(sql, pars);

                // 尝试匹配 = / IN / LIKE 三种条件
                var eqMatch = Regex.Match(trimSql, @"component_sn\s*=\s*(@\w+)", RegexOptions.IgnoreCase);
                var inMatch = Regex.Match(trimSql, @"component_sn\s+IN\s*\(([^)]+)\)", RegexOptions.IgnoreCase);
                var likeMatch = Regex.Match(trimSql, @"component_sn\s+LIKE\s*(@\w+)", RegexOptions.IgnoreCase);

                if (!eqMatch.Success && !inMatch.Success && !likeMatch.Success)
                    return KeyValuePair.Create(sql, pars);

                // 从 SQL 参数中提取 SN 值（或 LIKE 前缀）
                var (snValues, likePrefix) = ResolveSnCondition(pars, eqMatch, inMatch, likeMatch);

                if (snValues.Count == 0 && likePrefix == null)
                    return KeyValuePair.Create(sql, pars);

                // 根据 SN 查询配置表获取时间范围
                var ranges = QueryTimeRanges(db, snValues, likePrefix);
                if (ranges.Count == 0)
                    return KeyValuePair.Create(sql, pars);

                var minStart = ranges.Min(r => r.StartTime);
                var maxEnd = ranges.Max(r => r.EndTime);

                if (minStart == null && maxEnd == null)
                    return KeyValuePair.Create(sql, pars);

                // 补全缺失的 create_time 边界（>= 下界 / <= 上界）
                trimSql = AppendTimeBounds(trimSql, minStart.Value, maxEnd.Value);

                var dbType = db.CurrentConnectionConfig.DbType;
                log.LogInformation($"分区键时间兜底拦截【{dbType}】：{string.Join(",", snValues)} => 追加create_time分区键");

                return KeyValuePair.Create(trimSql, pars);
            });
        }

        /// <summary>根据匹配到的条件类型分发到对应的解析函数</summary>
        private static (List<string> snValues, string likePrefix) ResolveSnCondition(
            SugarParameter[] pars,
            Match eqMatch, Match inMatch, Match likeMatch)
        {
            if (eqMatch.Success)
                return ResolveEq(pars, eqMatch);

            if (inMatch.Success)
                return ResolveIn(pars, inMatch);

            return ResolveLike(pars, likeMatch);
        }

        /// <summary>解析 = 条件：提取单个参数值</summary>
        private static (List<string> snValues, string likePrefix) ResolveEq(SugarParameter[] pars, Match eqMatch)
        {
            var param = ResolveParam(pars, eqMatch.Groups[1].Value);
            if (param?.Value == null)
                return (new List<string>(), null);
            return (new List<string> { param.Value.ToString() }, null);
        }

        /// <summary>解析 IN 条件：提取括号内所有参数值</summary>
        private static (List<string> snValues, string likePrefix) ResolveIn(SugarParameter[] pars, Match inMatch)
        {
            var snValues = new List<string>();
            var paramMatches = Regex.Matches(inMatch.Groups[1].Value, @"@\w+");
            foreach (Match m in paramMatches)
            {
                var param = ResolveParam(pars, m.Value);
                if (param?.Value != null)
                    snValues.Add(param.Value.ToString());
            }
            return (snValues, null);
        }

        /// <summary>
        /// 解析 LIKE 条件：
        /// - 不含 % → 精确匹配
        /// - xxx%   → 按前缀模糊
        /// - %xxx   → 按后缀模糊
        /// - %xxx%  → 按包含模糊
        /// </summary>
        private static (List<string> snValues, string likePattern) ResolveLike(SugarParameter[] pars, Match likeMatch)
        {
            var param = ResolveParam(pars, likeMatch.Groups[1].Value);
            if (param?.Value == null)
                return (new List<string>(), null);

            var val = param.Value.ToString();
            if (!val.Contains('%'))
                return (new List<string> { val }, null);

            return (new List<string>(), val);
        }

        /// <summary>查询配置表获取 SN 对应的起止时间</summary>
        private static List<ComponentSnTimeRangeEntity> QueryTimeRanges(
            ISqlSugarClient db, List<string> snValues, string likePattern)
        {
            if (likePattern != null)
            {
                if (likePattern.StartsWith("%") && likePattern.EndsWith("%") && likePattern.Length > 2)
                    return db.Queryable<ComponentSnTimeRangeEntity>()
                        .Where(r => r.Sn.Contains(likePattern.Trim('%')))
                        .ToList();

                if (likePattern.StartsWith("%"))
                    return db.Queryable<ComponentSnTimeRangeEntity>()
                        .Where(r => r.Sn.EndsWith(likePattern.TrimStart('%')))
                        .ToList();

                if (likePattern.EndsWith("%"))
                    return db.Queryable<ComponentSnTimeRangeEntity>()
                        .Where(r => r.Sn.StartsWith(likePattern.TrimEnd('%')))
                        .ToList();

                return new List<ComponentSnTimeRangeEntity>();
            }

            return db.Queryable<ComponentSnTimeRangeEntity>()
                .Where(r => snValues.Contains(r.Sn))
                .ToList();
        }

        /// <summary>
        /// 检查 SQL 中 create_time 已存在的边界条件，补全缺失的一侧或双侧。
        /// - 已有 >= 或 > → 不补下界
        /// - 已有 <= 或 < → 不补上界
        /// - 都没有 → 补全双边
        /// </summary>
        private static string AppendTimeBounds(string sql, DateTime minStart, DateTime maxEnd)
        {
            var hasLower = Regex.IsMatch(sql, @"create_time\s*(>=|>)", RegexOptions.IgnoreCase);
            var hasUpper = Regex.IsMatch(sql, @"create_time\s*(<=|<)", RegexOptions.IgnoreCase);

            var lower = $" create_time >= '{minStart:yyyy-MM-dd HH:mm:ss}' ";
            var upper = $" create_time <= '{maxEnd:yyyy-MM-dd HH:mm:ss}' ";

            var prefix = sql.Contains(" WHERE ", StringComparison.OrdinalIgnoreCase) ? " AND " : " WHERE ";

            if (!hasLower && !hasUpper)
                return sql + prefix + lower + "AND " + upper;

            if (!hasLower)
                return sql + prefix + lower;

            if (!hasUpper)
                return sql + prefix + upper;

            return sql;
        }

        /// <summary>根据参数名在参数数组中查找对应的 SugarParameter</summary>
        private static SugarParameter ResolveParam(SugarParameter[] pars, string paramName)
        {
            return pars.FirstOrDefault(p =>
                string.Equals(p.ParameterName, paramName, StringComparison.OrdinalIgnoreCase));
        }
    }
}

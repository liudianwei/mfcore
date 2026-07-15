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
        private const string InterceptorRegistrationKey = nameof(ComponentSnPartitionExtensions);
        private const int MaxLikeMatchCount = 10000;

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
                var eqMatch = Regex.Match(trimSql,
                    "\\bcomponent_sn\\b(?:\\]|`|\")?\\s*=\\s*([@:]\\w+)", RegexOptions.IgnoreCase);
                var inMatch = Regex.Match(trimSql,
                    "\\bcomponent_sn\\b(?:\\]|`|\")?\\s+IN\\s*\\(([^)]+)\\)", RegexOptions.IgnoreCase);
                var likeMatch = Regex.Match(trimSql,
                    "\\bcomponent_sn\\b(?:\\]|`|\")?\\s+LIKE\\s*" +
                    "(CONCAT\\s*\\([^)]*\\)|(?:N?'%'\\s*(?:\\+|\\|\\|)\\s*)?[@:]\\w+" +
                    "(?:\\s*(?:\\+|\\|\\|)\\s*N?'%')?|N?'(?:''|[^'])*')",
                    RegexOptions.IgnoreCase);

                if (!eqMatch.Success && !inMatch.Success && !likeMatch.Success)
                    return KeyValuePair.Create(sql, pars);

                var conditionIndex = eqMatch.Success ? eqMatch.Index
                    : inMatch.Success ? inMatch.Index
                    : likeMatch.Index;

                // 当前查询层已有完整时间范围时无需再查询 SN 与时间范围映射表。
                if (HasCompleteTimeRange(trimSql, conditionIndex))
                    return KeyValuePair.Create(sql, pars);

                // 从 SQL 参数中提取 SN 值（或 LIKE 前缀）
                var (snValues, likePrefix) = ResolveSnCondition(pars, eqMatch, inMatch, likeMatch);

                if (snValues.Count == 0 && likePrefix == null)
                    return KeyValuePair.Create(sql, pars);

                // 根据 SN 查询配置表获取时间范围
                var ranges = QueryTimeRanges(db, snValues, likePrefix, log);
                if (ranges.Count == 0)
                    return KeyValuePair.Create(sql, pars);

                var minStart = ranges.Min(r => r.StartTime);
                var maxEnd = ranges.Max(r => r.EndTime);

                if (minStart == null && maxEnd == null)
                    return KeyValuePair.Create(sql, pars);

                // 补全缺失的 create_time 边界（>= 下界 / <= 上界）
                trimSql = AppendTimeBounds(trimSql, minStart, maxEnd, conditionIndex);

                var dbType = db.CurrentConnectionConfig.DbType;
                log.LogInformation($"分区键时间兜底拦截【{dbType}】：{string.Join(",", snValues)} => 追加create_time分区键");

                return KeyValuePair.Create(trimSql, pars);
            }, InterceptorRegistrationKey);
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
            var paramMatches = Regex.Matches(inMatch.Groups[1].Value, @"[@:]\w+");
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
            var expression = likeMatch.Groups[1].Value;
            var paramMatch = Regex.Match(expression, @"[@:]\w+");
            if (!paramMatch.Success)
            {
                var literalMatch = Regex.Match(expression, @"^N?'((?:''|[^'])*)'$",
                    RegexOptions.IgnoreCase);
                if (!literalMatch.Success)
                    return (new List<string>(), null);

                // SQL 字符串中的两个单引号表示一个实际的单引号。
                var literalValue = literalMatch.Groups[1].Value.Replace("''", "'");
                if (!literalValue.Contains('%'))
                    return (new List<string> { literalValue }, null);

                return (new List<string>(), literalValue);
            }

            var param = ResolveParam(pars, paramMatch.Value);
            if (param?.Value == null)
                return (new List<string>(), null);

            var val = param.Value.ToString();
            var expressionBeforeParam = expression.Substring(0, paramMatch.Index);
            var expressionAfterParam = expression.Substring(paramMatch.Index + paramMatch.Length);

            if (Regex.IsMatch(expressionBeforeParam, @"N?'%'", RegexOptions.IgnoreCase) &&
                !val.StartsWith("%"))
                val = "%" + val;

            if (Regex.IsMatch(expressionAfterParam, @"N?'%'", RegexOptions.IgnoreCase) &&
                !val.EndsWith("%"))
                val += "%";

            if (!val.Contains('%'))
                return (new List<string> { val }, null);

            return (new List<string>(), val);
        }

        /// <summary>查询配置表获取 SN 对应的起止时间</summary>
        private static List<ComponentSnTimeRangeEntity> QueryTimeRanges(
            ISqlSugarClient db, List<string> snValues, string likePattern, ILogger log)
        {
            if (likePattern != null)
            {
                var keyword = likePattern.Trim('%');
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    log.LogWarning("component_sn LIKE 条件仅包含通配符，跳过自动追加分区键：{LikePattern}",
                        likePattern);
                    return new List<ComponentSnTimeRangeEntity>();
                }

                var query = CreateLikeQuery(db, likePattern, keyword);
                if (query == null)
                    return new List<ComponentSnTimeRangeEntity>();

                var ranges = query
                    .OrderByDescending(r => r.EndTime)
                    .OrderByDescending(r => r.StartTime)
                    .Take(MaxLikeMatchCount + 1)
                    .ToList();

                if (ranges.Count > MaxLikeMatchCount)
                {
                    log.LogWarning(
                        "component_sn LIKE 条件匹配超过 {MaxCount} 条，改为在数据库端汇总时间范围：{LikePattern}",
                        MaxLikeMatchCount, likePattern);

                    var range = CreateLikeQuery(db, likePattern, keyword)
                        .Select(r => new ComponentSnTimeRangeEntity
                        {
                            StartTime = SqlFunc.AggregateMin(r.StartTime),
                            EndTime = SqlFunc.AggregateMax(r.EndTime)
                        })
                        .First();

                    return range == null
                        ? new List<ComponentSnTimeRangeEntity>()
                        : new List<ComponentSnTimeRangeEntity> { range };
                }

                return ranges;
            }

            return db.Queryable<ComponentSnTimeRangeEntity>()
                .Where(r => snValues.Contains(r.ComponentSn))
                .OrderByDescending(r => r.EndTime)
                .OrderByDescending(r => r.StartTime)
                .ToList();
        }

        private static ISugarQueryable<ComponentSnTimeRangeEntity> CreateLikeQuery(
            ISqlSugarClient db, string likePattern, string keyword)
        {
            if (likePattern.StartsWith("%") && likePattern.EndsWith("%") && likePattern.Length > 2)
                return db.Queryable<ComponentSnTimeRangeEntity>()
                    .Where(r => r.ComponentSn.Contains(keyword));

            if (likePattern.StartsWith("%"))
                return db.Queryable<ComponentSnTimeRangeEntity>()
                    .Where(r => r.ComponentSn.EndsWith(keyword));

            if (likePattern.EndsWith("%"))
                return db.Queryable<ComponentSnTimeRangeEntity>()
                    .Where(r => r.ComponentSn.StartsWith(keyword));

            return null;
        }

        /// <summary>
        /// 检查 SQL 中 create_time 已存在的边界条件，补全缺失的一侧或双侧。
        /// - 已有 >= 或 > → 不补下界
        /// - 已有 <= 或 < → 不补上界
        /// - 都没有 → 补全双边
        /// </summary>
        private static string AppendTimeBounds(
            string sql, DateTime? minStart, DateTime? maxEnd, int conditionIndex)
        {
            var (queryStart, queryEnd) = FindContainingQuery(sql, conditionIndex);
            var querySql = sql.Substring(queryStart, queryEnd - queryStart);
            var hasLower = Regex.IsMatch(querySql,
                "\\bcreate_time\\b(?:\\]|`|\")?\\s*(>=|>)", RegexOptions.IgnoreCase);
            var hasUpper = Regex.IsMatch(querySql,
                "\\bcreate_time\\b(?:\\]|`|\")?\\s*(<=|<)", RegexOptions.IgnoreCase);

            var bounds = new List<string>();
            if (!hasLower && minStart.HasValue)
                bounds.Add($"create_time >= '{minStart.Value:yyyy-MM-dd HH:mm:ss}'");

            if (!hasUpper && maxEnd.HasValue)
                bounds.Add($"create_time <= '{maxEnd.Value:yyyy-MM-dd HH:mm:ss}'");

            if (bounds.Count == 0)
                return sql;

            var insertionIndex = FindClauseStart(sql, queryStart, queryEnd);
            var whereIndex = FindTopLevelKeyword(sql, queryStart, insertionIndex, "WHERE");
            var boundSql = string.Join(" AND ", bounds);

            if (whereIndex < 0)
                return sql.Insert(insertionIndex, $" WHERE {boundSql} ");

            // 将原 WHERE 表达式整体括起来，避免原条件含 OR 时改变逻辑优先级。
            var predicateStart = whereIndex + "WHERE".Length;
            var predicate = sql.Substring(predicateStart, insertionIndex - predicateStart).Trim();
            var replacement = $" ({predicate}) AND {boundSql} ";
            return sql.Remove(predicateStart, insertionIndex - predicateStart)
                .Insert(predicateStart, replacement);
        }

        /// <summary>检查包含 component_sn 的查询层是否已经有完整的 create_time 时间范围。</summary>
        private static bool HasCompleteTimeRange(string sql, int conditionIndex)
        {
            var (queryStart, queryEnd) = FindContainingQuery(sql, conditionIndex);
            var querySql = sql.Substring(queryStart, queryEnd - queryStart);

            if (Regex.IsMatch(querySql,
                "\\bcreate_time\\b(?:\\]|`|\")?\\s+BETWEEN\\b",
                RegexOptions.IgnoreCase))
                return true;

            var hasLower = Regex.IsMatch(querySql,
                "\\bcreate_time\\b(?:\\]|`|\")?\\s*(>=|>)",
                RegexOptions.IgnoreCase);
            var hasUpper = Regex.IsMatch(querySql,
                "\\bcreate_time\\b(?:\\]|`|\")?\\s*(<=|<)",
                RegexOptions.IgnoreCase);

            return hasLower && hasUpper;
        }

        /// <summary>定位包含目标条件的最内层 SELECT 子查询；没有子查询包装时返回整个 SQL。</summary>
        private static (int start, int end) FindContainingQuery(string sql, int conditionIndex)
        {
            var stack = new Stack<(int index, bool isQuery)>();
            var result = (start: 0, end: sql.Length);
            var quote = '\0';

            for (var i = 0; i < sql.Length; i++)
            {
                if (quote != '\0')
                {
                    if (sql[i] == quote)
                    {
                        if (i + 1 < sql.Length && sql[i + 1] == quote)
                            i++;
                        else
                            quote = '\0';
                    }
                    continue;
                }

                if (sql[i] == '\'' || sql[i] == '"' || sql[i] == '`')
                {
                    quote = sql[i];
                    continue;
                }

                if (sql[i] == '(')
                {
                    var next = i + 1;
                    while (next < sql.Length && char.IsWhiteSpace(sql[next])) next++;
                    stack.Push((i, IsKeywordAt(sql, next, "SELECT")));
                }
                else if (sql[i] == ')' && stack.Count > 0)
                {
                    var open = stack.Pop();
                    if (open.isQuery && open.index < conditionIndex && conditionIndex < i &&
                        open.index + 1 >= result.start)
                        result = (open.index + 1, i);
                }
            }

            return result;
        }

        private static int FindClauseStart(string sql, int start, int end)
        {
            var clauses = new[] { "GROUP", "HAVING", "ORDER", "UNION", "OPTION", "OFFSET", "FETCH" };
            var depth = 0;
            var quote = '\0';
            for (var i = start; i < end; i++)
            {
                if (quote != '\0')
                {
                    if (sql[i] == quote)
                    {
                        if (i + 1 < end && sql[i + 1] == quote) i++;
                        else quote = '\0';
                    }
                    continue;
                }
                if (sql[i] == '\'' || sql[i] == '"' || sql[i] == '`') { quote = sql[i]; continue; }
                if (sql[i] == '(') { depth++; continue; }
                if (sql[i] == ')') { depth--; continue; }
                if (depth == 0 && (sql[i] == ';' || clauses.Any(c => IsKeywordAt(sql, i, c))))
                    return i;
            }
            return end;
        }

        private static int FindTopLevelKeyword(string sql, int start, int end, string keyword)
        {
            var depth = 0;
            var quote = '\0';
            for (var i = start; i < end; i++)
            {
                if (quote != '\0')
                {
                    if (sql[i] == quote)
                    {
                        if (i + 1 < end && sql[i + 1] == quote) i++;
                        else quote = '\0';
                    }
                    continue;
                }
                if (sql[i] == '\'' || sql[i] == '"' || sql[i] == '`') { quote = sql[i]; continue; }
                if (sql[i] == '(') { depth++; continue; }
                if (sql[i] == ')') { depth--; continue; }
                if (depth == 0 && IsKeywordAt(sql, i, keyword)) return i;
            }
            return -1;
        }

        private static bool IsKeywordAt(string sql, int index, string keyword)
        {
            if (index < 0 || index + keyword.Length > sql.Length ||
                !string.Equals(sql.Substring(index, keyword.Length), keyword,
                    StringComparison.OrdinalIgnoreCase))
                return false;

            var beforeOk = index == 0 || !IsIdentifierChar(sql[index - 1]);
            var after = index + keyword.Length;
            return beforeOk && (after == sql.Length || !IsIdentifierChar(sql[after]));
        }

        private static bool IsIdentifierChar(char value) => char.IsLetterOrDigit(value) || value == '_';

        /// <summary>根据参数名在参数数组中查找对应的 SugarParameter</summary>
        private static SugarParameter ResolveParam(SugarParameter[] pars, string paramName)
        {
            return pars.FirstOrDefault(p =>
                string.Equals(p.ParameterName, paramName, StringComparison.OrdinalIgnoreCase));
        }
    }
}

using MF.Utils;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MF.Orm
{
    public static class ComponentSnTimeRangeTrackerExtensions
    {
        private const string ConfigTable = "component_sn_time_range";
        private const string InterceptorRegistrationKey = nameof(ComponentSnTimeRangeTrackerExtensions);

        /// <summary>
        /// 注册拦截器：每次 INSERT / UPDATE 涉及 component_sn 列时，
        /// 自动记录或更新 component_sn_time_range 中的时间范围。
        /// </summary>
        public static void UseAutoSnTimeRangeTracker(
            this ISqlSugarClient db,
            ILogger log)
        {
            SqlSugarInterceptorManager.RegisterInterceptor((sql, pars) =>
            {
                var trimSql = sql.TrimStart();

                if (!trimSql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase) &&
                    !trimSql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase))
                    return KeyValuePair.Create(sql, pars);

                if (trimSql.Contains(ConfigTable, StringComparison.OrdinalIgnoreCase))
                    return KeyValuePair.Create(sql, pars);

                var componentSnValue = ExtractComponentSnValue(trimSql, pars);
                if (componentSnValue == null)
                    return KeyValuePair.Create(sql, pars);

                var createTime = ExtractCreateTime(trimSql, pars) ?? DateTime.Now;
                var updateTime = ExtractUpdateTime(trimSql, pars) ?? DateTime.Now;

                UpsertComponentSnTimeRange(db, componentSnValue, createTime, updateTime, log);

                return KeyValuePair.Create(sql, pars);
            }, InterceptorRegistrationKey);
        }

        private static string ExtractComponentSnValue(string sql, SugarParameter[] pars)
        {
            if (string.IsNullOrWhiteSpace(sql) || pars == null || pars.Length == 0)
                return null;

            // --- 1. 处理 UPDATE 场景 ---
            // 兼容 [component_sn], `component_sn`, "component_sn" 以及参数名中可能包含的下划线和特殊数字
            var updateMatch = Regex.Match(sql,
                @"\bcomponent_sn\b\s*=\s*(?<param>[@:]\w+)",
                RegexOptions.IgnoreCase);

            if (updateMatch.Success)
            {
                var paramName = updateMatch.Groups["param"].Value;
                var p = ResolveParam(pars, paramName);
                return p?.Value?.ToString();
            }

            // --- 2. 处理 INSERT 场景 ---
            // 使用 RegexOptions.Singleline 允许 . 匹配换行符
            var insertMatch = Regex.Match(sql,
                @"INSERT\s+INTO\s+[\w`\[\]""]+\s*\((?<cols>[^)]+)\)\s*VALUES\s*(?<vals>[\s\S]+)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (insertMatch.Success)
            {
                var colsStr = insertMatch.Groups["cols"].Value;
                var valsStr = insertMatch.Groups["vals"].Value;

                // 精准提取 VALUES 后的第一组小括号内的参数，防止被批量插入的多组括号干扰
                var firstValueGroup = Regex.Match(valsStr, @"\((?<first>[\s\S]*?)\)", RegexOptions.Singleline);

                if (firstValueGroup.Success)
                {
                    var columns = SplitList(colsStr);
                    var values = SplitList(firstValueGroup.Groups["first"].Value);

                    for (int i = 0; i < columns.Count && i < values.Count; i++)
                    {
                        // 清理可能存在的各种数据库中括号、反引号、空格和换行
                        var colName = columns[i].Trim().Trim('[', ']', '`', '"');

                        if (colName.Equals("component_sn", StringComparison.OrdinalIgnoreCase))
                        {
                            var valToken = values[i].Trim();

                            // 判断如果是参数化查询
                            if (valToken.StartsWith("@") || valToken.StartsWith(":"))
                            {
                                var p = ResolveParam(pars, valToken);
                                return p?.Value?.ToString();
                            }

                            // 容错处理：如果 SQL 没走参数化，而是直接传的硬编码字符串值 'SN123456'
                            return valToken.Trim('\'', '"');
                        }
                    }
                }
            }

            return null;
        }

        private static DateTime? ExtractCreateTime(string sql, SugarParameter[] pars)
        {
            var updateMatch = Regex.Match(sql, @"\bcreate_time\b\s*=\s*(@\w+)", RegexOptions.IgnoreCase);
            if (updateMatch.Success)
            {
                var p = ResolveParam(pars, updateMatch.Groups[1].Value);
                if (p?.Value is DateTime dt) return dt;
                if (p?.Value != null && DateTime.TryParse(p.Value.ToString(), out var parsed))
                    return parsed;
            }

            var insertMatch = Regex.Match(sql,
                @"INSERT\s+INTO\s+\w+[\s\S]*?\(([^)]+)\)\s*VALUES\s*\(([^)]+)\)",
                RegexOptions.IgnoreCase);
            if (insertMatch.Success)
            {
                var columns = SplitList(insertMatch.Groups[1].Value);
                var values = SplitList(insertMatch.Groups[2].Value);
                for (int i = 0; i < columns.Count && i < values.Count; i++)
                {
                    if (columns[i].Equals("create_time", StringComparison.OrdinalIgnoreCase) &&
                        values[i].StartsWith("@"))
                    {
                        var p = ResolveParam(pars, values[i]);
                        if (p?.Value is DateTime dt) return dt;
                        if (p?.Value != null && DateTime.TryParse(p.Value.ToString(), out var parsed))
                            return parsed;
                    }
                }
            }

            return null;
        }

        private static DateTime? ExtractUpdateTime(string sql, SugarParameter[] pars)
        {
            var updateMatch = Regex.Match(sql, @"\bupdate_time\b\s*=\s*([@:][\w]+)", RegexOptions.IgnoreCase);
            if (!updateMatch.Success)
                return null;

            var p = ResolveParam(pars, updateMatch.Groups[1].Value);
            if (p?.Value is DateTime dt)
                return dt;

            if (p?.Value != null && DateTime.TryParse(p.Value.ToString(), out var parsed))
                return parsed;

            return null;
        }

        private static List<string> SplitList(string text)
        {
            var items = new List<string>();
            int depth = 0, start = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '(') depth++;
                else if (text[i] == ')') depth--;
                else if (text[i] == ',' && depth == 0)
                {
                    items.Add(text.Substring(start, i - start).Trim().Trim('\''));
                    start = i + 1;
                }
            }
            if (start < text.Length)
                items.Add(text.Substring(start).Trim().Trim('\''));
            return items;
        }

        private static void UpsertComponentSnTimeRange(
            ISqlSugarClient db,
            string componentSnValue,
            DateTime createTime,
            DateTime updateTime,
            ILogger log)
        {
            try
            {
                var now = DateTime.Now;
                var id = PubId.SnowflakeId.ToString();
                var snParam = new SugarParameter("@component_sn", componentSnValue);
                var updateTimeParam = new SugarParameter("@update_time", updateTime);

                var exists = db.Ado.GetInt(
                    $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {ConfigTable} WHERE component_sn = @component_sn) THEN 1 ELSE 0 END", snParam);

                if (exists == 1)
                {
                    db.Ado.ExecuteCommand(
                        $"UPDATE {ConfigTable} SET end_time = @update_time, update_time = @now " +
                        $"WHERE component_sn = @component_sn AND end_time < @update_time",
                        snParam, updateTimeParam, new SugarParameter("@now", now));
                }
                else
                {
                    db.Ado.ExecuteCommand(
                        $"INSERT INTO {ConfigTable} (id, component_sn, start_time, end_time, creator, create_time, update_time, state, inner_version) " +
                        $"VALUES (@id, @component_sn, @create_time, @update_time, 'system', @now, @now, '0', 0)",
                        new SugarParameter("@id", id),
                        new SugarParameter("@component_sn", componentSnValue),
                        new SugarParameter("@create_time", createTime),
                        new SugarParameter("@update_time", updateTime),
                        new SugarParameter("@now", now));
                }

                log.LogInformation(
                    $"SN时间范围跟踪：{componentSnValue} => {createTime:yyyy-MM-dd HH:mm:ss} ~ {updateTime:yyyy-MM-dd HH:mm:ss}");
            }
            catch (Exception ex)
            {
                log.LogWarning($"SN时间范围跟踪失败：{componentSnValue}，{ex.Message}");
            }
        }

        private static SugarParameter ResolveParam(SugarParameter[] pars, string paramName)
        {
            return pars.FirstOrDefault(p =>
                string.Equals(p.ParameterName, paramName, StringComparison.OrdinalIgnoreCase));
        }
    }
}

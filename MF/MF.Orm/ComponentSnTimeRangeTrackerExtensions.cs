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

                UpsertComponentSnTimeRange(db, componentSnValue, createTime, log);

                return KeyValuePair.Create(sql, pars);
            });
        }

        private static string ExtractComponentSnValue(string sql, SugarParameter[] pars)
        {
            var updateMatch = Regex.Match(sql, @"\bcomponent_sn\b\s*=\s*(@\w+)", RegexOptions.IgnoreCase);
            if (updateMatch.Success)
            {
                var p = ResolveParam(pars, updateMatch.Groups[1].Value);
                return p?.Value?.ToString();
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
                    if (columns[i].Equals("component_sn", StringComparison.OrdinalIgnoreCase) &&
                        values[i].StartsWith("@"))
                    {
                        var p = ResolveParam(pars, values[i]);
                        return p?.Value?.ToString();
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

        private static void UpsertComponentSnTimeRange(ISqlSugarClient db, string componentSnValue, DateTime createTime, ILogger log)
        {
            try
            {
                var now = DateTime.Now;
                var id = PubId.SnowflakeId.ToString();
                var snParam = new SugarParameter("@component_sn", componentSnValue);
                var ctParam = new SugarParameter("@ct", createTime);

                var exists = db.Ado.GetInt(
                    $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {ConfigTable} WHERE component_sn = @component_sn) THEN 1 ELSE 0 END", snParam);

                if (exists == 1)
                {
                    db.Ado.ExecuteCommand(
                        $"UPDATE {ConfigTable} SET end_time = @ct WHERE component_sn = @component_sn AND end_time < @ct",
                        snParam, ctParam);
                }
                else
                {
                    db.Ado.ExecuteCommand(
                        $"INSERT INTO {ConfigTable} (id, component_sn, start_time, end_time, creator, create_time, state, inner_version) " +
                        $"VALUES (@id, @component_sn, @ct, @ct, 'system', @now, '0', 0)",
                        new SugarParameter("@id", id),
                        new SugarParameter("@component_sn", componentSnValue),
                        new SugarParameter("@ct", createTime),
                        new SugarParameter("@now", now));
                }

                log.LogInformation($"SN时间范围跟踪：{componentSnValue} => {createTime:yyyy-MM-dd HH:mm:ss}");
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

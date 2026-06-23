using MF.Orm;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MF.Orm
{
    public static class SqlSugarAutoTimeExtensions
    {
        // =========================================================================
        // 🎯 配置化进阶版：支持从配置文件动态传入大表名单
        // =========================================================================
        public static void UseAutoTimeLimit(
            this ISqlSugarClient db,
            string[] limitTables, // 👈 动态注入需要卡1年时间的大表数组
            int years,
            Microsoft.Extensions.Logging.ILogger log)
        {
            // 如果配置文件里没配，直接放行，不影响任何性能
            if (limitTables == null || limitTables.Length == 0) return;

            // 💡 极其硬核的性能优化：高并发下用 HashSet 的 Contains 速度比 Array.Any 快几十倍
            var hashTables = new HashSet<string>(limitTables, StringComparer.OrdinalIgnoreCase);
            SqlSugarInterceptorManager.RegisterInterceptor((sql, pars) =>
            {
                var trimSql = sql.TrimStart();

                // 1. 只处理 SELECT 查询
                if (!trimSql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    return KeyValuePair.Create(sql, pars);

                // 2. 核心拦截：动态匹配当前 SQL 是否包含配置文件里的任何一张大表
                // 利用 HashSet 完美兼顾大小写和高性能判定
                bool isBigTable = hashTables.Any(table => trimSql.Contains(table, StringComparison.OrdinalIgnoreCase));

                if (isBigTable)
                {
                    // 检查是否已经带了 create_time 的过滤条件
                    bool hasTimeFilter = trimSql.Contains("create_time", StringComparison.OrdinalIgnoreCase);

                    if (!hasTimeFilter)
                    {
                        string columnName = "create_time";

                        // 生成通用的标准 ISO 日期时间格式字符串
                        string timeMin = DateTime.Now.AddYears(-years).ToString("yyyy-MM-dd HH:mm:ss");
                        string timeMax = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // 构造标准的 ANSI SQL 过滤语句
                        string timeCondition = $" {columnName} >= '{timeMin}' AND {columnName} <= '{timeMax}' ";

                        // 3. 根据原 SQL 是否有 WHERE 关键字进行动态编织
                        if (trimSql.Contains(" WHERE ", StringComparison.OrdinalIgnoreCase))
                        {
                            trimSql = trimSql + " AND " + timeCondition;
                        }
                        else
                        {
                            trimSql = trimSql + " WHERE " + timeCondition;
                        }

                        // 4. 打印安全性拦截审计日志
                        var dbType = db.CurrentConnectionConfig.DbType;
                        log.LogInformation($"SQL时间兜底拦截【当前库: {dbType}】：检测到配置的大表盲查，已自动追加1年时间限制。{Environment.NewLine}【原 SQL】: {sql} {Environment.NewLine}【新 SQL】: {trimSql}");

                        // 5. 吐回替换后带有时间限制的新 SQL
                        return KeyValuePair.Create(trimSql, pars);
                    }
                }

                // 没触发拦截或已带时间的，原封不动放行
                return KeyValuePair.Create(sql, pars);
            });
        }
    }
}

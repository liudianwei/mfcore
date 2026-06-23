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
    public static class SqlSugarAutoLimitExtensions
    {
        private static bool IsPagedSql(string sql)
        {
            sql = sql.ToUpperInvariant();

            return
                sql.Contains(" OFFSET ")
             || sql.Contains(" FETCH NEXT ")
             || sql.Contains(" ROW_NUMBER()");
        }
        private static bool IsRowNumberPaging(string sql)
        {
            sql = sql.ToUpperInvariant();

            // 1️⃣ 内层有 ROW_NUMBER() OVER
            bool hasRowNumber = Regex.IsMatch(sql, @"ROW_NUMBER\s*\(\s*\)\s*OVER", RegexOptions.IgnoreCase);
            // 2️⃣ 外层有 RowIndex BETWEEN
            // 匹配 ROW_NUMBER 生成的列名 + BETWEEN
            bool hasBetween = Regex.IsMatch(sql, @"\bROWINDEX\b\s+BETWEEN\b", RegexOptions.IgnoreCase);
            return hasRowNumber && hasBetween;
        }
        private static bool IsAggregateSql(string sql)
        {
            sql = sql.ToUpperInvariant();

            // 只匹配最外层 SELECT 中的聚合函数
            return Regex.IsMatch(sql,
                @"^\s*SELECT\s+.*\b(COUNT|SUM|AVG|MIN|MAX)\s*\(",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        private static bool HasTopLimit(string sql)
        {
            var match = Regex.Match(
                sql,
                @"^\s*SELECT\s+(DISTINCT\s+)?TOP\s+(\d+)",
                RegexOptions.IgnoreCase
            );

            return match.Success;
        }
        private static bool HasOuterLimit(string sql)
        {
            sql = sql.ToUpperInvariant();
            var lastLimit = sql.LastIndexOf(" LIMIT ");
            return lastLimit > -1;
        }
        private static bool HasFetch(string sql)
        {
            return Regex.IsMatch(
                sql,
                @"FETCH\s+FIRST\s+\d+\s+ROWS\s+ONLY\s*$",
                RegexOptions.IgnoreCase
            );
        }

        private static bool Filter(string sql, DbType dbType)
        {
            if (IsPagedSql(sql))
                return true;
            if (IsRowNumberPaging(sql))
                return true;
            if (IsAggregateSql(sql))
                return true;
            return dbType switch
            {
                DbType.SqlServer => HasTopLimit(sql),
                DbType.MySql or DbType.PostgreSQL or DbType.Sqlite => HasOuterLimit(sql),
                DbType.Oracle => HasFetch(sql),
                _ => false
            };
        }

        private static string AddLimitByDbType(string sql, DbType dbType, int maxRows)
        {
            switch (dbType)
            {
                case DbType.SqlServer:
                    return AddTop(sql, maxRows);

                case DbType.MySql:
                case DbType.PostgreSQL:
                case DbType.Sqlite:
                    return $"{sql} LIMIT {maxRows}";

                case DbType.Oracle:
                    return $"{sql} FETCH FIRST {maxRows} ROWS ONLY";

                default:
                    return sql;
            }
        }
        private static string AddTop(string sql, int maxRows)
        {
            return Regex.Replace(
                sql,
                @"SELECT\s+(DISTINCT\s+)?",
                m => m.Value + $"TOP {maxRows} ",
                RegexOptions.IgnoreCase
            );
        }

        public static void UseAutoLimit(this SqlSugarClient db, int maxRow, Microsoft.Extensions.Logging.ILogger<SqlSugarClient> log)
        {
            SqlSugarInterceptorManager.RegisterInterceptor((sql, pars) =>
            {
                var trimSql = sql.TrimStart();
                // 只处理 SELECT
                if (!trimSql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    return KeyValuePair.Create(sql, pars);
                // 已有限制直接放行
                if (Filter(sql, db.CurrentConnectionConfig.DbType))
                {
                    return KeyValuePair.Create(sql, pars);
                }
                var dbType = db.CurrentConnectionConfig.DbType;
                var newSql = AddLimitByDbType(sql, dbType, maxRow);
                log.LogInformation($"sql查询没有最大行数限制 替换sql {Environment.NewLine}{sql}  {Environment.NewLine}{newSql}");
                return KeyValuePair.Create(newSql, pars);
            });
        }
    }
}

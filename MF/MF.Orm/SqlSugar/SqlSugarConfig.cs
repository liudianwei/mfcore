using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace MF.Orm.SqlSugar
{
    public static class SqlSugarConfig
    {
        public static List<SqlFuncExternal> GetLambda()
        {
            //Lambda自定义解析
            var expMethods = new List<SqlFuncExternal>
                        {
                            new SqlFuncExternal()
                            {
                                UniqueMethodName = "ToDateFormat",
                                MethodValue = (expInfo, dbType, expContext) =>
                                {
                                    switch (dbType)
                                    {
                                        case DbType.SqlServer:
                                            return $"CONVERT (VARCHAR (10), {expInfo.Args[0].MemberName}, 121 )";

                                        case DbType.MySql:
                                            return $"DATE_FORMAT( {expInfo.Args[0].MemberName}, '%Y-%m-%d' ) ";

                                        case DbType.Sqlite:
                                            return $"date({expInfo.Args[0].MemberName})";

                                        case DbType.PostgreSQL:
                                        case DbType.Oracle:
                                            return $"to_date({expInfo.Args[0].MemberName},yyyy-MM-dd)";

                                        default:
                                            throw new Exception("未实现");
                                    }
                                }
                            },
                        };
            return expMethods;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        /// <returns></returns>
        public static (DbType, string) GetConnectionString(IConfiguration configuration)
        {
            string type = configuration["Orm:DbType"];
            var dbType = (DbType)Enum.Parse(typeof(DbType), type);
            return dbType switch
            {
                DbType.MySql => (DbType.MySql, configuration["Orm:MySqlConnectionString"]),
                DbType.SqlServer => (DbType.SqlServer, configuration["Orm:SqlServerConnectionString"]),
                DbType.Sqlite => (DbType.Sqlite, configuration["Orm:SqliteConnectionString"]),
                DbType.Oracle => (DbType.Oracle, configuration["Orm:OracleConnectionString"]),
                DbType.PostgreSQL => (DbType.PostgreSQL, configuration["Orm:PostgreSQLConnectionString"]),
                _ => throw new Exception("不支持的数据库类型"),
            };
        }
    }
}
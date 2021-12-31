using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


using MF.Orm.Repository;
using MF.Orm.Service;
using MF.Orm.SqlSugar;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MF.Extensions.DependencyInjection
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection RegisterBase(this IServiceCollection services, ServiceLifetime injection = ServiceLifetime.Scoped)
        {
            switch (injection)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
                    services.AddScoped(typeof(IBaseServices<>), typeof(BaseServices<>));
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
                    services.AddSingleton(typeof(IBaseServices<>), typeof(BaseServices<>));
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
                    services.AddTransient(typeof(IBaseServices<>), typeof(BaseServices<>));
                    break;
            }
            return services;
        }

        private static string PretySql(string sql)
        {
            return sql.Replace("SELECT", "SELECT\r\n")
                       .Replace(",", ",\r\n")
                       .Replace("FROM", "\r\nFROM")
                       .Replace("WHERE", "\r\nWHERE")
                       .Replace("ORDER", "\r\nORDER")
                       .Replace("LIMIT", "\r\nLIMIT")
                       .Replace("Left JOIN", "\r\nLeft JOIN")
                       .Replace(")  AND  (", ")\r\nAND (")
                       .Replace(")   AND (", ")\r\nAND (");
        }

        private static void SlowSql(SqlSugarClient db, ILogger log)
        {
            //执行时间超过1秒
            if (db.Ado.SqlExecutionTime.TotalSeconds > 1)
            {
                //代码CS文件名
                var fileName = db.Ado.SqlStackTrace.FirstFileName;
                //代码行数
                var fileLine = db.Ado.SqlStackTrace.FirstLine;
                //方法名
                var FirstMethodName = db.Ado.SqlStackTrace.FirstMethodName;
                //db.Ado.SqlStackTrace.MyStackTraceList[1].xxx 获取上层方法的信息

                log.LogInformation($"\tFileName: {fileName}\r\n\tFileLine: {fileLine}\r\n\tFirstMethodName: {FirstMethodName}");
            }
        }

        /// <summary>
        /// SqlSugar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarClient<T>(this IServiceCollection services, Action<ConnectionConfig> configAction, ServiceLifetime lifetime = ServiceLifetime.Scoped) where T : SqlSugarClient
        {
            services.GetMachineCodeString();
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(serviceProvider =>
                    {
                        var config = new ConnectionConfig()
                        {
                            ConfigureExternalServices = new ConfigureExternalServices
                            {
                                SqlFuncServices = SqlSugarConfig.GetLambda()
                            }
                        };
                        configAction.Invoke(config);
                        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                        var log = loggerFactory.CreateLogger<SqlSugarClient>();
                        var db = new SqlSugarClient(config);

                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        bool.TryParse(configuration["Log:SqlLog"], out bool flag);
                        //if (string.IsNullOrWhiteSpace(flag))
                        //{
                        //    flag = "false";
                        //}
                        if (flag)
                        {
                            db.Ado.IsEnableLogEvent = true;
                            //SQL执行前事件
                            //db.Aop.OnLogExecuting = (sql, pars) =>
                            //{
                            //    foreach (var item in pars)
                            //    {
                            //        sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                            //    }
                            //    sql = pretySql(sql);
                            //    log.LogInformation($"执行前SQL: \r\n{sql}");
                            //};
                            //SQL执行完事件
                            db.Aop.OnLogExecuted = (sql, pars) =>
                            {
                                //foreach (var item in pars)
                                //{
                                //    sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                                //}

                                sql = Formatt(sql, pars);
                                sql = PretySql(sql);
                                log.LogInformation($"执行后SQL: \r\n{sql}");
                                log.LogInformation($"执行时间: {db.Ado.SqlExecutionTime.TotalSeconds}");

                                SlowSql(db, log);
                            };
                            db.Aop.OnError = (exp) =>//执行SQL 错误事件
                            {
                                log.LogDebug(exp, exp.Sql);
                            };
                        }
                        else
                        {
                            db.Ado.IsEnableLogEvent = false;
                        }

                        return (T)db;
                    });
                    break;

                case ServiceLifetime.Scoped:
                    services.AddScoped(serviceProvider =>
                    {
                        var configS = new ConnectionConfig()
                        {
                            ConfigureExternalServices = new ConfigureExternalServices
                            {
                                SqlFuncServices = SqlSugarConfig.GetLambda()
                            }
                        };
                        configAction.Invoke(configS);
                        var db = new SqlSugarClient(configS);

                        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                        var log = loggerFactory.CreateLogger<SqlSugarClient>();
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        bool.TryParse(configuration["Log:SqlLog"], out bool flag);
                        //if (string.IsNullOrWhiteSpace(flag))
                        //{
                        //    flag = "false";
                        //}
                        if (flag)
                        {
                            db.Ado.IsEnableLogEvent = true;
                            //SQL执行前事件
                            //db.Aop.OnLogExecuting = (sql, pars) =>
                            //{
                            //    foreach (var item in pars)
                            //    {
                            //        sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                            //    }
                            //    sql = pretySql(sql);
                            //    log.LogInformation($"执行前SQL: \r\n{sql}");
                            //};
                            //SQL执行完事件
                            db.Aop.OnLogExecuted = (sql, pars) =>
                            {
                                //foreach (var item in pars)
                                //{
                                //    sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                                //}

                                sql = Formatt(sql, pars);
                                sql = PretySql(sql);
                                log.LogInformation($"执行后SQL: \r\n{sql}");
                                log.LogInformation($"执行时间: {db.Ado.SqlExecutionTime.TotalSeconds}");

                                SlowSql(db, log);
                            };
                            db.Aop.OnError = (exp) =>//执行SQL 错误事件
                            {
                                log.LogDebug(exp, exp.Sql);
                            };
                            db.Aop.OnDiffLogEvent = (diff) =>
                            {
                                // 审计日志
                            };
                        }
                        else
                        {
                            db.Ado.IsEnableLogEvent = false;
                        }

                        return (T)db;
                    });
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(serviceProvider =>
                    {
                        var configT = new ConnectionConfig()
                        {
                            ConfigureExternalServices = new ConfigureExternalServices
                            {
                                SqlFuncServices = SqlSugarConfig.GetLambda()
                            }
                        };
                        configAction.Invoke(configT);
                        var db = new SqlSugarClient(configT);

                        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                        var log = loggerFactory.CreateLogger<SqlSugarClient>();
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        bool.TryParse(configuration["Log:SqlLog"], out bool flag);
                        //if (string.IsNullOrWhiteSpace(flag))
                        //{
                        //    flag = "false";
                        //}
                        if (flag)
                        {
                            db.Ado.IsEnableLogEvent = true;
                            //SQL执行前事件
                            //db.Aop.OnLogExecuting = (sql, pars) =>
                            //{
                            //    foreach (var item in pars)
                            //    {
                            //        sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                            //    }
                            //    sql = pretySql(sql);
                            //    log.LogInformation($"执行前SQL: \r\n{sql}");
                            //};
                            //SQL执行完事件
                            db.Aop.OnLogExecuted = (sql, pars) =>
                            {
                                //foreach (var item in pars)
                                //{
                                //    sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                                //}

                                sql = Formatt(sql, pars);
                                sql = PretySql(sql);
                                log.LogInformation($"执行后SQL: \r\n{sql}");
                                log.LogInformation($"执行时间: {db.Ado.SqlExecutionTime.TotalSeconds}");

                                SlowSql(db, log);
                            };
                            db.Aop.OnError = (exp) =>//执行SQL 错误事件
                            {
                                log.LogDebug(exp, exp.Sql);
                            };
                        }
                        else
                        {
                            db.Ado.IsEnableLogEvent = false;
                        }

                        return (T)db;
                    });
                    break;
            }
            return services;
        }

        public static void InitDB(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["Orm:Init"] == null || configuration["Orm:Init"].ToString().ToLower() != "true")
            {
                Console.WriteLine("无需初始化数据库...");
                return;
            }
            var provider = services.BuildServiceProvider();
            SqlSugarClient db = provider.GetService<SqlSugarClient>();
            IWebHostEnvironment env = provider.GetRequiredService<IWebHostEnvironment>();

            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var log = loggerFactory.CreateLogger<SqlSugarClient>();

            Dictionary<string, Type> entitys = new Dictionary<string, Type>();
            //List<string> dllnames = new List<string>()
            //{
            //    "MDCenter.dll",
            //    "ProductCenter.dll",
            //    "ScadaCenter.dll",
            //    "UserCenter.dll",
            //    "MF.Modules.EnumType.Shared.dll",
            //    "MF.Modules.Factory.Shared.dll",
            //    "MF.Modules.Product.Shared.dll",
            //    "MF.Modules.UserCenter.Shared.dll"
            //};

            var dllsStr = "";
            string[] dllns = null;
            List<string> dllnames = new List<string>()
            {
                "MF.Orm.dll"
            };

            try
            {
                dllsStr = configuration["Orm:Dlls"];
                if (dllsStr == "")
                {
                    Console.WriteLine("配置Orm:Dlls没有找到或内容空，请填写包含数据库实体的dll名称");
                    return;
                }
                dllns = dllsStr.Split(",");
                if (dllns.Length == 0)
                {
                    Console.WriteLine("配置Orm:Dlls没有找到或内容空，请填写包含数据库实体的dll名称");
                    return;
                }

                dllnames = dllns.ToList();
            }
            catch (Exception ee) 
            {
                Console.WriteLine(ee.Message);
                Console.WriteLine(ee.StackTrace);
                return;
            }

            try
            {
                var path = Environment.CurrentDirectory;
                log.LogError("当前环境 " + env.EnvironmentName);
                if (env.EnvironmentName == "Development")
                {
                    path = Path.Combine(path, "bin", configuration["Orm:DllDir"]);
                }
                log.LogError("dll路径: " + path);
                foreach (var dllpath in dllnames)
                {
                    var ass = Assembly.LoadFrom(Path.Combine(path, dllpath+".dll"));
                    var types = ass.GetTypes().ToList();
                    types = types.Where(i => i.GetCustomAttribute<SugarTable>() != null).ToList();

                    foreach (var type in types)
                    {
                        var tablename = ((SugarTable)type.GetCustomAttribute(typeof(SugarTable))).TableName;
                        if (!entitys.ContainsKey(tablename))
                        {
                            entitys.Add(tablename, type);
                        }
                        else
                        {
                            log.LogError($"表实体：{tablename} 重复");
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                log.LogError("加载表实体模型dll异常");
                log.LogError(ee.Message);
                return;
            }
            Console.WriteLine($"{entitys.Count} 张表");
            //获取当前数据库所有表名称
            //db.EntityMaintenance.GetEntityInfo();

            //var lss = entitys.Keys.ToList();
            //lss.Sort();

            //foreach (var tnn in lss)
            //{
            //    Console.WriteLine(tnn);
            //}

            try
            {
                db.DbMaintenance.CreateDatabase();
            }
            catch (Exception ee)
            {
                log.LogError("加载表实体模型dll异常");
                Console.WriteLine(ee.Message);
                //TODO 退出服务程序
            }
            var til = db.DbMaintenance.GetTableInfoList();

            var tables = db.DbMaintenance.GetTableInfoList().OrderBy(i => i.Name).Distinct().ToList();
            if (tables.Count != 0)
            {
                tables.ForEach((n) =>
                {
                    db.DbMaintenance.DropTable(n.Name);
                });
            }

            foreach (var item in entitys.Values)
            {
                try
                {
                    db.CodeFirst.InitTables(item);
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                    Console.WriteLine(item.Name);
                }
            }

            // 创建表
            //db.CodeFirst.InitTables(entitys.Values.ToArray());

            Console.WriteLine("Init Table success");
            log.LogInformation("Init Table success");

            // 执行sql脚本
            switch (db.Context.CurrentConnectionConfig.DbType)
            {
                case DbType.MySql:
                    ExecuteSql(db, "mysql", log);
                    break;

                case DbType.SqlServer:
                    ExecuteSql(db, "sqlserver", log);
                    break;

                case DbType.Sqlite:
                    ExecuteSql(db, "sqlite", log);
                    break;

                case DbType.Oracle:
                    ExecuteSql(db, "oracle", log);
                    break;
            }
        }

        private static void ExecuteSql(SqlSugarClient db, string sqlName, ILogger log)
        {
            //var sqlpath = Path.Combine(Environment.CurrentDirectory, sqlName + ".sql");
            var sqlpath = Path.Combine(Environment.CurrentDirectory, "mysql.sql");
            if (File.Exists(sqlpath))
            {
                var sql = File.ReadAllText(sqlpath);
                if (sqlName == "sqlserver")
                {
                    sql = sql.Replace("`", "");
                    sql = sql.Replace("SET NAMES utf8mb4;", "");
                    sql = sql.Replace("SET FOREIGN_KEY_CHECKS = 0;", "");
                    sql = sql.Replace("SET FOREIGN_KEY_CHECKS = 1;", "");
                }
                Console.WriteLine(sqlName + " Init Record...");
                log.LogInformation(sqlName + " Init Record...");
                db.Ado.ExecuteCommand(sql);
            }
            else
            {
                Console.WriteLine("not found " + sqlpath);
                log.LogInformation("not found " + sqlpath);
            }
        }

        public static string Formatt(string sql, SugarParameter[] pars)
        {
            Dictionary<int, List<SugarParameter>> sorts = new Dictionary<int, List<SugarParameter>>();
            int maxlen = 0;
            foreach (var item in pars)
            {
                if (maxlen < item.ParameterName.Length)
                {
                    maxlen = item.ParameterName.Length;
                }

                if (sorts.TryGetValue(item.ParameterName.Length, out var objlist))
                {
                    objlist.Add(item);
                }
                else
                {
                    sorts.Add(item.ParameterName.Length, new List<SugarParameter>() { item });
                }
            }

            while (maxlen > 0)
            {
                if (sorts.TryGetValue(maxlen, out var list))
                {
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                        }
                    }
                }
                maxlen--;
            }
            return sql;
        }

        /// <summary>
        /// 替换第一个符合条件的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue">所要替换掉的值</param>
        /// <param name="newValue">所要替换的值</param>
        /// <returns>返回替换后的值 所要替换掉的值为空或Null，返回原值</returns>
        public static string ReplaceFirst(this string value, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
                return value;

            int idx = value.IndexOf(oldValue);
            if (idx == -1)
                return value;
            value = value.Remove(idx, oldValue.Length);
            return value.Insert(idx, newValue);
        }

        /// <summary>
        /// 替换最后一个符合条件的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue">所要替换掉的值</param>
        /// <param name="newValue">所要替换的值</param>
        /// <returns>返回替换后的值 所要替换掉的值为空或Null，返回原值</returns>
        public static string ReplaceLast(this string value, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
                return value;

            int idx = value.LastIndexOf(oldValue);
            if (idx == -1)
                return value;
            value = value.Remove(idx, oldValue.Length);
            return value.Insert(idx, newValue);
        }

        public static IServiceCollection AddEntityMap(this IServiceCollection services, string assemblyName, ServiceLifetime injection = ServiceLifetime.Scoped)
        {
            //var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            //if (assembly is null)
            //{
            //    throw new DllNotFoundException($"\"{assemblyName}\".dll不存在");
            //}
            //List<Di> dis = new List<Di>();

            //// 查找所有不是接口的类
            //var types = assembly.GetTypes().Where(o => !o.IsInterface).ToList();

            //// 找出实体类
            //var Entitys = types.Where(o => o.GetCustomAttribute<SugarTable>() != null).ToList();

            //foreach (var item in Entitys)
            //{
            //    var name = item.Name;
            //    var tableName = item.GetCustomAttribute<SugarTable>().TableName;
            //}
            ////Console.WriteLine(Entitys.Count);
            return services;
        }
    }
}
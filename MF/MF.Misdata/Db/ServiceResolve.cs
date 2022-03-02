using Common.Utils;
using MF.NetCoreApp;
using MF.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DBUtils
{
    /// <summary>
    /// 使用DI
    /// </summary>
    public static class ServiceResolve
    {
        private static IServiceProvider _serviceProvider = null;

        /// <summary>
        /// services.BuildServiceProvider()
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetServiceResolve(IServiceProvider serviceProvider)
        {
            //IServiceProvider serviceProvider1 = new ServiceCollection().BuildServiceProvider();
            _serviceProvider = serviceProvider;

            try
            {
                var strMachineCode = MachineCode.GetMachineCodeString();
                Console.WriteLine($"机器码:{strMachineCode}");
                var item = new Esnecil().CheckMisdataCr(strMachineCode);
                if (!item.Item1)
                {
                    var msg = $"授权失败,请联系管理员进行授权!Warning Message ===>{item.Item2}；机器码为===>{strMachineCode}";
                    Console.WriteLine(msg);
                    SystemLog.Fatal(msg);
                    throw new Exception(msg);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static T Resolve<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        public static T ResolveS<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public static IOptions<T> ResolveOption<T>() where T : class, new()
        {
            //var serviceProvider = new ServiceCollection().BuildServiceProvider();
            return _serviceProvider.GetService<IOptions<T>>();
        }

        public static T ResolveA<T>() where T : class
        {
            return _serviceProvider == null ? null : ActivatorUtilities.GetServiceOrCreateInstance<T>(_serviceProvider);
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
            //services.GetMachineCodeString();
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(serviceProvider =>
                    {
                        var configS = new ConnectionConfig()
                        {
                            ConfigureExternalServices = new ConfigureExternalServices
                            {
                            }
                        };
                        configAction.Invoke(configS);
                        var db = new SqlSugarClient(configS);

                        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                        var log = loggerFactory.CreateLogger<SqlSugarClient>();
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        bool.TryParse(configuration["ConnectronStr:SqlLog"], out bool flag);
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
                                foreach (var item in pars)
                                {
                                    sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                                }

                                log.LogInformation($"执行后SQL: \r\n{sql}");
                                log.LogInformation($"执行时间: {db.Ado.SqlExecutionTime.TotalSeconds}");
                                Console.WriteLine($"执行后SQL:{sql}");
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
            }
            return services;
        }

        /// <summary>
        /// SqlSugarScope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarScope<T>(this IServiceCollection services, Action<ConnectionConfig> configAction,
            ServiceLifetime lifetime = ServiceLifetime.Singleton) where T : SqlSugarScope
        {
            //services.GetMachineCodeString();
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(serviceProvider =>
                    {
                        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                        var log = loggerFactory.CreateLogger<SqlSugarScope>();
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        bool.TryParse(configuration["ConnectronStr:SqlLog"], out bool flag);
                        var configS = new ConnectionConfig()
                        {
                            ConfigureExternalServices = new ConfigureExternalServices
                            {
                            }
                        };
                        configAction.Invoke(configS);
                        var db = new SqlSugarScope(configS, dB =>
                         {
                             if (flag)
                             {
                                 dB.Ado.IsEnableLogEvent = true;
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
                                 dB.Aop.OnLogExecuted = (sql, pars) =>
                                 {
                                     foreach (var item in pars)
                                     {
                                         sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                                     }

                                     log.LogInformation($"执行后SQL: \r\n{sql}");
                                     log.LogInformation($"执行时间: {dB.Ado.SqlExecutionTime.TotalSeconds}");
                                     Console.WriteLine($"执行后SQL:{sql}");
                                 };
                                 dB.Aop.OnError = (exp) =>//执行SQL 错误事件
                                 {
                                     log.LogDebug(exp, exp.Sql);
                                 };
                                 dB.Aop.OnDiffLogEvent = (diff) =>
                                 {
                                     // 审计日志
                                 };
                             }
                             else
                             {
                                 dB.Ado.IsEnableLogEvent = false;
                             }
                         });

                        return (T)db;
                    });
                    break;
            }
            return services;
        }
    }
}
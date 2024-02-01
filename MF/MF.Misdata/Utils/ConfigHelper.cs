using Common.DBUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.IO;

namespace Common.Utils
{
    // <summary>
    /// 配置文件
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        ///
        /// </summary>
        public static IConfiguration config { get; set; } = null;

        public static string LoggerDirPath = Directory.GetCurrentDirectory();
        public static string LoggerBusinessName = "BusinessLog";
        public static string LoggerSystemName = "SystemLog";
        public static string ConfigJsonPath = Path.Combine(Directory.GetCurrentDirectory(), "config", "appsettings.json");

        /// <summary>
        ///  初始化
        /// </summary>
        /// <returns></returns>
        public static void Init()
        {
            try
            {
                IServiceCollection services = new ServiceCollection();
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton<IConfiguration>(serviceProvider =>
                {
                    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                    configurationBuilder.AddJsonFile(ConfigJsonPath);
                    return configurationBuilder.Build();
                });
                config = services.BuildServiceProvider().GetService<IConfiguration>();
                var sqlSugarConfig = SugarAccess.GetConnectionParam();
                services.AddSqlSugarScope<SqlSugarScope>(config =>
                {
                    config.DbType = sqlSugarConfig.Item1;
                    config.ConnectionString = sqlSugarConfig.Item2;
                    config.IsAutoCloseConnection = true;
                    config.InitKeyType = InitKeyType.Attribute;
                    config.MoreSettings = new ConnMoreSettings() { DisableNvarchar = true };//添加这一行 ,将参数全部转成varchar模式
                });
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                LoggerDirPath = GetAppseting("Logger:DirPath") ?? Directory.GetCurrentDirectory();
                LoggerBusinessName = GetAppseting("Logger:BusinessName") ?? "BusinessLog";
                LoggerSystemName = GetAppseting("Logger:SystemName") ?? "SystemLog";
                BusinessLog.Path = $"{LoggerDirPath}\\{LoggerBusinessName}";
                SystemLog.Path = $"{LoggerDirPath}\\{LoggerSystemName}";
                ServiceResolve.SetServiceResolve(serviceProvider);
            }
            catch (Exception e)
            {
                Console.WriteLine("Config初始化失败!");
                SystemLog.Fatal("Config初始化失败!", e);
                throw e;
            }
        }

        /// <summary>
        /// 获取配置 xx:xx:xx
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetAppseting(string index)
        {
            if (config == null)
            {
                throw new Exception("请先初始化 AppConfig");
            }
            return config[index];
        }
    }
}
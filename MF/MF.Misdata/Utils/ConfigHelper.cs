using Microsoft.Extensions.Configuration;

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
        private static IConfigurationRoot config = null;

        /// <summary>
        ///  初始化
        /// </summary>
        /// <returns></returns>
        public static void Init()
        {
            try
            {
                config = new ConfigurationBuilder()
                    .SetBasePath($"{Directory.GetCurrentDirectory()}/Config")
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
            }
            catch (Exception e)
            {
                SystemLog.Fatal("Config初始化失败", e);
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
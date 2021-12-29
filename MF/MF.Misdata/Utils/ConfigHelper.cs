using MF.NetCoreApp;
using MF.Utils;
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
                Console.WriteLine("Config初始化失败!");
                SystemLog.Fatal("Config初始化失败!", e);
                throw e;
            }
            try
            {
                var strMachineCode = MachineCode.GetMachineCodeString();
                Console.WriteLine($"机器码:{strMachineCode}");
                var item = new Esnecil().CheckMisdataCr(strMachineCode);
                if (!item.Item1)
                {
                    Console.WriteLine($"授权失败,请联系管理员进行授权!throw message===>{item.Item2}");
                    throw new Exception($"授权失败,请联系管理员进行授权!throw message===>{item.Item2}；机器码为===>{strMachineCode}");
                }
            }
            catch (Exception e)
            {
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
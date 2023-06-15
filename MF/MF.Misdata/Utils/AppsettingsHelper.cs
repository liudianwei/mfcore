using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Common.Utils
{
    public class AppsettingsHelper
    {
        public IConfiguration Configuration { get; set; }

        //定义一个用于保存静态变量的实例
        private static AppsettingsHelper instance = null;
        //定义一个保证线程同步的标识
        private static readonly object locker = new object();
        //构造函数为私有，使外界不能创建该类的实例
        public AppsettingsHelper()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "config", "appsettings.json")).Build();
        }
        public static AppsettingsHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null) instance = new AppsettingsHelper();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 根据索引获取配置项value值
        /// </summary>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public string GetConfigByIndex(string indexPath)
        {
            return Configuration[indexPath].ToString();//Configuration["MEASSettings:PrintName"];
        }

        /// <summary>
        /// 根据节点获取配置文件对象实例
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private IConfiguration GetConfigObjs(string className)
        {
            //string aa = Configuration.GetSection("MEASSettings").Get<MEASSettings>().PrintName;
            return Configuration.GetSection(className);//    MEASSettings bbbb = see.Get<MEASSettings>();
        }

        /// <summary>
        /// 根据具体对象获取对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">实体对象</param>
        /// <returns></returns>
        public object GetConfigObjByEntity<T>(T obj)
        {
            string className = Type.GetType(obj.ToString()).Name;
            return Configuration.GetSection(className).Get(Type.GetType(obj.ToString()));
        }
        /// <summary>
        /// 根据节点获取配置文件对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static T GetConfigObjs<T>(string options)
        {
            return Instance.GetConfigObjs(options).Get<T>();
        }
        //使用方式如下所示：
        ////方式一
        //string aa = Configuration.GetSection("MEASSettings").Get<MEASSettings>().PrintName;
        //MEASSettings aaa = Configuration.GetSection("MEASSettings").Get<MEASSettings>();
        ////方式二
        //string bb = ConfigHelper.Instance.GetConfigByIndex("MEASSettings:PrintName");
        ////方式三
        //IConfiguration cc = ConfigHelper.Instance.GetConfigObjs("MEASSettings");
        //MEASSettings ccc = cc.Get<MEASSettings>();
        ////方式四
        //MEASSettings set = new MEASSettings();
        //MEASSettings ddd = ConfigHelper.Instance.GetConfigObjByEntity<MEASSettings>(set) as MEASSettings;
        ////方式五
        //MEASSettings temp = ConfigHelper.Instance.GetConfigObjByEntity<MEASSettings>(new MEASSettings()) as MEASSettings;

    }
}

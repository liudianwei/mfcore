using System;
using System.Net;

namespace SystemFramework
{
    /// <summary>
    /// ApplicationConfig 的摘要说明。
    /// 获得应用程序配置文件基本参数
    /// </summary>
    public partial class ApplicationConfig
    {
        /// <summary>
        /// 监控属性代码
        /// </summary>
        public static int MES_Code = 1;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string connectionString_Access;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString_Access
        {
            get
            {
                return connectionString_Access;
            }
            set
            {
                connectionString_Access = value;
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string connectionString_MES;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString_MES
        {
            get
            {
                return connectionString_MES;
            }
            set
            {
                connectionString_MES = value;
            }
        }

        /// <summary>
        /// 特殊参数
        /// </summary>
        private static string specialpara;

        /// <summary>
        /// 特殊参数
        /// </summary>
        public static string SpecialPara
        {
            get
            {
                return specialpara;
            }
            set
            {
                specialpara = value;
            }
        }

        /// <summary>
        /// ("1"通过监控系统MIS保存数据) ("0"不通过监控系统MIS保存数据)
        /// </summary>
        private static string isSaveAlarm;

        /// <summary>
        /// ("1"通过监控系统MIS保存机床报警数据) ("0"不通过监控系统MIS保存机床报警数据)
        /// </summary>
        public static string IsSaveAlarm
        {
            get
            {
                return isSaveAlarm;
            }
            set
            {
                isSaveAlarm = value;
            }
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        public ApplicationConfig()
        {
            try
            {
                try
                {
                    MES_Code = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.GetValues("MES_Code")[0]);
                }
                catch
                {
                    MES_Code = 1;
                }

                try
                {
                    connectionString_MES = System.Configuration.ConfigurationManager.AppSettings.GetValues("ConnectionString_MES")[0].ToString();
                }
                catch { }
                try
                {
                    specialpara = System.Configuration.ConfigurationManager.AppSettings.GetValues("SpecialPara")[0].ToString();
                }
                catch { }
                try
                {
                    isSaveAlarm = System.Configuration.ConfigurationManager.AppSettings.GetValues("IsSaveAlarm")[0].ToString();
                }
                catch { }
            }
            catch (Exception error)
            {
                ApplicationLog.WriteLog(error, "ApplicationConfig()");
            }
        }
    }
}
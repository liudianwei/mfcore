using BizDataAccess;
using SqlSugar;
using System;
using System.Configuration;
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
        public static int MES_Code { get; set; } = 1;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string connectionString_Access { get; set; }

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
        private static string connectionString_MES { get; set; }

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
        private static string specialpara { get; set; }

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
        private static string isSaveAlarm { get; set; }

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
                    MES_Code = Convert.ToInt32(ConfigurationManager.AppSettings.GetValues("MES_Code")[0]);
                }
                catch
                {
                    MES_Code = 1;
                }

                try
                {
                    connectionString_MES = ConfigurationManager.AppSettings.GetValues("ConnectionString_MES")[0].ToString();
                    MesSqlDbManager.connectionString = ConfigurationManager.AppSettings.GetValues("ConnectionString_MES")[0].ToString();
                }
                catch { }
                try
                {
                    specialpara = ConfigurationManager.AppSettings.GetValues("SpecialPara")[0].ToString();
                }
                catch { }
                try
                {
                    isSaveAlarm = ConfigurationManager.AppSettings.GetValues("IsSaveAlarm")[0].ToString();
                }
                catch { }

                try
                {
                    string _dbType = "SqlServer";
                    try
                    {
                        _dbType = Convert.ToString(ConfigurationManager.AppSettings.GetValues("DbType")[0]);
                    }
                    catch
                    {
                        _dbType = "SqlServer";
                        ApplicationLog.WriteLog("未适配数据库类型:配置<DbType>将以默认值(" + _dbType + ")启动");
                    }
                    MesSqlDbManager.dbType = (DbType)Enum.Parse(typeof(DbType), _dbType);
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
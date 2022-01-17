using SqlSugar;
using System;
using System.Configuration;
using SystemFramework;

namespace BizDataAccess
{
    /// <summary>
    /// MES数据库
    /// </summary>
    public class MesSqlDbManager
    {
        /// <summary>
        /// ORM 默认映射SqlServer
        /// </summary>
        private static DbType dbType = DbType.SqlServer;

        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient GetInstance()
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

            switch (_dbType)
            {
                case "MySql":
                    dbType = DbType.MySql;
                    break;

                case "SqlServer":
                    dbType = DbType.SqlServer;
                    break;

                case "Sqlite":
                    dbType = DbType.Sqlite;
                    break;

                case "Oracle":
                    dbType = DbType.Oracle;
                    break;

                case "PostgreSQL":
                    dbType = DbType.PostgreSQL;
                    break;

                default:
                    dbType = DbType.SqlServer;
                    break;
            }

            SqlSugarClient db = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = ApplicationConfig.ConnectionString_MES,
                    DbType = dbType,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute //初始化主键和自增列信息到ORM的方式
                    });
            return db;
        }
    }
}
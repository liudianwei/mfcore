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
        public static DbType dbType { get; set; } = DbType.SqlServer;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string connectionString { get; set; } = string.Empty;

        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient GetInstance(DbType _dbType = DbType.SqlServer, string _connectionString = "")
        {
            if (_connectionString != "")
            {
                dbType = _dbType;
                connectionString = _connectionString;
            }
            SqlSugarClient db = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = connectionString,
                    DbType = dbType,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute //初始化主键和自增列信息到ORM的方式
                });
            return db;
        }
    }
}
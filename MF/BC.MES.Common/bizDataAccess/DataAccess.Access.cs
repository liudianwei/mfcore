//using System.Collections.Generic;
using System.Data;
using System.Collections.Specialized;

using System.Data.OleDb;
using SystemFramework;


namespace BizDataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DataAccess
    {
   
        /// <summary>
        /// 用于 Access
        /// 执行标准SQL语句，不要求返回结果，适合（增、删、改）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filename">Excel Access文件</param>
        /// <returns></returns>
        public static bool ExecuteSQL_OleDb_Access(string filename, string sql)
        {
            string ErrorMessage;
            string dbConnectionString_Access;
            dbConnectionString_Access = ConnectString_OleDb_Access(filename);

            return SQLCommon.ExecuteNonQuery_OleDb(sql, dbConnectionString_Access, out ErrorMessage);
        }
        
        
        /// <summary>
        /// 用于 Excel Access
        /// 执行标准SQL查询语句，返回记录集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filename"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable_OleDb_Access(string sql, string filename, out DataTable dt)
        {
            string ErrorMessage;
            string strConnectionstring;
            strConnectionstring = ConnectString_OleDb_Access(filename);
            ErrorMessage = "";
            return SQLCommon.ExecuteDataTable_OleDb(sql, strConnectionstring, out dt, out ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnectionstring"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable_OleDb_Access_Connectionstring(string sql, string strConnectionstring, out DataTable dt)
        {
            string ErrorMessage;
            ErrorMessage = "";
            return SQLCommon.ExecuteDataTable_OleDb(sql, strConnectionstring, out dt, out ErrorMessage);
        }        /// <summary>
        /// 根据Access文件名称获得Access连接字符串
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ConnectString_OleDb_Access(string filename)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename;
            return strConn;
        }

        /// <summary>
        /// 用于 Access
        /// 执行标准SQL语句，不要求返回结果，适合（增、删、改）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ConnectionString_Access"></param>
        /// <returns></returns>
        public static bool ExecuteSQL_OleDb_Access_ConnectionString_Access(string sql, string ConnectionString_Access)
        {
            string ErrorMessage;
            return SQLCommon.ExecuteNonQuery_OleDb(sql, ConnectionString_Access, out ErrorMessage);
        }

        /// <summary>
        /// 用于 Access
        /// 执行标准SQL语句，不要求返回结果，适合（增、删、改）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSQL_OleDb_Access(string sql)
        {
            string ErrorMessage;
            return SQLCommon.ExecuteNonQuery_OleDb(sql, ApplicationConfig.ConnectionString_Access, out ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSQL_OleDb_Access(StringCollection sql)
        {
            string ErrorMessage;
            return SQLCommon.ExecuteNonQuery_OleDb(sql, ApplicationConfig.ConnectionString_Access, out ErrorMessage);
        }
        /// <summary>
        /// 用于  Access
        /// 执行标准SQL查询语句，返回记录集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable_OleDb_Access(string sql, out DataTable dt)
        {
            string ErrorMessage;
            return SQLCommon.ExecuteDataTable_OleDb(sql, ApplicationConfig.ConnectionString_Access, out dt, out ErrorMessage);
        }

        /// <summary>
        /// 返回count
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ExecuteOutNum_OleDb_Access(string sql, out int count)
        {
            string errorMessage;
            return SQLCommon.ExecuteOutNum_OleDb(sql, ApplicationConfig.ConnectionString_Access, out  count, out  errorMessage);
        }
        /// <summary>
        /// 返回count
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filename"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ExecuteOutNum_OleDb_Access(string sql, string filename, out int count)
        {
            string errorMessage;
            string connectionString = ConnectString_OleDb_Access(filename);
            return SQLCommon.ExecuteOutNum_OleDb(sql, connectionString, out  count, out  errorMessage);
        }
        
        /// <summary>
        /// 增加Access数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        /// <param name="oleDbParameter"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable_OleDb_Insert_Access(string sql, DataTable dt, ref OleDbParameter[] oleDbParameter)
        {
            string errorMessage;
            return SQLCommon.ExecuteDataTable_OleDb_Insert(sql, ApplicationConfig.ConnectionString_Access, dt, ref oleDbParameter, out  errorMessage);
        }
                
        /// <summary>
        /// 执行Insert 命令，同时返回自动编号
        /// 保存到Access数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="newID"></param>
        public static bool ExecuteNonQuery_newID_OleDb(string sql, out long newID)
        {
            return SQLCommon.ExecuteNonQuery_newID_OleDb(sql, ApplicationConfig.ConnectionString_Access, out  newID);
        }
        /// <summary>
        /// 执行Insert 命令，同时返回自动编号
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filename"></param>
        /// <param name="newID"></param>
        public static bool ExecuteNonQuery_newID_OleDb(string sql,string filename, out long newID)
        {
            string strConnectionstring;
            strConnectionstring = ConnectString_OleDb_Access(filename);
            return SQLCommon.ExecuteNonQuery_newID_OleDb(sql, strConnectionstring, out  newID);
        }
    }
}

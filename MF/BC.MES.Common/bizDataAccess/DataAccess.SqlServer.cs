//using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Data.OleDb;
using SystemFramework;


namespace BizDataAccess
{
    public partial class DataAccess
    {
        /// <summary>
        /// 增加SqlServer数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        /// <param name="oleDbParameter"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable_OleDb_Insert_SqlServer(string sql, DataTable dt, ref OleDbParameter[] oleDbParameter)
        {
            string errorMessage;
            return SQLCommon.ExecuteDataTable_OleDb_Insert(sql, ApplicationConfig.ConnectionString_MES, dt, ref oleDbParameter, out  errorMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ExecuteOutNum(string sql, out int count)
        {
            string errorMessage;
            return SQLCommon.ExecuteOutNum(sql, ApplicationConfig.ConnectionString_MES, out  count, out  errorMessage);
        }
        /// <summary>
        /// 提供通用的调用存储过程的方法（更新数据）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="sqlParameters">存储过程参数数组</param>
        /// <param name="dt">增加数据的数据表</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure_Update(string procedureName, ref SqlParameter[] sqlParameters, DataTable dt)
        {
            string errorMessage;
            return SQLCommon.ExecuteStoredProcedure_Update(procedureName, ApplicationConfig.ConnectionString_MES, ref sqlParameters, dt, out  errorMessage);
        }
        /// <summary>
        /// 直接执行制定SQL语句.用于无返回条件的SQL语句执行，Insert ,delete update等。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>成功返回True  / 失败返回False</returns>
        public static bool ExecuteSQL(string sql)
        {
            string ErrorMessage;
            return SQLCommon.ExecuteNonQuery(sql, ApplicationConfig.ConnectionString_MES, out ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSQL(StringCollection sql)
        {
            string ErrorMessage;
            return SQLCommon.ExecuteNonQuery(sql, ApplicationConfig.ConnectionString_MES, out ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        public static bool ExecuteSQL(string sql, ref SqlParameter[] sqlParameters, DataTable tb)
        {
            string ErrorMessage;
            return SQLCommon.ExecuteSQL(sql, ref sqlParameters, ApplicationConfig.ConnectionString_MES, tb, out  ErrorMessage);
        }

        /// <summary>
        /// 直接执行制定SQL语句.用于无返回条件的SQL语句执行，Insert ,delete update等。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnectionstring"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns>成功返回True  / 失败返回False</returns>
        public static bool ExecuteSQL(string sql, string strConnectionstring, out string ErrorMessage)
        {
            ErrorMessage = "";
            return SQLCommon.ExecuteNonQuery(sql, strConnectionstring, out ErrorMessage);
        }

         /// <summary>
        /// 增加记录后，返回自动编号
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="newID"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery_newID(string sql, out long newID)
        {
            return SQLCommon.ExecuteNonQuery_newID(sql, out newID);
        }


        /// <summary>
        /// 执行指定SQL语句，用于执行查询后返回记录集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnectionstring"></param>
        /// <param name="ds">返回记录集</param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteDataSet(string sql, string strConnectionstring, out DataSet ds, out string ErrorMessage)
        {
            ErrorMessage = "";
            return SQLCommon.ExecuteDataset(sql, strConnectionstring, out ds, out ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnectionstring"></param>
        /// <param name="dt"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable(string sql, string strConnectionstring, out DataTable dt, out string ErrorMessage)
        {
            ErrorMessage = "";
            return SQLCommon.ExecuteDataTable(sql, strConnectionstring, out dt, out ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable(string sql, out DataTable dt)
        {
            string ErrorMessage = "";
            return SQLCommon.ExecuteDataTable(sql, ApplicationConfig.ConnectionString_MES, out dt, out ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnectionstring"></param>
        /// <param name="dr"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteDataReader(string sql, string strConnectionstring, out SqlDataReader dr, out string ErrorMessage)
        {
            ErrorMessage = "";
            return SQLCommon.ExecuteDataReader(sql, strConnectionstring, out dr, out ErrorMessage);
        }
    }
}

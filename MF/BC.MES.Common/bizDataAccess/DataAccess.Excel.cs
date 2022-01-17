//using System.Collections.Generic;
using System.Data;
using System.Collections.Specialized;

using System.Data.OleDb;


namespace BizDataAccess
{
    public partial class DataAccess
    {
        /// <summary>
        /// 获得Excel连接
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ConnectString_OleDb(string filename)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=\"" + filename + "\""; ;
            strConn += ";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1\"";
            return strConn;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static OleDbConnection getConn_OleDb(string filename)
        {
            string strConnectionstring = ConnectString_OleDb(filename);
            OleDbConnection conn = new OleDbConnection(strConnectionstring);
            conn.Open();
            return conn;
        }
        /// <summary>
        /// 增加Excel数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filename"></param>
        /// <param name="dt"></param>
        /// <param name="oleDbParameter"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable_OleDb_Insert_Excel(string sql, string filename, DataTable dt, ref OleDbParameter[] oleDbParameter)
        {
            string errorMessage;
            string connectionString = ConnectString_OleDb(filename);
            return SQLCommon.ExecuteDataTable_OleDb_Insert(sql, connectionString, dt, ref oleDbParameter, out  errorMessage);
        }

        /// <summary>
        /// 用于 Excel Access
        /// 执行标准SQL语句，不要求返回结果，适合（增、删、改）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filename">Excel Access文件</param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteSQL_OleDb(string sql, string filename, out string ErrorMessage)
        {
            string strConnectionstring;
            strConnectionstring = ConnectString_OleDb(filename);
            ErrorMessage = "";
            return SQLCommon.ExecuteNonQuery_OleDb(sql, strConnectionstring, out ErrorMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filename"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteSQL_OleDb(StringCollection sql, string filename, out string ErrorMessage)
        {
            string strConnectionstring;
            strConnectionstring = ConnectString_OleDb(filename);
            ErrorMessage = "";
            return SQLCommon.ExecuteNonQuery_OleDb(sql, strConnectionstring, out ErrorMessage);
        }

        /// <summary>
        /// 用于 Excel Access
        /// 执行标准SQL查询语句，返回记录集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="filename"></param>
        /// <param name="dt"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteDataTable_OleDb(string sql, string filename, out DataTable dt, out string ErrorMessage)
        {
            string strConnectionstring;
            strConnectionstring = ConnectString_OleDb(filename);
            ErrorMessage = "";
            return SQLCommon.ExecuteDataTable_OleDb(sql, strConnectionstring, out dt, out ErrorMessage);
        }
    }
}

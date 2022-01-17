using System;
//using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Data.OleDb;
using SystemFramework;


namespace BizDataAccess
{
    public partial class DataAccess
    {

        /// <summary>
        /// 创建增加数据用表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable GetInsertTable(DataTable dt)
        {
            DataTable dt1;
            DataRow row;

            dt1 = dt.Clone();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row = dt1.NewRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row[j] = dt.Rows[i][j];
                }
                dt1.Rows.Add(row);

            }

            return dt1;
        }
    
      


        /// <summary>
        /// 获取EXCEL的表 表名字列 
        /// </summary>
        /// <param name="p_ExcelFile">Excel文件</param>
        /// <returns>数据表</returns>
        public static DataTable GetExcelTableName(string p_ExcelFile)
        {
            try
            {
                if (System.IO.File.Exists(p_ExcelFile))
                {
                    OleDbConnection _ExcelConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=\"Excel 8.0\";Data Source=" + p_ExcelFile);
                    _ExcelConn.Open();
                    DataTable _Table = _ExcelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    _ExcelConn.Close();
                    return _Table;
                }
                return null;
            }
            catch
            {
                return null;
            }
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnectionstring"></param>
        /// <returns></returns>
        public static SqlConnection getConn(string strConnectionstring)
        {
            SqlConnection conn = new SqlConnection(strConnectionstring);
            conn.Open();
            return conn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="mTrans"></param>
        /// <param name="sql"></param>
        public static void TranExecuteNonQuery(SqlConnection conn, SqlTransaction mTrans, string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Transaction = mTrans; 
            cmd.ExecuteNonQuery();
        }



        /// <summary>
        /// 执行指定Sql 语句，返回所影响的行数。
        /// </summary>
        /// <param name="sql">拼接出来的SQL语句。</param>
        /// <param name="strConnectionstring"></param>
        /// <param name="count">返回所影响的记录条数</param>
        /// <param name="ErrorMessage">ErrorMessage</param>
        /// <returns>True  /  False</returns>
        public static bool ExecuteSql_count(string sql,string strConnectionstring, out int count,out string ErrorMessage)
        {
            SqlParameter[] thisParms = new SqlParameter[2];
            thisParms[0] = new System.Data.SqlClient.SqlParameter("@sql", sql);
            thisParms[0].Direction = ParameterDirection.Input;
            thisParms[1] = new System.Data.SqlClient.SqlParameter("@count", SqlDbType.Int, 32);
            thisParms[1].Direction = ParameterDirection.Output;

            count = 0;
            ErrorMessage = "";
            if (SQLCommon.ExecuteStoredProcedure("sp_xt_executesql_count", strConnectionstring, ref thisParms, out ErrorMessage))
            {
                count = Convert.ToInt32(thisParms[1].Value.ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行指定插入 Sql 语句，返回当前记录的新ID。
        /// </summary>
        /// <param name="sql">拼接出来的SQL语句。</param>
        /// <param name="strConnectionstring"></param>
        /// <param name="tablename"></param>
        /// <param name="newID">返回当前记录的新ID</param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteSql_newID(string sql,string strConnectionstring, string tablename, out long newID, out string ErrorMessage)
        {
            SqlParameter[] thisParms = new SqlParameter[3];
            thisParms[0] = new System.Data.SqlClient.SqlParameter("@sql", sql);
            thisParms[0].Direction = ParameterDirection.Input;
            thisParms[1] = new System.Data.SqlClient.SqlParameter("@newID", SqlDbType.BigInt, 64);
            thisParms[1].Direction = ParameterDirection.Output;
            thisParms[2] = new System.Data.SqlClient.SqlParameter("@tablename", tablename);
            thisParms[2].Direction = ParameterDirection.Input;

            newID = 0;
            ErrorMessage = "";
            if (SQLCommon.ExecuteStoredProcedure("sp_xt_insertRecord_newID", strConnectionstring, ref thisParms, out ErrorMessage))
            {
                newID = Convert.ToInt32(thisParms[1].Value.ToString());
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="strConnectionstring"></param>
        /// <param name="tablename"></param>
        /// <param name="newID"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteSql_newID(string sql,string strConnectionstring,string tablename, out int newID,out string ErrorMessage)
        {
            SqlParameter[] thisParms = new SqlParameter[3];
            thisParms[0] = new System.Data.SqlClient.SqlParameter("@sql", sql);
            thisParms[0].Direction = ParameterDirection.Input;
            thisParms[1] = new System.Data.SqlClient.SqlParameter("@newID", SqlDbType.Int, 32);
            thisParms[1].Direction = ParameterDirection.Output;
            thisParms[2] = new System.Data.SqlClient.SqlParameter("@tablename", tablename);
            thisParms[2].Direction = ParameterDirection.Input;

            newID = 0;
            ErrorMessage = "";
            if (SQLCommon.ExecuteStoredProcedure("sp_xt_insertRecord_newID", strConnectionstring, ref thisParms, out ErrorMessage))
            {
                newID = Convert.ToInt32(thisParms[1].Value.ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据输入参数，对指定表进行分页显示
        /// </summary>
        /// <param name="tbname">要进行分页的表/视图的名称</param>
        /// <param name="strConnectionstring"></param>
        /// <param name="fieldkey">表或者视图的主键</param>
        /// <param name="where">进行过滤的条件</param>
        /// <param name="fieldshow">需要显示的字段名称</param>
        /// <param name="fieldorder">排序的条件，直接输入字段名 +desc/asc，例如id desc,name asc</param>
        /// <param name="pagecurrent">当前页的页码</param>
        /// <param name="pagesize">每一页的显示的数据量</param>
        /// <param name="pagecount">返回页码总数</param>
        /// <param name="itemcount">返回数据总数</param>
        /// <param name="ds"></param>
        /// <param name="ErrorMessage">返回错误</param>
        /// <returns>True / False</returns>
        public static bool ExecPageQuery(string tbname,string strConnectionstring, string fieldkey, string where, string fieldshow, string fieldorder, int pagecurrent, int pagesize, out int pagecount, out int itemcount,out DataSet ds, out string ErrorMessage)
        {
            SqlParameter[] thisParms = new SqlParameter[9];
            thisParms[0] = new System.Data.SqlClient.SqlParameter("@tbname", tbname);
            thisParms[0].Direction = ParameterDirection.Input;
            thisParms[1] = new System.Data.SqlClient.SqlParameter("@fieldkey", fieldkey);
            thisParms[1].Direction = ParameterDirection.Input;
            thisParms[2] = new System.Data.SqlClient.SqlParameter("@where", where);
            thisParms[2].Direction = ParameterDirection.Input;
            thisParms[3] = new System.Data.SqlClient.SqlParameter("@fieldshow",fieldshow);
            thisParms[3].Direction = ParameterDirection.Input;
            thisParms[4] = new System.Data.SqlClient.SqlParameter("@fieldorder", fieldorder);
            thisParms[4].Direction = ParameterDirection.Input;
            thisParms[5] = new System.Data.SqlClient.SqlParameter("@pagecurrent", pagecurrent);
            thisParms[5].Direction = ParameterDirection.Input;
            thisParms[6] = new System.Data.SqlClient.SqlParameter("@pagesize", pagesize);
            thisParms[6].Direction = ParameterDirection.Input;
            thisParms[7] = new System.Data.SqlClient.SqlParameter("@pagecount", SqlDbType.Int,32);
            thisParms[7].Direction = ParameterDirection.Output;
            thisParms[8] = new System.Data.SqlClient.SqlParameter("@itemcount", SqlDbType.Int,32);
            thisParms[8].Direction = ParameterDirection.Output;

            pagecount = 0;
            itemcount = 0;
            ErrorMessage = "";
            if (SQLCommon.ExecuteStoredProcedure("sp_xt_pagesplit", strConnectionstring, ref thisParms, out ds, out ErrorMessage))
            {
                pagecount = Convert.ToInt32(thisParms[7].Value.ToString());
                itemcount = Convert.ToInt32(thisParms[8].Value.ToString());
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据输入参数，对指定表进行分页显示
        /// </summary>
        /// <param name="tbname">要进行分页的表/视图的名称</param>
        /// <param name="strConnectionstring">表或者视图的主键</param>
        /// <param name="fieldkey">表或者视图的主键</param>
        /// <param name="where">进行过滤的条件</param>
        /// <param name="fieldshow">需要显示的字段名称</param>
        /// <param name="fieldorder">排序的条件，直接输入字段名 +desc/asc，例如id desc,name asc</param>
        /// <param name="filedgroup">排序的条件，直接输入字段名 +desc/asc，例如id desc,name asc</param>
        /// <param name="pagecurrent">当前页的页码</param>
        /// <param name="pagesize">每一页的显示的数据量</param>
        /// <param name="pagecount">返回页码总数</param>
        /// <param name="itemcount">返回数据总数</param>
        /// <param name="ds"></param>
        /// <param name="ErrorMessage">返回错误</param>
        /// <returns>True / False</returns>
        public static bool ExecPageQuery_GroupBy(string tbname, string strConnectionstring, string fieldkey, string where, string fieldshow, string fieldorder,string filedgroup ,int pagecurrent, int pagesize, out int pagecount, out int itemcount, out DataSet ds, out string ErrorMessage)
        {
            SqlParameter[] thisParms = new SqlParameter[10];
            thisParms[0] = new System.Data.SqlClient.SqlParameter("@tbname", tbname);
            thisParms[0].Direction = ParameterDirection.Input;
            thisParms[1] = new System.Data.SqlClient.SqlParameter("@fieldkey", fieldkey);
            thisParms[1].Direction = ParameterDirection.Input;
            thisParms[2] = new System.Data.SqlClient.SqlParameter("@where", where);
            thisParms[2].Direction = ParameterDirection.Input;
            thisParms[3] = new System.Data.SqlClient.SqlParameter("@fieldshow", fieldshow);
            thisParms[3].Direction = ParameterDirection.Input;
            thisParms[4] = new System.Data.SqlClient.SqlParameter("@fieldorder", fieldorder);
            thisParms[4].Direction = ParameterDirection.Input;
            thisParms[5] = new System.Data.SqlClient.SqlParameter("@filedgroup", filedgroup);
            thisParms[5].Direction = ParameterDirection.Input;
            thisParms[6] = new System.Data.SqlClient.SqlParameter("@pagecurrent", pagecurrent);
            thisParms[6].Direction = ParameterDirection.Input;
            thisParms[7] = new System.Data.SqlClient.SqlParameter("@pagesize", pagesize);
            thisParms[7].Direction = ParameterDirection.Input;
            thisParms[8] = new System.Data.SqlClient.SqlParameter("@pagecount", SqlDbType.Int, 32);
            thisParms[8].Direction = ParameterDirection.Output;
            thisParms[9] = new System.Data.SqlClient.SqlParameter("@itemcount", SqlDbType.Int, 32);
            thisParms[9].Direction = ParameterDirection.Output;

            pagecount = 0;
            itemcount = 0;
            ErrorMessage = "";
            if (SQLCommon.ExecuteStoredProcedure("sp_xt_pagesplit_GroupBy", strConnectionstring, ref thisParms, out ds, out ErrorMessage))
            {
                pagecount = Convert.ToInt32(thisParms[8].Value.ToString());
                itemcount = Convert.ToInt32(thisParms[9].Value.ToString());
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intGrantee"></param>
        /// <param name="intGranteeType"></param>
        /// <param name="intObjectId"></param>
        /// <param name="intOjbectType"></param>
        /// <param name="strConnectionString"></param>
        /// <param name="strErrmessage"></param>
        /// <returns></returns>
        public static bool addGrants(int intGrantee, int intGranteeType, int intObjectId, int intOjbectType,string strConnectionString,out string strErrmessage)
        {
            string sql = "insert into grants (grantee,granteetype,objectid,objecttype,createtime,creator,creatorid) values("
                +intGrantee + ","
                +intGranteeType + ","
                +intObjectId + ","
                + intOjbectType + ",getdate(),'SYSTEM',0)";
            if (ExecuteSQL(sql, strConnectionString, out strErrmessage))
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intGrantee"></param>
        /// <param name="intGranteeType"></param>
        /// <param name="intObjectId"></param>
        /// <param name="intOjbectType"></param>
        /// <param name="strConnectionString"></param>
        /// <param name="strErrmessage"></param>
        /// <returns></returns>
        public static bool delGrants(int intGrantee, int intGranteeType, int intObjectId, int intOjbectType, string strConnectionString, out string strErrmessage)
        {

            string sql = "update grants set deleted=1 where grantee=" + intGrantee + " and granteetype=" + intGranteeType + " and objectid=" + intObjectId + " and objecttype=" + intOjbectType;
            if (ExecuteSQL(sql, strConnectionString, out strErrmessage))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 提供通用的调用存储过程的方法（增加数据的数据表的到数据库的调用）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="sqlParameters">存储过程参数数组</param>
        /// <param name="dt">增加数据的数据表</param>
        /// <returns></returns>
		public static bool ExecuteStoredProcedure(string procedureName, ref SqlParameter[] sqlParameters,  DataTable dt)
		{
			string errorMessage;
			return SQLCommon.ExecuteStoredProcedure( procedureName,  ApplicationConfig.ConnectionString_MES,  ref sqlParameters,   dt, out  errorMessage);
		}
        /// <summary>
        /// 提供通用的调用存储过程的方法（增加数据的数据表的到数据库的调用）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="sqlParameters">存储过程参数数组</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure(string procedureName, ref SqlParameter[] sqlParameters)
        {
            string errorMessage;
            return SQLCommon.ExecuteStoredProcedure(procedureName, ApplicationConfig.ConnectionString_MES, ref sqlParameters, out  errorMessage);
        }


        /// <summary>
        /// 提供通用的调用存储过程的方法（增加数据的数据表的到数据库的调用）
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool ExecuteStoredProcedure(string procedureName, out DataTable dt)
        {
            string errorMessage;
            return SQLCommon.ExecuteStoredProcedure(procedureName, ApplicationConfig.ConnectionString_MES, out dt, out  errorMessage);
        }
        /// <summary>
        /// 执行带参数的存储过程
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool ExecuteStoredProcedure(string procedureName, ref SqlParameter[] sqlParameters, out DataTable dt)
        {
            string errorMessage;
            DataSet ds;
            SQLCommon.ExecuteStoredProcedure(procedureName, ApplicationConfig.ConnectionString_MES, ref sqlParameters,out ds, out  errorMessage);
            try
            {
                dt = ds.Tables[0];

                return true;
            }
            catch 
            {
                dt = null;
                return false;
            
            }
        }

    }
}
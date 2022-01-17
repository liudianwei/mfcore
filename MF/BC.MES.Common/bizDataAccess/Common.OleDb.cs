using System;
//using System.Collections.Generic;
using System.Data;
using System.Collections.Specialized;
using System.Data.OleDb;

using SystemFramework;

namespace BizDataAccess
{

    /// <summary>
    /// 数据访问类——提供通用的数据访问方法
    /// </summary>
    public partial class SQLCommon
    {

        /// <summary>
        /// 执行标准SQL查询语句，返回记录集
        /// </summary>
        /// <param name="sql">标准SQL查询语句</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="dt">查询结果记录集</param>
        /// <param name="oleDbParameter"></param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteDataTable_OleDb_Insert(string sql, string connectionString, DataTable dt, ref OleDbParameter[] oleDbParameter, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        using (OleDbDataAdapter dsCommand = new OleDbDataAdapter())
                        {
                            dsCommand.InsertCommand = new OleDbCommand(sql, conn);
                            for (int i = 0; i < oleDbParameter.Length; i++)
                            {
                                oleDbParameter[i].IsNullable = true;
                                dsCommand.InsertCommand.Parameters.Add(oleDbParameter[i]);
                            }
                            if (dt == null) return false;
                            dt.AcceptChanges();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i].SetAdded();
                            }
                            dsCommand.Update(dt);
                        }
                        dt.AcceptChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.ToString();
                        ApplicationLog.WriteLog(e, "\r\nExecuteDataTable_OleDb\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                ApplicationLog.WriteLog(e, "\r\n连接字符串：\r\n" + connectionString);
            }
            return result;
        }
        /// <summary>
        /// 执行标准SQL查询语句，返回记录集
        /// </summary>
        /// <param name="sql">标准SQL查询语句</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="dt">查询结果记录集</param>
        /// <param name="oleDbParameter">参数</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteDataTable_OleDb_Update(string sql, string connectionString, DataTable dt, ref OleDbParameter[] oleDbParameter, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            //dt = new DataTable();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbDataAdapter dsCommand = new OleDbDataAdapter())
                    {
                        dsCommand.UpdateCommand = new OleDbCommand(sql, conn);
                        for (int i = 0; i < oleDbParameter.Length; i++)
                        {
                            dsCommand.UpdateCommand.Parameters.Add(oleDbParameter[i]);
                        }
                        if (dt == null) return false;
                        dt.AcceptChanges();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].SetModified();//.SetAdded();
                        }
                        dsCommand.Update(dt);
                    }
                    dt.AcceptChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteDataTable_OleDb\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// 用于 Excel Access
        /// 执行标准SQL语句，不要求返回结果，适合（增、删、改）
        /// </summary>
        /// <param name="sql">标准SQL语句</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteNonQuery_OleDb(string sql, string connectionString, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {

                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery_OleDb\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 用于 Excel Access
        /// 执行标准SQL语句集合，不要求返回结果，适合（增、删、改）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery_OleDb(StringCollection sql, string connectionString, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    for (int i = 0; i < sql.Count; i++)
                    {
                        using (OleDbCommand cmd = new OleDbCommand(sql[i].ToString(), conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery_OleDb\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// 用于 Excel Access
        /// 执行标准SQL查询语句，返回记录集
        /// </summary>
        /// <param name="sql">标准SQL查询语句</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="ds">查询结果记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteDataset_OleDb(string sql, string connectionString, out DataSet ds, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            ds = new DataSet();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {


                try
                {
                    using (OleDbDataAdapter dsCommand = new OleDbDataAdapter())
                    {
                        dsCommand.SelectCommand = new OleDbCommand(sql, conn);
                        conn.Open();
                        dsCommand.Fill(ds);
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteDataset_OleDb(返回DataSet)\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// 执行标准SQL查询语句，返回记录集
        /// </summary>
        /// <param name="sql">标准SQL查询语句</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="dt">查询结果记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteDataTable_OleDb(string sql, string connectionString, out DataTable dt, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            dt = new DataTable("TableName");
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbDataAdapter dsCommand = new OleDbDataAdapter())
                    {
                        dsCommand.SelectCommand = new OleDbCommand(sql, conn);
                        dsCommand.Fill(dt);
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteDataTable_OleDb\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 执行标准SQL查询语句，返回记录集
        /// </summary>
        /// <param name="sql">标准SQL查询语句</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="ds">查询结果记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteDataTable_OleDb(string sql, string connectionString, out DataSet ds, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            ds = new DataSet();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OleDbDataAdapter dsCommand = new OleDbDataAdapter())
                    {
                        dsCommand.SelectCommand = new OleDbCommand(sql, conn);
                        dsCommand.Fill(ds);
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteDataTable_OleDb(返回DataSet)\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return result;
        }
        /// <summary>
        /// 返回符合条件的记录行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <param name="count"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteOutNum_OleDb(string sql, string connectionString, out int count, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                count = 0;
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        object obj = cmd.ExecuteScalar();
                        if (obj != null)
                        {
                            count = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                        }
                        else
                            count = 0;
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteOutNum\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 增加记录，同时返回自动编号
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <param name="newID"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery_newID_OleDb(string sql, string connectionString, out long newID)
        {
            string errorMessage;
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                newID = 0;
                try
                {
                    conn.Open();
                    using (OleDbTransaction myTransaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                        {
                            myTransaction.Commit();
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "select @@identity as id";
                            newID = Convert.ToInt64(cmd.ExecuteScalar());
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteOutNum\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }

        }
    }
}

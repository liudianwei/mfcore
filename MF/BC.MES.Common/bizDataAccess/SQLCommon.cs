using System;
//using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
//using System.Data.OleDb;

using SystemFramework;

namespace BizDataAccess
{

    /// <summary>
    /// 数据访问类——提供通用的数据访问方法
    /// </summary>
    public partial class SQLCommon
    {
        /// <summary>
        /// SqlServer连接状态
        /// =true 连接正常
        /// =false 连接失败
        /// </summary>
        static bool sqlServerConnectionStatus = true;
        /// <summary>
        /// SqlServer连接状态
        /// =true 连接正常
        /// =false 连接失败
        /// </summary>
        public static bool SqlServerConnectionStatus
        {
            get
            {
                return sqlServerConnectionStatus;
            }
            set
            {
                // SqlServer 与数据通讯状态
                MisDataOnChange.tagDataSqlServerConnectionStatus.InvokeTagData(value);
                sqlServerConnectionStatus = value;
            }
        }
        /// <summary>
        /// 测试与数据的连接
        /// SqlServer连接状态
        /// =true 连接正常
        /// =false 连接失败
        /// </summary>
        /// <returns></returns>
        public static void TestConnection()
        {
            string sql = "select top 1 uid from sysusers ";
            string connectionString = ApplicationConfig.ConnectionString_MES;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                SqlServerConnectionStatus = true;
                // SqlServer 与数据通讯状态
                MisDataOnChange.tagDataSqlServerConnectionStatus.InvokeTagData(SqlServerConnectionStatus);
                return;
            }
            catch (Exception e)
            {
                //SqlServerConnectionStatus = false;
                ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                // SqlServer 与数据通讯状态
                MisDataOnChange.tagDataSqlServerConnectionStatus.InvokeTagData(SqlServerConnectionStatus);
                return;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        /// <summary>
        /// 测试与数据的连接
        /// SqlServer连接状态
        /// =true 连接正常
        /// =false 连接失败
        /// </summary>
        /// <returns></returns>
        public static bool TestConnectionWorkStart()
        {
            bool isOK = true;
            string sql = "select top 1 uid from sysusers ";
            string connectionString = ApplicationConfig.ConnectionString_MES;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return isOK;
            }
            catch
            {
                isOK = false;
                return isOK;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        /// <summary>
        /// 执行标准SQL语句，不要求返回结果，适合（增、删、改）
        /// </summary>
        /// <param name="sql">标准SQL语句</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteNonQuery(StringCollection sql, string connectionString, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            string sqlText = "";
            //SqlCommand cmd;
            //    using (SqlConnection connection =
            //new SqlConnection(GetConnectionString()))
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;

                        for (int i = 0; i < sql.Count; i++)
                        {
                            cmd.CommandText = sql[i].ToString();
                            sqlText = cmd.CommandText;
                            //SqlCommand cmd = new SqlCommand(sql[i].ToString(), conn);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery\r\nSQL语句：\r\n" + sqlText + "\r\n连接字符串：\r\n" + connectionString);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                return result;
            }
            //try
            //{
            //    if (!SqlServerConnectionStatus) return false;
            //    conn.Open();
            //    cmd = new SqlCommand();
            //    cmd.Connection = conn;

            //    for (int i = 0; i < sql.Count; i++)
            //    {
            //        cmd.CommandText = sql[i].ToString();
            //        sqlText = cmd.CommandText;
            //        //SqlCommand cmd = new SqlCommand(sql[i].ToString(), conn);
            //        cmd.ExecuteNonQuery();
            //    }
            //    SqlServerConnectionStatus = true;
            //    result = true;
            //}
            //catch (Exception e)
            //{
            //    errorMessage = e.ToString();
            //    TestConnection();
            //    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery\r\nSQL语句：\r\n" + sqlText + "\r\n连接字符串：\r\n" + connectionString);
            //}
            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //        conn.Close();
            //}
            //return result;
        }
        /// <summary>
        /// 执行SQL命令，实现Insert Update Delete 命令
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string sql, string connectionString, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (!SqlServerConnectionStatus) return false;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 执行SQL后，返回第一行，第一列的结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <param name="count"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteOutNum(string sql, string connectionString, out int count, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                count = 0;
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (!SqlServerConnectionStatus) return false;
                        conn.Open();
                        count = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
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
        /// <param name="ds">查询结果记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteDataset(string sql, string connectionString, out DataSet ds, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.SelectCommand = new SqlCommand(sql, conn);
                        dsCommand.Fill(ds);
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
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
        public static bool ExecuteDataTable(string sql, string connectionString, out DataTable dt, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.SelectCommand = new SqlCommand(sql, conn);
                        dsCommand.Fill(dt);
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
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
        /// <param name="dr">查询结果记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteDataReader(string sql, string connectionString, out SqlDataReader dr, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            dr = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        if (!SqlServerConnectionStatus) return false;
                        conn.Open();
                        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteNonQuery\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
                }
            }

            return result;
        }

        /// <summary>
        /// 提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="sqlParameters">存储过程参数数组</param>
        /// <param name="ds">存储过程里面返回的记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure(string procedureName, string connectionString, ref SqlParameter[] sqlParameters, out DataSet ds, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.SelectCommand = new SqlCommand(procedureName, conn);
                        dsCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < sqlParameters.Length; i++)
                        {
                            dsCommand.SelectCommand.Parameters.Add(sqlParameters[i]);
                        }
                        dsCommand.Fill(ds);
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteStoredProcedure\r\n存储过程：\r\n" + procedureName + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="startRecord">从其开始的从零开始的记录号</param>
        /// <param name="maxRecords">要检索的最大记录数</param>
        /// <param name="sqlParameters">存储过程参数数组</param>
        /// <param name="ds">存储过程里面返回的记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure(string procedureName, string connectionString, int startRecord, int maxRecords, ref SqlParameter[] sqlParameters, out DataSet ds, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.SelectCommand = new SqlCommand(procedureName, conn);
                        dsCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < sqlParameters.Length; i++)
                        {
                            dsCommand.SelectCommand.Parameters.Add(sqlParameters[i]);
                        }
                        dsCommand.Fill(ds, startRecord, maxRecords, "srcTable");
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteStoredProcedure\r\n存储过程：\r\n" + procedureName + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 提供通用的调用存储过程的方法（不需要返回查询的记录集的调用）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="sqlParameters">存储过程参数数组</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure(string procedureName, string connectionString, ref SqlParameter[] sqlParameters, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < sqlParameters.Length; i++)
                        {
                            cmd.Parameters.Add(sqlParameters[i]);
                        }
                        cmd.ExecuteNonQuery();
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteStoredProcedure\r\n存储过程：\r\n" + procedureName + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="connectionString">连库字符串</param>
        /// 
        /// <param name="ds">存储过程里面返回的记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure(string procedureName, string connectionString, out DataSet ds, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {

                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.SelectCommand = new SqlCommand(procedureName, conn);
                        dsCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

                        dsCommand.Fill(ds);
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteStoredProcedure\r\n存储过程：\r\n" + procedureName + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 提供通用的调用存储过程的方法（需要返回查询的记录集的调用）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="connectionString">连库字符串</param>
        /// 
        /// <param name="dt">存储过程里面返回的记录集</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure(string procedureName, string connectionString, out DataTable dt, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {

                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.SelectCommand = new SqlCommand(procedureName, conn);
                        dsCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

                        dsCommand.Fill(dt);
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteStoredProcedure\r\n存储过程：\r\n" + procedureName + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 以表的形式增加数据到数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="connectionString"></param>
        /// <param name="dt"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool ExecuteSQL(string sql, ref SqlParameter[] sqlParameters, string connectionString, DataTable dt, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {


                try
                {
                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.InsertCommand = new SqlCommand(sql, conn);

                        for (int i = 0; i < sqlParameters.Length; i++)
                        {
                            sqlParameters[i].IsNullable = true;
                            dsCommand.InsertCommand.Parameters.Add(sqlParameters[i]);
                        }
                        if (dt == null) return false;
                        dt.AcceptChanges();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].SetAdded();
                        }
                        dsCommand.Update(dt);
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    //ApplicationConfig.ConnectionStringPIS_NEW_Value = "0";
                    TestConnection();
                    errorMessage = e.ToString();
                    ApplicationLog.WriteLog(e, "\r\nExecuteSQL\r\nSQL语句：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 提供通用的调用存储过程的方法（增加数据的数据表的到数据库的调用）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="sqlParameters">存储过程参数数组</param>
        /// <param name="dt">增加数据的数据表</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure(string procedureName, string connectionString, ref SqlParameter[] sqlParameters, DataTable dt, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (!SqlServerConnectionStatus) return false;
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.InsertCommand = new SqlCommand(procedureName, conn);
                        dsCommand.InsertCommand.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < sqlParameters.Length; i++)
                        {
                            dsCommand.InsertCommand.Parameters.Add(sqlParameters[i]);
                        }
                        if (dt == null) return false;
                        dt.AcceptChanges();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].SetAdded();
                        }
                        dsCommand.Update(dt);
                    }
                    SqlServerConnectionStatus = true;
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteStoredProcedure\r\n存储过程：\r\n" + procedureName + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 提供通用的调用存储过程的方法（更新数据）
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="connectionString">连库字符串</param>
        /// <param name="sqlParameters">存储过程参数数组</param>
        /// <param name="dt">增加数据的数据表</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>布尔值，true表示该执行成功，false表示执行失败</returns>
        public static bool ExecuteStoredProcedure_Update(string procedureName, string connectionString, ref SqlParameter[] sqlParameters, DataTable dt, out string errorMessage)
        {
            bool result = false;
            errorMessage = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlDataAdapter dsCommand = new SqlDataAdapter())
                    {
                        dsCommand.UpdateCommand = new SqlCommand(procedureName, conn);
                        dsCommand.UpdateCommand.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < sqlParameters.Length; i++)
                        {
                            dsCommand.UpdateCommand.Parameters.Add(sqlParameters[i]);
                        }
                        if (dt == null) return false;
                        dt.AcceptChanges();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i].SetAdded();
                        }
                        dsCommand.Update(dt);
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    errorMessage = e.ToString();
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\nExecuteStoredProcedure\r\n存储过程：\r\n" + procedureName + "\r\n连接字符串：\r\n" + connectionString);
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
        /// 增加记录后，返回自动编号
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="newID"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery_newID(string sql, out long newID)
        {
            sql += ";select @@identity";
            string connectionString;
            connectionString = ApplicationConfig.ConnectionString_MES;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();
                        newID = Convert.ToInt64(cmd.ExecuteScalar());
                    }
                    return true;
                }
                catch (Exception e)
                {
                    newID = 0;
                    TestConnection();
                    ApplicationLog.WriteLog(e, "\r\n ExecuteNonQuery_newID\r\n存储过程：\r\n" + sql + "\r\n连接字符串：\r\n" + connectionString);
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

//using System.Collections.Generic;

using System.Threading;


namespace BizDataAccess
{
    public partial class DataAccess
    {
        /// <summary>
        /// 测试与数据的连接线程
        /// </summary>
        static Thread threadTestConnection;


        /// <summary>
        /// 测试与数据的连接
        /// </summary>
        /// <returns></returns>
        //public static void TestConnection()
        //{
        //    try
        //    {
        //        if (threadTestConnection == null)
        //        {
        //            threadTestConnection = new Thread(new ThreadStart(SQLCommon.TestConnection));
        //            threadTestConnection.Start();
        //        }
        //        else
        //        {
        //            threadTestConnection.Abort();
        //            threadTestConnection = null;
        //            threadTestConnection = new Thread(new ThreadStart(SQLCommon.TestConnection));
        //            threadTestConnection.Start();
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        ApplicationLog.WriteLog(err, err.Source);
        //    }
        //}
        /// <summary>
        /// 测试与数据的连接
        /// </summary>
        /// <returns></returns>
        public static void TestConnectionFirst()
        {
            SQLCommon.TestConnection();
        }
    }
}

using HslCommunication.LogNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace SystemFramework
{
    /// <summary>
    /// Class1 的摘要说明。
    /// </summary>
    public class ApplicationLog
    {
        /// <summary>
        /// 日志跟踪路径
        /// </summary>
        public static string TraceFolderPath { set; get; } = string.Empty;

        /// <summary>
        /// 是否输出Console
        /// </summary>
        public static bool IsConsole { set; get; } = false;

        /// <summary>
        /// 初始化日志
        /// </summary>
        public ApplicationLog(bool isconsole = false)
        {
            IsConsole = isconsole;
            string MES_Code = "";
            try
            {
                MES_Code = ConfigurationManager.AppSettings.GetValues("MES_Code")[0].ToString();
            }
            catch
            {
                MES_Code = Guid.NewGuid().ToString();
            }
            try
            {
                TraceFolderPath = ConfigurationManager.AppSettings.GetValues("TraceFolderPath")[0].ToString();
            }
            catch
            {
                TraceFolderPath = Environment.CurrentDirectory;
            }
            Common.Utils.BusinessLog.Path = $"{TraceFolderPath}Macro_{MES_Code}/BusinessLog";
            Common.Utils.SystemLog.Path = $"{TraceFolderPath}Macro_{MES_Code}/SystemLog";
        }

        #region Console队列

        /// <summary>
        /// 通知队列
        /// </summary>
        public static ConcurrentQueue<MsgLog> concurrentQueueConsole = new ConcurrentQueue<MsgLog>();

        /// <summary>
        /// 通知队列 信号量
        /// </summary>
        public static AutoResetEvent autoResetQueueConsole = new AutoResetEvent(false);

        /// <summary>
        ///写入消息队列
        /// </summary>
        /// <param name="msg"></param>
        private static void EnqueueConsole(MsgLog msg)
        {
            concurrentQueueConsole.Enqueue(msg);
            autoResetQueueConsole.Set(); //通知task队列里有内容了  可以开始循环读取
        }

        #endregion Console队列

        /// <summary>
        /// 初始化日志线程
        /// </summary>
        /// <param name="filenames"></param>
        /// <param name="degree">记录大于DEBUG等级的日志</param>
        public static void IntLogThread(List<string> filenames, string degree = "DEBUG")
        {
            Common.Utils.BusinessLog.Init(filenames, (HslMessageDegree)Enum.Parse(typeof(HslMessageDegree), degree));
        }

        /// <summary>
        /// 记录逻辑日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="strContentInfo"></param>
        /// <param name="degree">INFO/DEBUG/ERROR/FATAL/EXCEPTION</param>
        /// <param name="e"></param>
        public static void BusinessLog(string fileName, string strContentInfo, string degree = "DEBUG", Exception e = null)
        {
            switch (degree)
            {
                case "INFO":
                    Common.Utils.BusinessLog.Info(strContentInfo, fileName);
                    break;

                case "DEBUG":
                    Common.Utils.BusinessLog.Debug(strContentInfo, fileName);
                    break;

                case "ERROR":
                    Common.Utils.BusinessLog.Error(strContentInfo, fileName);
                    break;

                case "FATAL":
                    Common.Utils.BusinessLog.Fatal(strContentInfo, fileName);
                    break;

                case "EXCEPTION":
                    Common.Utils.BusinessLog.Exception(strContentInfo, e, fileName);
                    break;

                default:
                    break;
            }

            if (IsConsole)
            {
                EnqueueConsole(new MsgLog() { Name = fileName, Content = strContentInfo });
            }
        }

        /// <summary>
        /// 记录逻辑日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="strContentInfo"></param>
        /// <param name="degree">INFO/DEBUG/ERROR/FATAL/EXCEPTION</param>
        /// <param name="e"></param>
        public static void SystemLog(string fileName, string strContentInfo, string degree = "DEBUG", Exception e = null)
        {
            switch (degree)
            {
                case "INFO":
                    Common.Utils.SystemLog.Info(strContentInfo, fileName);
                    break;

                case "DEBUG":
                    Common.Utils.SystemLog.Debug(strContentInfo, fileName);
                    break;

                case "ERROR":
                    Common.Utils.SystemLog.Error(strContentInfo, fileName);
                    break;

                case "FATAL":
                    Common.Utils.SystemLog.Fatal(strContentInfo, fileName);
                    break;

                case "EXCEPTION":
                    Common.Utils.SystemLog.Exception(strContentInfo, e, fileName);
                    break;

                default:
                    break;
            }

            if (IsConsole)
            {
                EnqueueConsole(new MsgLog() { Name = fileName, Content = strContentInfo });
            }
        }

        /// <summary>
        /// 写入系统日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="catchInfo"></param>
        public static void WriteLog(Exception ex, string catchInfo)
        {
            Common.Utils.SystemLog.Exception(catchInfo, ex);
            if (IsConsole)
            {
                EnqueueConsole(new MsgLog() { Name = "common", Content = $"{catchInfo}:{ex?.Message}" });
            }
        }

        /// <summary>
        /// 写入系统日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="catchInfo"></param>
        public static void WriteLog(string catchInfo)
        {
            Common.Utils.SystemLog.Exception(catchInfo);
            if (IsConsole)
            {
                EnqueueConsole(new MsgLog() { Name = "common", Content = $"{catchInfo}" });
            }
        }
    }

    /// <summary>
    /// 日志消息
    /// </summary>
    public class MsgLog
    {
        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Content { get; set; }
    }
}
using HslCommunication.LogNet;
using System;

namespace Common.Utils
{
    /// <summary>
    /// 日志对象初始化
    /// </summary>
    public class BusinessLog
    {
        /// <summary>
        ///初始化业务日志
        /// </summary>
        /// <returns></returns>
        private static readonly ILogNet _logger = new LogNetDateTime(ConfigHelper.GetAppseting("Logger:DirPath") + ConfigHelper.GetAppseting("Logger:BusinessName"), GenerateMode.ByEveryDay);

        private BusinessLog()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Warnmsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Warn(string Warnmsg, Exception e, string filename = "common")
        {
            _logger.WriteWarn(Warnmsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Warnmsg"></param>
        /// <param name="filename"></param>
        public static void Warn(string Warnmsg, string filename = "common")
        {
            _logger.WriteWarn(Warnmsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Infomsg"></param>
        /// <param name="filename"></param>
        public static void Info(string Infomsg, string filename = "common")
        {
            _logger.WriteInfo(Infomsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Infomsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Info(string Infomsg, Exception e, string filename = "common")
        {
            _logger.WriteInfo(Infomsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Debugmsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Debug(string Debugmsg, Exception e, string filename = "common")
        {
            _logger.WriteDebug(Debugmsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Debugmsg"></param>
        /// <param name="filename"></param>
        public static void Debug(string Debugmsg, string filename = "common")
        {
            _logger.WriteDebug(Debugmsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Errormsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Error(string Errormsg, Exception e, string filename = "common")
        {
            _logger.WriteError(Errormsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Errormsg"></param>
        /// <param name="filename"></param>
        public static void Error(string Errormsg, string filename = "common")
        {
            _logger.WriteError(Errormsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Fatalmsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Fatal(string Fatalmsg, Exception e, string filename = "common")
        {
            _logger.WriteFatal(Fatalmsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Fatalmsg"></param>
        /// <param name="filename"></param>
        public static void Fatal(string Fatalmsg, string filename = "common")
        {
            _logger.WriteFatal(Fatalmsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Exceptionmsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Exception(string Exceptionmsg, Exception e, string filename = "common")
        {
            _logger.WriteException(Exceptionmsg, e, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Exceptionmsg"></param>
        /// <param name="filename"></param>
        public static void Exception(string Exceptionmsg, string filename = "common")
        {
            _logger.WriteException(Exceptionmsg, new Exception(), filename);
        }
    }
}
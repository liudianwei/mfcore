using HslCommunication.LogNet;
using System;
using System.Collections.Generic;

namespace Common.Utils
{
    /// <summary>
    /// 日志对象初始化
    /// </summary>
    public class BusinessLog
    {
        /// <summary>
        /// 日志路径
        /// </summary>
        public static string Path { get; set; } = Environment.CurrentDirectory + "/BusinessLog";

        /// <summary>
        /// 按工位区分日志集合
        /// </summary>
        public static Dictionary<string, ILogNet> LogNets = new Dictionary<string, ILogNet>();

        /// <summary>
        /// 初始化日志对象
        /// </summary>
        /// <param name="filenames"></param>
        /// <param name="degree">默认DBBUG最低等级,打印比它高等级的所有日志,等级由高到低None,FATAL,ERROR,WARN,INFO,DEBUG</param>
        /// <param name="consoleOutput">默认不输出</param>
        public static void Init(List<string> filenames, HslMessageDegree degree = HslMessageDegree.DEBUG, bool consoleOutput = false)
        {
            foreach (var item in filenames)
            {
                var _logger = new LogNetDateTime(Path, GenerateMode.ByEveryDay);
                _logger.SetMessageDegree(degree);
                _logger.ConsoleOutput = consoleOutput;
                if (!LogNets.ContainsKey(item))
                {
                    LogNets.Add(item, _logger);
                }
            }
        }

        /// <summary>
        /// 获取当前工位所属日志对象
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ILogNet GetLogNet(string filename = "common", HslMessageDegree degree = HslMessageDegree.DEBUG, bool consoleOutput = false)
        {
            if (LogNets.TryGetValue(filename, out var _logger))
            {
                return _logger;
            }
            else
            {
                _logger = new LogNetDateTime(Path, GenerateMode.ByEveryDay);
                _logger.SetMessageDegree(degree);
                _logger.ConsoleOutput = consoleOutput;
                LogNets.Add(filename, _logger);
                return _logger;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Warnmsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Warn(string Warnmsg, Exception e, string filename = "common")
        {
            GetLogNet(filename)?.WriteWarn(Warnmsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Warnmsg"></param>
        /// <param name="filename"></param>
        public static void Warn(string Warnmsg, string filename = "common")
        {
            GetLogNet(filename)?.WriteWarn(Warnmsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Infomsg"></param>
        /// <param name="filename"></param>
        public static void Info(string Infomsg, string filename = "common")
        {
            GetLogNet(filename)?.WriteInfo(Infomsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Infomsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Info(string Infomsg, Exception e, string filename = "common")
        {
            GetLogNet(filename)?.WriteInfo(Infomsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Debugmsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Debug(string Debugmsg, Exception e, string filename = "common")
        {
            GetLogNet(filename)?.WriteDebug(Debugmsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Debugmsg"></param>
        /// <param name="filename"></param>
        public static void Debug(string Debugmsg, string filename = "common")
        {
            GetLogNet(filename)?.WriteDebug(Debugmsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Errormsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Error(string Errormsg, Exception e, string filename = "common")
        {
            GetLogNet(filename)?.WriteError(Errormsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Errormsg"></param>
        /// <param name="filename"></param>
        public static void Error(string Errormsg, string filename = "common")
        {
            GetLogNet(filename)?.WriteError(Errormsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Fatalmsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Fatal(string Fatalmsg, Exception e, string filename = "common")
        {
            GetLogNet(filename)?.WriteFatal(Fatalmsg, e.ToString(), filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Fatalmsg"></param>
        /// <param name="filename"></param>
        public static void Fatal(string Fatalmsg, string filename = "common")
        {
            GetLogNet(filename)?.WriteFatal(Fatalmsg, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Exceptionmsg"></param>
        /// <param name="e"></param>
        /// <param name="filename"></param>
        public static void Exception(string Exceptionmsg, Exception e, string filename = "common")
        {
            GetLogNet(filename)?.WriteException(Exceptionmsg, e, filename);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Exceptionmsg"></param>
        /// <param name="filename"></param>
        public static void Exception(string Exceptionmsg, string filename = "common")
        {
            GetLogNet(filename)?.WriteException(Exceptionmsg, new Exception(), filename);
        }
    }
}
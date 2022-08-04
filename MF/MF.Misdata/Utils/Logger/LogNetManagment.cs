using System;
using System.Text;

namespace HslCommunication.LogNet
{
    /*************************************************************************************
     *
     *    目标：
     *        1. 高性能的日志类
     *        2. 灵活的配置
     *        3. 日志分级
     *        4. 控制输出
     *        5. 方便筛选
     *        6. 方便的配置按小时，天，月，年记录
     *
     *************************************************************************************/

    /// <summary>
    /// 日志类的管理器
    /// </summary>
    public class LogNetManagment
    {
        /// <summary>
        /// 存储文件的时候指示单文件存储
        /// </summary>
        internal const int LogSaveModeBySingleFile = 1;

        /// <summary>
        /// 存储文件的时候指示根据文件大小存储
        /// </summary>
        internal const int LogSaveModeByFileSize = 2;

        /// <summary>
        /// 存储文件的时候指示根据日志时间来存储
        /// </summary>
        internal const int LogSaveModeByDateTime = 3;

        /// <summary>
        /// 日志文件的头标志
        /// </summary>
        internal const string LogFileHeadString = "";

        internal static string GetDegreeDescription(HslMessageDegree degree)
        {
            switch (degree)
            {
                case HslMessageDegree.None:
                    return "放弃";

                case HslMessageDegree.FATAL:
                    return "致命";

                case HslMessageDegree.EXCEPTION:
                    return "异常";

                case HslMessageDegree.ERROR:
                    return "错误";

                case HslMessageDegree.WARN:
                    return "警告";

                case HslMessageDegree.INFO:
                    return "信息";

                case HslMessageDegree.DEBUG:
                    return "调试";

                default:
                    return "全部";
            }
        }

        /// <summary>
        /// 公开的一个静态变量，允许随意的设置
        /// </summary>
        public static ILogNet LogNet { get; set; }

        /// <summary>
        /// 通过异常文本格式化成字符串用于保存或发送
        /// </summary>
        /// <param name="text">文本消息</param>
        /// <param name="ex">异常</param>
        /// <returns>异常最终信息</returns>
        public static string GetSaveStringFromException(string text, Exception ex, HslMessageDegree degree)
        {
            StringBuilder builder = new StringBuilder(text);

            if (ex != null)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    builder.Append(" : ");
                }
                builder.Append(Environment.NewLine);
                try
                {
                    builder.Append($"{GetDegreeDescription(degree)}信息:"); builder.Append(ex.Message);
                    builder.Append(Environment.NewLine);
                    builder.Append($"{GetDegreeDescription(degree)}堆栈:");
                    builder.Append(Environment.NewLine);
                    builder.Append(ex.StackTrace);
                    builder.Append(Environment.NewLine);
                    builder.Append($"{GetDegreeDescription(degree)}类型:"); builder.Append(ex.GetType().ToString());
                }
                catch
                {
                }
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }
    }
}
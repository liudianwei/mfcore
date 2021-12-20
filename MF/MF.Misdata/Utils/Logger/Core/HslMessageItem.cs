using System;
using System.Threading;

namespace Common.Utils.MacroInfoLogger
{
    #region LogMessage

    /// <summary>
    /// 单个日志的记录信息
    /// </summary>
    public class HslMessageItem
    {
        private static long IdNumber = 0;

        /// <summary>
        /// 默认的无参构造器
        /// </summary>
        public HslMessageItem()
        {
            Id = Interlocked.Increment(ref IdNumber);
        }

        /// <summary>
        /// 单个记录信息的标识ID，程序重新运行时清空
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// 消息的等级
        /// </summary>
        public HslMessageDegree Degree { get; set; } = HslMessageDegree.DEBUG;

        /// <summary>
        /// 线程ID
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// 消息文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 消息发生的事件
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 消息的关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 是否取消写入到文件中去，在事件BeforeSaveToFile触发的时候捕获即可设置。
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// 日志写入文件名
        /// </summary>
        public string filename { get; set; } = "common";

        /// <summary>
        /// 返回表示当前对象的字符串
        /// </summary>
        /// <returns>字符串信息</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(KeyWord))
            {
                return $"[{Degree}] {Time:yyyy-MM-dd HH:mm:ss.fff} Thread [{ThreadId:D3}] {Text}";
            }
            else
            {
                return $"[{Degree}] {Time:yyyy-MM-dd HH:mm:ss.fff} Thread [{ThreadId:D3}] {KeyWord} : {Text}";
            }
        }

        /// <summary>
        /// 返回表示当前对象的字符串，剔除了关键字
        /// </summary>
        /// <returns>字符串信息</returns>
        public string ToStringWithoutKeyword()
        {
            return $"[{Degree}] {Time:yyyy-MM-dd HH:mm:ss.fff} Thread [{ThreadId:D3}] {Text}";
        }
    }

    #endregion LogMessage
}
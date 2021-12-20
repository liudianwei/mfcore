namespace Common.Utils.MacroInfoLogger
{
    #region Message Degree

    /// <summary>
    /// 记录消息的等级
    /// </summary>
    public enum HslMessageDegree
    {
        /// <summary>
        /// 一条消息都不记录
        /// </summary>
        None = 1,

        /// <summary>
        /// 记录致命等级及以上日志的消息
        /// </summary>
        FATAL = 2,

        /// <summary>
        /// 记录异常等级及以上日志的消息
        /// </summary>
        ERROR = 3,

        /// <summary>
        /// 记录警告等级及以上日志的消息
        /// </summary>
        WARN = 4,

        /// <summary>
        /// 记录信息等级及以上日志的消息
        /// </summary>
        INFO = 5,

        /// <summary>
        /// 记录调试等级及以上日志的信息
        /// </summary>
        DEBUG = 6
    }

    #endregion LogMessage
}
namespace Common.Utils.MacroInfoLogger
{
    #region Log Output Format

    /// <summary>
    /// 日志文件输出模式
    /// </summary>
    public enum GenerateMode
    {
        /// <summary>
        /// 按每个小时生成日志文件
        /// </summary>
        ByEveryHour = 1,

        /// <summary>
        /// 按每天生成日志文件
        /// </summary>
        ByEveryDay = 2,

        /// <summary>
        /// 按每个周生成日志文件
        /// </summary>
        ByEveryWeek = 3,

        /// <summary>
        /// 按每个月生成日志文件
        /// </summary>
        ByEveryMonth = 4,

        /// <summary>
        /// 按每季度生成日志文件
        /// </summary>
        ByEverySeason = 5,

        /// <summary>
        /// 按每年生成日志文件
        /// </summary>
        ByEveryYear = 6,
    }

    #endregion LogMessage
}
namespace Common.Communication
{
    /// <summary>
    /// 返回值
    /// </summary>
    public class OperateResultValue
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        ///值
        /// </summary>
        public string Value { get; set; }
    }
}
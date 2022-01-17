namespace BasicData
{
    /// <summary>
    /// 变量类型实体
    /// </summary>
    public class QualityDataType
    {
        /// <summary>
        /// 变量代码
        /// </summary>
        public int TagID { get; set; }

        /// <summary>
        /// 变量类型代码 Basic 通用标签类型,Quality 质量标签类型, Alarm 个性标签类型(报警)
        /// </summary>
        public string TagClassType { get; set; }

        /// <summary>
        /// 变量类型字符类型 (西门子 11：BOOL 2：INT 3：DINT 17：BYTE 8：STRING 18:WORD 4:REAL)
        /// </summary>
        public int TagTypeID { get; set; }

        /// <summary>
        /// 变量类型
        /// </summary>
        public string TagType { get; set; }

        /// <summary>
        /// 变量类型长度
        /// </summary>
        public int TagTypeLength { get; set; }

        /// <summary>
        /// OpCode
        /// </summary>
        public string OpCode { get; set; }

        /// <summary>
        /// 工位号
        /// </summary>
        public string OpName { get; set; }

        /// <summary>
        /// 变量值
        /// </summary>
        public object TagValue { get; set; }

        /// <summary>
        /// 192 数据正常
        /// </summary>
        public int TagQuality { get; set; }

        ///// <summary>
        ///// 变量组排序(1:拧矩，2：转角)
        ///// </summary>
        //public int TagGroupOrder { get; set; }

        ///// <summary>
        ///// 变量特点
        ///// </summary>
        //public int TagFeature { get; set; }

        /// <summary>
        /// 合格标志（0,1,2）
        /// </summary>
        public int IsGoodBad { get; set; }

        /// <summary>
        /// 变量描述
        /// </summary>
        public string TagDescription { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 是否监控
        /// </summary>
        public bool IsMonitor { get; set; }

        /// <summary>
        /// Tag名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 扫描频率(ms)
        /// </summary>
        public int SamplingInterval { get; set; }
    }
}
namespace Common.Model
{
    public class QualityDataType
    {
        /// <summary>
        /// 标签ID
        /// </summary>
        public string TagID { get; set; }

        /// <summary>
        /// 变量名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 变量类型分组(Basic,Quality,Alarm)
        /// </summary>
        public string TagClassType { get; set; }

        /// <summary>
        /// 变量类型
        /// </summary>
        public TagType TagType { get; set; }

        /// <summary>
        /// 变量类型长度
        /// </summary>
        public int TagTypeLength { get; set; } = 0;

        /// <summary>
        /// 工位代码
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
        /// 变量质量 默认good
        /// </summary>
        public string TagQuality { get; set; } = "good";

        /// <summary>
        /// 是否合格 默认0
        /// </summary>
        public int IsGoodBad { get; set; } = 0;

        /// <summary>
        /// 变量描述
        /// </summary>
        public string TagDescription { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = false;

        /// <summary>
        /// 是否监控
        /// </summary>
        public bool IsMonitor { get; set; }
    }
}
using System;

namespace Common.Model
{
    public class CustomeEventArgs
    {
        private string tagID = "";
        private string tagName = "";
        private string tagClassType = "";
        private string opName = "";
        private object tagValue = 0;
        private TagType tagType = TagType.@bool;
        private string tagQuality = "good";
        private DateTime timeStamp = DateTime.Now;
        private int bitType = 8;
        private string tagDescription = "";

        /// <summary>
        ///
        /// </summary>
        public CustomeEventArgs()
        { }

        /// <summary>
        /// 标签ID
        /// </summary>
        public string TagID
        {
            get { return this.tagID; }
            set { this.tagID = value; }
        }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName
        {
            get { return this.tagName; }
            set { this.tagName = value; }
        }

        /// <summary>
        /// 变量类型代码 Basic 通用标签类型,Quality 质量标签类型, Alarm 个性标签类型(报警)
        /// </summary>
        public string TagClassType
        {
            get { return this.tagClassType; }
            set { this.tagClassType = value; }
        }

        /// <summary>
        /// 变量类型
        /// </summary>
        public TagType TagType
        {
            get { return this.tagType; }
            set { this.tagType = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        public object TagValue
        {
            get { return this.tagValue; }
            set { this.tagValue = value; }
        }

        /// <summary>
        /// 工位号
        /// </summary>
        public string OpName
        {
            get { return this.opName; }
            set { this.opName = value; }
        }

        /// <summary>
        /// 标签质量(good)
        /// </summary>
        public string TagQuality
        {
            get { return this.tagQuality; }
            set { this.tagQuality = value; }
        }

        /// <summary>
        /// 变量时间标签
        /// </summary>
        public DateTime TimeStamp
        {
            get { return this.timeStamp; }
            set { this.timeStamp = value; }
        }

        /// <summary>
        /// 位模(8 bit 或 16 bit)
        /// </summary>
        public int BitType
        {
            get { return this.bitType; }
            set { this.bitType = value; }
        }

        /// <summary>
        /// 标签质量描述
        /// </summary>
        public string TagDescription
        {
            get { return this.tagDescription; }
            set { this.tagDescription = value; }
        }
    }
}
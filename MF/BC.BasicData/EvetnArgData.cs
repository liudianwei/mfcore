using System;

namespace EvetnArgData
{
    partial class IEvetnArgData
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DelegateClassHandle(object sender, CustomeEvetnArgs e);

    /// <summary>
    ///
    /// </summary>
    public class TagData
    {
        private object tagKey;

        /// <summary>
        /// 变量代码
        /// </summary>
        public object TagKey
        {
            get { return tagKey; }
            set { tagKey = value; }
        }

        private object tagValue;

        /// <summary>
        ///
        /// </summary>
        public object TagValue
        {
            get { return tagValue; }
            set { tagValue = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public event DelegateClassHandle TagDataOnChange;

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public void InvokeTagData(CustomeEvetnArgs e)
        {
            if (TagDataOnChange != null)
            {
                TagDataOnChange(this, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tagKey"></param>
        /// <param name="tagValue"></param>
        public void InvokeTagData(object tagKey, object tagValue)
        {
            if (TagDataOnChange != null)
            {
                CustomeEvetnArgs e = new CustomeEvetnArgs();
                e.TagID = tagKey;
                e.TagValue = tagValue;
                TagDataOnChange(this, e);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tagKey"></param>
        /// <param name="opName"></param>
        /// <param name="tagValue"></param>
        /// <param name="tagQuality"></param>
        /// <param name="timeStamp"></param>
        public void InvokeTagData(object tagKey, object opName, object tagValue, object tagQuality, DateTime timeStamp)
        {
            if (TagDataOnChange != null)
            {
                CustomeEvetnArgs e = new CustomeEvetnArgs();
                e.TagID = tagKey;
                e.OpName = opName;
                e.TagValue = tagValue;
                e.TagQuality = tagQuality;
                e.TimeStamp = timeStamp;
                TagDataOnChange(this, e);
            }
        }
    }

    /// <summary>
    /// 传输PLC变量
    /// </summary>
    [Serializable]
    public class CustomeEvetnArgs : EventArgs
    {
        private object tagID = 0;
        private object tagName = "";
        private string tagClassType = "";
        private object opName = "";
        private object tagValue = 0;
        private object tagTypeID = 0;
        private string tagType = "";
        private object tagQuality = 0;
        private DateTime timeStamp = DateTime.Now;

        /// <summary>
        ///
        /// </summary>
        public CustomeEvetnArgs()
        { }

        /// <summary>
        /// 变量代码
        /// </summary>
        public object TagID
        {
            get { return this.tagID; }
            set { this.tagID = value; }
        }

        /// <summary>
        /// 变量名称
        /// </summary>
        public object TagName
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
        public object TagTypeID
        {
            get { return this.tagTypeID; }
            set { this.tagTypeID = value; }
        }

        /// <summary>
        /// 变量类型
        /// </summary>
        public string TagType
        {
            get { return this.tagType; }
            set { this.tagType = value; }
        }

        /// <summary>
        /// 变量数值
        /// </summary>
        public object TagValue
        {
            get { return this.tagValue; }
            set { this.tagValue = value; }
        }

        /// <summary>
        /// 工位号
        /// </summary>
        public object OpName
        {
            get { return this.opName; }
            set { this.opName = value; }
        }

        /// <summary>
        /// 质量代码（192）
        /// </summary>
        public object TagQuality
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
        /// Guid唯一ID
        /// </summary>
        public string Id = "";

        /// <summary>
        /// 开始执行脚本的时间
        /// </summary>
        public DateTime HandlerTimeStamp { get; set; }

        /// <summary>
        /// 结束执行脚本的时间
        /// </summary>
        public DateTime StopTimeStamp { get; set; }

        /// <summary>
        ///
        /// </summary>
        public dynamic SerialNo { get; set; }

        /// <summary>
        ///
        /// </summary>
        public dynamic EngineType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public dynamic TypeCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        public TimeSpan SpentTimeStamp { get; set; }
    }
}
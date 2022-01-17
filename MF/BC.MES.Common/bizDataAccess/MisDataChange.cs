using System;
//using System.Collections.Generic;


namespace BizDataAccess
{




    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DelegateMisDataChange(object sender, MisEvetnArgs e);
    /// <summary>
    /// 
    /// </summary>
    public class MisDataOnChange
    {

        /// <summary>
        /// SqlServer 与数据通讯状态
        /// </summary>
        static public MisDataOnChange tagDataSqlServerConnectionStatus = new MisDataOnChange();

        private object tagKey;
        /// <summary>
        /// 
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
        public event DelegateMisDataChange TagDataOnChange;


 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void InvokeTagData(MisEvetnArgs e)
        {
            if (TagDataOnChange != null)
            {
                TagDataOnChange(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        public void InvokeTagData(object tagValue)
        {
            if (TagDataOnChange != null)
            {
                MisEvetnArgs e = new MisEvetnArgs();
                e.TagValue = tagValue;
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
                MisEvetnArgs e = new MisEvetnArgs();
                e.TagKey = tagKey;
                e.TagValue = tagValue;
                TagDataOnChange(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagKey"></param>
        /// <param name="tagValue"></param>
        /// <param name="tagQuality"></param>
        public void InvokeTagData(object tagKey, object tagValue, object tagQuality)
        {
            if (TagDataOnChange != null)
            {
                MisEvetnArgs e = new MisEvetnArgs();
                e.TagKey = tagKey;
                e.TagValue = tagValue;
                e.TagQuality = tagQuality;
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
        public void InvokeTagData(object tagKey, object opName, object tagValue, object tagQuality)
        {
            if (TagDataOnChange != null)
            {
                MisEvetnArgs e = new MisEvetnArgs();
                e.TagKey = tagKey;
                e.OpName = opName;
                e.TagValue = tagValue;
                e.TagQuality = tagQuality;
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
                MisEvetnArgs e = new MisEvetnArgs();
                e.TagKey = tagKey;
                e.OpName = opName;
                e.TagValue = tagValue;
                e.TagQuality = tagQuality;
                e.TimeStamp = timeStamp;
                TagDataOnChange(this, e);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MisEvetnArgs : EventArgs
    {
        object tagKey = 0;
        object opName = "";
        object tagValue = 0;
        object tagQuality = 0;
        DateTime timeStamp = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public MisEvetnArgs()
        { }
        /// <summary>
        /// 
        /// </summary>
        public object TagKey
        {
            get { return this.tagKey; }
            set { this.tagKey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object TagValue
        {
            get { return this.tagValue; }
            set { this.tagValue = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object OpName
        {
            get { return this.opName; }
            set { this.opName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object TagQuality
        {
            get { return this.tagQuality; }
            set { this.tagQuality = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TimeStamp
        {
            get { return this.timeStamp; }
            set { this.timeStamp = value; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct MisQualityDataType
    {
        /// <summary>
        /// 
        /// </summary>
        public int tagValueType;
        /// <summary>
        /// 
        /// </summary>
        public int tagTypeID;
        /// <summary>
        /// 
        /// </summary>
        public string opName;
        /// <summary>
        /// 
        /// </summary>
        public int tagID;
        /// <summary>
        /// 
        /// </summary>
        public object tagValue;
        /// <summary>
        /// 
        /// </summary>
        public int tagQuality;

    }
}

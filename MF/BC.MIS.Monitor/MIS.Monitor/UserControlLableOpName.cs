using System;
using System.ComponentModel;
using System.Windows.Forms;
using EvetnArgData;

namespace Monitor
{
    /// <summary>
    /// UserControlLable11 的摘要说明。
    /// </summary>
    public class UserControlLableOpName : System.Windows.Forms.Label
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        private string opName;

        /// <summary>
        ///  TagNameID名称
        /// </summary>
        [
        Description("工位号"),
        ]
        public string OpName
        {
            get
            {
                return opName;
            }
            set
            {
                opName = value;
                //string tagTimeStamp;
                //GetResulDataByDataChanged(tagNameID,out lableText,out  tagTimeStamp);
            }
        }

        private int tagKey = 0;

        /// <summary>
        ///  TagNameID名称
        /// </summary>
        [
        Description("列位置"),
        ]
        public int TagKey
        {
            get
            {
                return tagKey;
            }
            set
            {
                tagKey = value;
                //string tagTimeStamp;
                //GetResulDataByDataChanged(tagNameID,out lableText,out  tagTimeStamp);
            }
        }

        private int tagValueID;

        /// <summary>
        ///  TagNameID名称
        /// </summary>
        [
        Description("变量排序[e.TagValueType]"),
        ]
        public int TagValueID
        {
            get
            {
                return tagValueID;
            }
            set
            {
                tagValueID = value;
            }
        }

        private int tagQuality;

        /// <summary>
        ///  TagNameID名称
        /// </summary>
        [
        Description("合格标志"),
        ]
        public int TagQuality
        {
            get
            {
                return tagQuality;
            }
            set
            {
                tagQuality = value;
            }
        }

        /// <summary>
        ///  LableText 变量值
        /// </summary>
        private string lableText = "变量值";

        /// <summary>
        ///  LableText 变量值
        /// </summary>
        [
        Description("LableText变量值"),
        ]
        public string LableText
        {
            get
            {
                return lableText;
            }
            set
            {
                lableText = value;
                this.Invalidate();
            }
        }

        //
        // <doc>
        // <desc>
        //      Overrides the text property of Control.  This label ignores
        //      the text property, so we add additional attributes here so the
        //      property does not show up in the properties window and is not
        //      persisted.
        // </desc>
        // </doc>
        //
        /// <summary>
        ///
        /// </summary>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public UserControlLableOpName()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();
            AutoSize = false;
            // TODO: 在 InitializeComponent 调用后添加任何初始化

            GlobleData.tagData_Lable_OpName.TagDataOnChange += new DelegateClassHandle(tagData_Lable_TagDataOnChange);
        }

        private void tagData_Lable_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            if (OpName == e.OpName.ToString() && TagKey == Convert.ToInt32(e.TagID) && tagValueID == Convert.ToInt32(e.TagTypeID))
            {
                try
                {
                    LableText = e.TagValue.ToString();
                }
                catch { }
                Invalidate();
            }
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器
        /// 修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            //
            // UserControlLable
            //
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        }

        #endregion 组件设计器生成的代码

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.Text = this.lableText;
        }
    }
}
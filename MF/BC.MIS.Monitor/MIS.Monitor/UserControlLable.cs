using EvetnArgData;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Monitor
{
    /// <summary>
    /// UserControlLable11 的摘要说明。
    /// </summary>
    public class UserControlLable : System.Windows.Forms.Label
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        private int tagNameID;

        /// <summary>
        ///  TagNameID名称
        /// </summary>
        [
        Description("TagNameID变量标识"),
        ]
        public int TagNameID
        {
            get
            {
                return tagNameID;
            }
            set
            {
                tagNameID = value;
                //string tagTimeStamp;
                //GetResulDataByDataChanged(tagNameID,out lableText,out  tagTimeStamp);
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
        public UserControlLable()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();

            // TODO: 在 InitializeComponent 调用后添加任何初始化

            GlobleData.tagData_Lable.TagDataOnChange += new DelegateClassHandle(tagData_Lable_TagDataOnChange);

            //if(GlobleData.ResultDataOnDataChange!=null)
            //{
            //    GlobleData.ResultDataOnDataChange.RowChanged+=new DataRowChangeEventHandler(ResultData_RowChanged);
            //}
        }

        private void tagData_Lable_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            if (tagNameID == Convert.ToInt32(e.TagID))
            {
                LableText = e.TagValue.ToString();
                Invalidate();
            }
            //LableText=
        }

        //        /// <summary>
        //        /// 根据变量标识，确定对应数据
        //        /// </summary>
        //        /// <param name="tagValue"></param>
        //        /// <param name="tagTimeStamp"></param>
        //        private void GetResulDataByDataChanged(int tagID,out string tagValue,out string tagTimeStamp)
        //        {
        ////			DataRow []dataRow;
        ////			string selectString;
        ////
        //            int index ;

        //            try
        //            {
        //                if(GlobleData.ResultData!=null)
        //                {
        //                    if(!GlobleData.table_TagID_ResultDataIndex.ContainsKey(tagID))
        //                    {
        //                        tagValue = this.lableText;
        //                        tagTimeStamp = DateTime.Now.ToString();

        //                        return ;

        //                    }

        //                    index = (int)GlobleData.table_TagID_ResultDataIndex[tagID];

        //                    tagValue = GlobleData.ResultData.Rows[index][BaseData_tagTableData.TagValue_FIELD].ToString() ;
        //                    tagTimeStamp =   GlobleData.ResultData.Rows[index][BaseData_tagTableData.lastupdatetime_FIELD].ToString();

        //                }
        //                else
        //                {
        //                    tagValue = this.lableText;
        //                    tagTimeStamp = DateTime.Now.ToString();
        //                }
        //            }
        //            catch(Exception error)
        //            {
        //                //MessageBox.Show(String.Format("Error while 根据变量标识，确定对应数据:-{0}" ,error.Message),"UserControlLable-GetResulDataByDataChanged - ",MessageBoxButtons.OK,MessageBoxIcon.Error);
        //                tagValue = this.lableText;
        //                tagTimeStamp = DateTime.Now.ToString();
        //                ApplicationLog.WriteLog(error,"\r\n根据变量标识，确定对应数据\r\nGetResulDataByDataChanged\r\ntagID="+tagID.ToString());

        //            }

        //        }

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

        ///// <summary>
        ///// 监控值变化时，图形重绘
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ResultData_RowChanged(object sender, DataRowChangeEventArgs e)
        //{
        //    try
        //    {
        //        if(GlobleData.ResultDataOnDataChangeRow!=null)
        //        {
        //            if(tagNameID==int.Parse(GlobleData.ResultDataOnDataChangeRow[TagNameDataResulteData.ItemNameID_FIELD].ToString()))
        //            {
        //                lableText = GlobleData.ResultDataOnDataChangeRow[TagNameDataResulteData.ItemValue_FIELD].ToString();
        //                this.Invalidate();

        //            }
        //        }
        //    }
        //    catch(Exception err)
        //    {
        //        ApplicationLog.WriteLog(err,"\r\n监控值变化时，图形重绘\r\nResultData_RowChanged");

        //    }

        //}
    }
}
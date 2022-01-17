using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SystemFramework;
using EvetnArgData;

namespace Monitor
{
    /// <summary>
    /// AlarmListView 的摘要说明。
    /// </summary>
    public class AlarmListView : System.Windows.Forms.ListView
    {
        /// <summary>
        /// 从OnDataChange发送int tagID, int tagValue到处理程序
        /// private void OnDataChange(int tagID, int tagValue)
        /// </summary>
        /// <param name="tagID"></param>
        /// <param name="tagValue"></param>
        public delegate void OnDataChangeInvoke(object tagID, object tagValue);

        /// <summary>
        /// 从OnDataChange发送int tagID, int tagValue到处理程序
        /// private void OnDataChange(int tagID, int tagValue)
        /// </summary>
        private OnDataChangeInvoke onDataChangeInvoke;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.ColumnHeader statuts;

        /// <summary>
        /// 当时发生的报警总行数
        /// </summary>
        private int Items_Count = 0;

        /// <summary>
        ///
        /// </summary>
        public AlarmListView()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();
            this.View = View.Details;
            // TODO: 在 InitComponent 调用后添加任何初始化

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            onDataChangeInvoke = new OnDataChangeInvoke(OnDataChange);

            GlobleData.tagDataAlarmListView.TagDataOnChange += new DelegateClassHandle(tagDataAlarm_TagDataOnChange);
        }

        private void tagDataAlarm_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            try
            {
                BeginInvoke(onDataChangeInvoke, new object[] { e.TagID, e.TagValue });
            }
            catch
            {
            }
        }

        private void OnDataChange(object tagID, object tagValue)
        {
            DataTable pLC_AlgViewCHTData = (DataTable)tagValue;
            InsertIntoListView(pLC_AlgViewCHTData);
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
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
            this.AlarmNumber = new System.Windows.Forms.ColumnHeader();
            this.ComeInDate = new System.Windows.Forms.ColumnHeader();
            this.ComeInTime = new System.Windows.Forms.ColumnHeader();
            this.MessageText = new System.Windows.Forms.ColumnHeader();
            this.ComeFromPossition = new System.Windows.Forms.ColumnHeader();
            this.TypeName = new System.Windows.Forms.ColumnHeader();
            this.SerileNumber = new System.Windows.Forms.ColumnHeader();
            this.statuts = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            //
            // AlarmNumber
            //
            this.AlarmNumber.Text = "编号";
            this.AlarmNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AlarmNumber.Width = 100;
            //
            // ComeInDate
            //
            this.ComeInDate.Text = "日期";
            this.ComeInDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ComeInDate.Width = 120;
            //
            // ComeInTime
            //
            this.ComeInTime.Text = "时间";
            this.ComeInTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ComeInTime.Width = 100;
            //
            // MessageText
            //
            this.MessageText.Text = "内容";
            this.MessageText.Width = 400;
            //
            // ComeFromPossition
            //
            this.ComeFromPossition.Text = "工位";
            this.ComeFromPossition.Width = 80;
            //
            // TypeName
            //
            this.TypeName.Text = "类型";
            this.TypeName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // SerileNumber
            //
            this.SerileNumber.Text = "序号";
            //
            // statuts
            //
            this.statuts.Text = "状态";
            //
            // AlarmListView
            //
            this.AutoArrange = false;
            this.BackColor = System.Drawing.Color.White;
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SerileNumber,
            this.AlarmNumber,
            this.ComeInDate,
            this.ComeInTime,
            this.TypeName,
            this.MessageText,
            this.ComeFromPossition,
            this.statuts});
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FullRowSelect = true;
            this.GridLines = true;
            this.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.MultiSelect = false;
            this.Size = new System.Drawing.Size(800, 100);
            this.View = System.Windows.Forms.View.Details;
            this.ResumeLayout(false);
        }

        #endregion 组件设计器生成的代码

        /// <summary>
        /// 报警到达
        /// </summary>
        private Color colorTextCameIn = Color.Black;

        /// <summary>
        /// 报警到达
        /// </summary>
        private Color colorBackgroudCameIn = Color.Red;

        /// <summary>
        /// 报警解除
        /// </summary>
        private Color colorTextWentOut = Color.Black;

        /// <summary>
        /// 报警解除
        /// </summary>
        private Color colorBackgroudWentOut = Color.Yellow;

        /// <summary>
        /// 报警确认
        /// </summary>
        private Color colorTextAcknowledgment = Color.Black;

        /// <summary>
        /// 报警确认
        /// </summary>
        private Color colorBackgroudAcknowledgment = Color.Blue;

        /// <summary>
        /// 报警过滤条件
        /// </summary>
        private string alarmItemSelect;

        /// <summary>
        ///
        /// </summary>
        public System.Windows.Forms.ColumnHeader AlarmNumber;

        /// <summary>
        ///
        /// </summary>
        public System.Windows.Forms.ColumnHeader ComeInDate;

        /// <summary>
        ///
        /// </summary>
        public System.Windows.Forms.ColumnHeader ComeInTime;

        /// <summary>
        ///
        /// </summary>
        public System.Windows.Forms.ColumnHeader MessageText;

        /// <summary>
        ///
        /// </summary>
        public System.Windows.Forms.ColumnHeader ComeFromPossition;

        /// <summary>
        ///
        /// </summary>
        public System.Windows.Forms.ColumnHeader TypeName;

        /// <summary>
        ///
        /// </summary>
        public System.Windows.Forms.ColumnHeader SerileNumber;

        private DataRow[] dataRow = null;
        private string[] listViewItemString = new string[8];
        private ListViewItem listViewItem = null;

        /// <summary>
        /// 报警过滤条件
        /// </summary>
        [
        Description("报警过滤条件"),
        ]
        public string AlarmItemSelect
        {
            get
            {
                return alarmItemSelect;
            }
            set
            {
                alarmItemSelect = value;
            }
        }

        /// <summary>
        /// 从系统获得
        /// =null 时，初始化时显示全部报警
        /// =AlarmItemSelect,初始化时显示AlarmItemSelect条件报警
        /// </summary>
        private string alarmMonitorConnnection;

        /// <summary>
        ///
        /// </summary>
        [
        Description("暂时空"),
        ]
        public string AlarmMonitorConnnection
        {
            get
            {
                return alarmMonitorConnnection;
            }
            set
            {
                alarmMonitorConnnection = value;
            }
        }

        /// <summary>
        /// 报警到达文字颜色
        /// </summary>

        [
        Description("报警到达文字颜色"),
        ]
        public Color ColorTextCameIn
        {
            get
            {
                return colorTextCameIn;
            }
            set
            {
                colorTextCameIn = value;
            }
        }

        /// <summary>
        /// 报警到达文本底色
        /// </summary>
        [
        Description("报警到达文本底色"),
        ]
        public Color ColorBackgroudCameIn
        {
            get
            {
                return colorBackgroudCameIn;
            }
            set
            {
                colorBackgroudCameIn = value;
            }
        }

        /// <summary>
        /// 报警解除文字颜色
        /// </summary>

        [
        Description("报警解除文字颜色"),
        ]
        public Color ColorTextWentOut
        {
            get
            {
                return colorTextWentOut;
            }
            set
            {
                colorTextWentOut = value;
            }
        }

        /// <summary>
        /// 报警解除文本底色
        /// </summary>
        [
        Description("报警解除文本底色"),
        ]
        public Color ColorBackgroudWentOut
        {
            get
            {
                return colorBackgroudWentOut;
            }
            set
            {
                colorBackgroudWentOut = value;
            }
        }

        /// <summary>
        /// 报警确认文字颜色
        /// </summary>

        [
        Description("报警确认文字颜色"),
        ]
        public Color ColorTextAcknowledgment
        {
            get
            {
                return colorTextAcknowledgment;
            }
            set
            {
                colorTextAcknowledgment = value;
            }
        }

        /// <summary>
        /// 报警确认文本底色
        /// </summary>
        [
        Description("报警确认文本底色"),
        ]
        public Color ColorBackgroudAcknowledgment
        {
            get
            {
                return colorBackgroudAcknowledgment;
            }
            set
            {
                colorBackgroudAcknowledgment = value;
            }
        }

        /// <summary>
        /// 初始化报警显示
        /// </summary>
        public void InitAlarmListView()
        {
            try
            {
                if (GlobleData.ResultDataAlarmAll != null)
                {
                    //isAddItem = true 图形重绘
                    //isAddItem = false 不图形重绘
                    bool isAddItem = false;
                    DateTime dateTime;
                    int status;

                    if (this.alarmItemSelect == null)
                        isAddItem = true;
                    else
                    {
                        if (alarmItemSelect.Length == 0)
                            isAddItem = true;
                        else
                        {
                            isAddItem = false;
                        }
                    }

                    if (isAddItem)
                    {
                        dataRow = GlobleData.ResultDataAlarmAll.Select();
                    }
                    else
                    {
                        string strSelectResultDataAlarmAll;

                        strSelectResultDataAlarmAll = "Text2" + "='" + alarmItemSelect + "'";

                        dataRow = GlobleData.ResultDataAlarmAll.Select(strSelectResultDataAlarmAll);
                    }
                    for (int i = 0; i < dataRow.Length; i++)
                    {
                        status = int.Parse(dataRow[i]["State"].ToString());

                        listViewItemString[0] = ((int)(Items_Count + 1)).ToString();
                        listViewItemString[1] = dataRow[i]["MsgNr"].ToString();
                        dateTime = DateTime.Parse(dataRow[i]["DateTime"].ToString());
                        listViewItemString[2] = dateTime.ToLongDateString();
                        listViewItemString[3] = dateTime.ToLongTimeString();
                        listViewItemString[4] = dataRow[i]["TypeName"].ToString();
                        listViewItemString[5] = dataRow[i]["Text1"].ToString();
                        listViewItemString[6] = dataRow[i]["Text2"].ToString();
                        listViewItemString[7] = status.ToString();

                        listViewItem = new ListViewItem(listViewItemString);

                        listViewItem.SubItems[0].BackColor = Color.DarkGray;

                        switch (status)
                        {
                            case 1:
                                listViewItem.BackColor = this.ColorBackgroudCameIn;
                                listViewItem.ForeColor = this.ColorTextCameIn;
                                break;

                            case 2:
                                listViewItem.BackColor = this.ColorBackgroudWentOut;
                                listViewItem.ForeColor = this.ColorTextWentOut;
                                break;

                            default:
                                listViewItem.BackColor = this.ColorBackgroudCameIn;
                                listViewItem.ForeColor = this.ColorTextCameIn;
                                break;
                        }
                        if (this.Items.Count >= GlobleData.ResultDataAlarmRowMaxCount)
                        {
                            if (this.Items.Count > 1)
                                this.Items.RemoveAt(0);
                        }

                        this.Items.Add(listViewItem);
                        Items_Count++;

                        this.EnsureVisible(this.Items.Count - 1);
                    }
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        /// <summary>
        /// 报警控件，监控值变化时，图形重绘
        /// </summary>
        /// <param name="pLC_AlgViewCHTData"></param>
        private void InsertIntoListView(DataTable pLC_AlgViewCHTData)
        {
            //isAddItem = true 图形重绘
            //isAddItem = false 不图形重绘
            bool isAddItem = false;
            DateTime dateTime;
            int status;

            DataRow dr;
            DataTable dt;

            try
            {
                if (pLC_AlgViewCHTData == null) return;
                dt = pLC_AlgViewCHTData;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (this.alarmItemSelect == null)
                        isAddItem = true;
                    else
                    {
                        if (alarmItemSelect.Length == 0)
                            isAddItem = true;
                        else
                        {
                            //符合过滤条件的显示到AlarmListView中
                            //工位号
                            if (alarmItemSelect.CompareTo(dr["Text2"].ToString()) == 0)
                                isAddItem = true;
                            else
                                isAddItem = false;
                        }
                    }

                    if (isAddItem)
                    {
                        status = int.Parse(dr["State"].ToString());

                        listViewItemString[0] = ((int)(Items_Count + 1)).ToString();
                        listViewItemString[1] = dr["MsgNr"].ToString();
                        dateTime = DateTime.Parse(dr["DateTime"].ToString());
                        listViewItemString[2] = dateTime.ToLongDateString();
                        listViewItemString[3] = dateTime.ToLongTimeString();
                        listViewItemString[4] = dr["TypeName"].ToString();
                        listViewItemString[5] = dr["Text1"].ToString();
                        listViewItemString[6] = dr["Text2"].ToString();
                        listViewItemString[7] = status.ToString();

                        listViewItem = new ListViewItem(listViewItemString);

                        switch (status)
                        {
                            case 1:
                                listViewItem.BackColor = this.ColorBackgroudCameIn;
                                listViewItem.ForeColor = this.ColorTextCameIn;
                                break;

                            case 2:
                                listViewItem.BackColor = this.ColorBackgroudWentOut;
                                listViewItem.ForeColor = this.ColorTextWentOut;
                                break;

                            default:
                                listViewItem.BackColor = this.ColorBackgroudCameIn;
                                listViewItem.ForeColor = this.ColorTextCameIn;
                                break;
                        }

                        if (this.Items.Count >= GlobleData.ResultDataAlarmRowMaxCount)
                        {
                            if (this.Items.Count > 1)
                            {
                                this.Items.RemoveAt(0);
                            }
                        }
                        this.Items.Add(listViewItem);

                        Items_Count++;

                        this.EnsureVisible(this.Items.Count - 1);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    this.Invalidate();
                }
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, err.Message);
            }
        }
    }
}
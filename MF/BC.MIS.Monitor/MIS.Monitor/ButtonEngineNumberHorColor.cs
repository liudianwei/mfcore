using EvetnArgData;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Monitor
{
    /// <summary>
    /// UserControlButton 的摘要说明。
    /// </summary>
    public class ButtonEngineNumberHorColor : System.Windows.Forms.Button
    {
        private System.ComponentModel.IContainer components = null;
        private ToolTip ttShow = new ToolTip();

        /// <summary>
        ///  Tag位置0时颜色 1
        /// </summary>
        private Color colorsBackGroudColorSet0 = Color.Blue;

        /// <summary>
        ///  Tag位置1时颜色 2
        /// </summary>
        private Color colorsBackGroudColorSet1 = Color.Cyan;

        /// <summary>
        ///  Tag位置2时颜色 4
        /// </summary>
        private Color colorsBackGroudColorSet2 = Color.Lime;

        /// <summary>
        ///  Tag位置3时颜色 8
        /// </summary>
        private Color colorsBackGroudColorSet3 = Color.Yellow;

        /// <summary>
        ///  Tag位置4时颜色 16
        /// </summary>
        private Color colorsBackGroudColorSet4 = Color.Red;

        /// <summary>
        ///  Tag位置5时颜色 32
        /// </summary>
        private Color colorsBackGroudColorSet5 = Color.Brown;

        /// <summary>
        ///  Tag位置6时颜色 64
        /// </summary>
        private Color colorsBackGroudColorSet6 = Color.Pink;

        /// <summary>
        ///  Tag位置7时颜色 128
        /// </summary>
        private Color colorsBackGroudColorSet7 = Color.Green;

        /// <summary>
        ///  设置Tag位置0时颜色
        /// </summary>
        [
        Description("设置Tag位置0时颜色"),
        ]
        public Color ColorSet0
        {
            get
            {
                return colorsBackGroudColorSet0;
            }
            set
            {
                colorsBackGroudColorSet0 = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  设置Tag位置1时颜色
        /// </summary>
        [
        Description("设置Tag位置1时颜色"),
        ]
        public Color ColorSet1
        {
            get
            {
                return colorsBackGroudColorSet1;
            }
            set
            {
                colorsBackGroudColorSet1 = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  设置Tag位置2时颜色
        /// </summary>
        [
        Description("设置Tag位置2时颜色"),
        ]
        public Color ColorSet2
        {
            get
            {
                return colorsBackGroudColorSet2;
            }
            set
            {
                colorsBackGroudColorSet2 = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  设置Tag位置3时颜色
        /// </summary>
        [
        Description("设置Tag位置3时颜色"),
        ]
        public Color ColorSet3
        {
            get
            {
                return colorsBackGroudColorSet3;
            }
            set
            {
                colorsBackGroudColorSet3 = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  设置Tag位置4时颜色
        /// </summary>
        [
        Description("设置Tag位置4时颜色"),
        ]
        public Color ColorSet4
        {
            get
            {
                return colorsBackGroudColorSet4;
            }
            set
            {
                colorsBackGroudColorSet4 = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  设置Tag位置5时颜色
        /// </summary>
        [
        Description("设置Tag位置5时颜色"),
        ]
        public Color ColorSet5
        {
            get
            {
                return colorsBackGroudColorSet5;
            }
            set
            {
                colorsBackGroudColorSet5 = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  设置Tag位置6时颜色
        /// </summary>
        [
        Description("设置Tag位置6时颜色"),
        ]
        public Color ColorSet6
        {
            get
            {
                return colorsBackGroudColorSet6;
            }
            set
            {
                colorsBackGroudColorSet6 = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  设置Tag位置7时颜色
        /// </summary>
        [
        Description("设置Tag位置7时颜色"),
        ]
        public Color ColorSet7
        {
            get
            {
                return colorsBackGroudColorSet7;
            }
            set
            {
                colorsBackGroudColorSet7 = value;
                Invalidate();
            }
        }

        /// <summary>
        /// TagKey 显示选项
        /// </summary>
        private string tagKey = "1";

        /// <summary>
        /// TagKey 显示选项
        /// </summary>
        [
        Description("显示选项"),
        ]
        public string TagKey
        {
            get
            {
                return tagKey;
            }
            set
            {
                tagKey = value;
            }
        }

        /// <summary>
        /// 变量值
        /// </summary>
        private int colorsBackGroudColorTagValue;

        /// <summary>
        /// 机床状态
        /// </summary>
        private int machineStatusValue;

        /// <summary>
        /// 机床状态
        /// </summary>
        public int MachineStatusValue
        {
            get
            {
                return machineStatusValue;
            }
            set
            {
                machineStatusValue = value;
            }
        }

        /// <summary>
        /// 工件编号
        /// </summary>
        private string engineNumber = "";

        /// <summary>
        /// 工件编号
        /// </summary>
        public string EngineNumber
        {
            get
            {
                return engineNumber;
            }
            set
            {
                engineNumber = value;
                //重绘控件
                Invalidate();
            }
        }

        private string msToolTipHead = "工件编号：";

        //ToolTip显示工件编号
        private string msToolTip = "";

        /// <summary>
        /// 当鼠标在上方时出现工件编号
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("Basic_Property"), Description("当鼠标在上方时出现工件编号")]
        public string ToolTipText
        {
            get
            {
                return msToolTip;
            }
            set
            {
                msToolTip = value;
                //重绘控件
                Invalidate();
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
        ///  Button 文本
        /// </summary>
        private string lableText = "Button";

        /// <summary>
        ///  Button 文本
        /// </summary>
        [
        Description("Button 文本"),
        ]
        public string LableText
        {
            get
            {
                //重绘控件
                Invalidate();
                return lableText;
            }
            set
            {
                lableText = value;
                //重绘控件
                Invalidate();
            }
        }

        /// <summary>
        ///  OpName
        /// 工位名称
        /// </summary>
        private string opName;

        /// <summary>
        ///  OpName
        /// 工位名称
        /// </summary>
        [
        Description("工位名称"),
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
            }
        }

        /// <summary>
        ///  Button 文本
        /// </summary>
        private Color textColor = Color.White;

        /// <summary>
        ///  Button 文本
        /// </summary>
        [
        Description("TextColor 文本颜色"),
        ]
        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        ///  Button 文本颜色代码
        /// </summary>
        [
        Description("文本颜色代码"),
        ]
        public int ColorsBackGroudColorTagValue
        {
            get
            {
                return colorsBackGroudColorTagValue;
            }
            set
            {
                colorsBackGroudColorTagValue = value;
                switch (colorsBackGroudColorTagValue)
                {
                    case 1:
                        textColor = colorsBackGroudColorSet0;
                        break;

                    case 2:
                        textColor = colorsBackGroudColorSet1;
                        break;

                    case 4:
                        textColor = colorsBackGroudColorSet2;
                        break;

                    case 8:
                        textColor = colorsBackGroudColorSet3;
                        break;

                    case 16:
                        textColor = colorsBackGroudColorSet4;
                        break;

                    case 32:
                        textColor = colorsBackGroudColorSet5;
                        break;

                    case 64:
                        textColor = colorsBackGroudColorSet6;
                        break;

                    case 128:
                        textColor = colorsBackGroudColorSet7;
                        break;

                    default:
                        textColor = Color.White;
                        break;
                }

                Invalidate();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ButtonEngineNumberHorColor()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();

            // TODO: 在 InitializeComponent 调用后添加任何初始化

            //ttShow.
            msToolTip = msToolTipHead + engineNumber;

            // textColor = Color.Black;
            this.Cursor = Cursors.Hand;
            this.Click += new EventHandler(UserControlButton_Click);
            this.MouseHover += new EventHandler(UserControlButton_MouseHover);
            ////工件编号的变化
            //GlobleData.tagDataEngineNumber.TagDataOnChange += new DelegateClassHandle(tagDataEngineNumber_TagDataOnChange);
            ////机床状态
            //GlobleData.tagDataMachine.TagDataOnChange += new DelegateClassHandle(tagDataMachine_TagDataOnChange);
            //GlobleData.tagDataEngineNumberColor.TagDataOnChange += new DelegateClassHandle(tagDataEngineNumberColor_TagDataOnChange);
        }

        private void tagDataEngineNumberColor_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            if (OpName == e.OpName.ToString() & TagKey == e.TagID.ToString())
            {
                colorsBackGroudColorTagValue = Convert.ToInt32(e.TagValue);

                switch (colorsBackGroudColorTagValue)
                {
                    case 1:
                        textColor = colorsBackGroudColorSet0;
                        break;

                    case 2:
                        textColor = colorsBackGroudColorSet1;
                        break;

                    case 4:
                        textColor = colorsBackGroudColorSet2;
                        break;

                    case 8:
                        textColor = colorsBackGroudColorSet3;
                        break;

                    case 16:
                        textColor = colorsBackGroudColorSet4;
                        break;

                    case 32:
                        textColor = colorsBackGroudColorSet5;
                        break;

                    case 64:
                        textColor = colorsBackGroudColorSet6;
                        break;

                    case 128:
                        textColor = colorsBackGroudColorSet7;
                        break;

                    default:
                        textColor = Color.White;
                        break;
                }

                Invalidate();
            }
        }

        ///// <summary>
        ///// 机床状态
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void tagDataMachine_TagDataOnChange(object sender, CustomeEvetnArgs e)
        //{
        //    if (OpName == e.OpName.ToString())
        //    {
        //        MachineStatusValue = Convert.ToInt32(e.TagValue);
        //    }
        //}

        //void tagDataEngineNumber_TagDataOnChange(object sender, CustomeEvetnArgs e)
        //{
        //    if (OpName == e.OpName.ToString())
        //    {
        //        engineNumber =Convert.ToString( e.TagValue);
        //        msToolTip = msToolTipHead + engineNumber;
        //    }
        //}
        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public void ButtonEngineNumberHorColor_Show(CustomeEvetnArgs e)
        {
            engineNumber = Convert.ToString(e.TagValue);
            msToolTip = msToolTipHead + engineNumber;
        }

        private void UserControlButton_Click(object sender, EventArgs e)
        {
            GlobleData.tagDataEngineStatusShowDialog.InvokeTagData(MachineStatusValue, opName, engineNumber, 1, DateTime.Now);
        }

        private void UserControlButton_MouseHover(object sender, EventArgs e)
        {
            if (this.ToolTipText != "")
            {
                this.ttShow.AutoPopDelay = 6000;
                this.ttShow.InitialDelay = 2000;
                this.ttShow.ReshowDelay = 1000;
                this.ttShow.ShowAlways = true;
                this.ttShow.SetToolTip(this, this.ToolTipText);
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
            this.SuspendLayout();
            //
            // ButtonEngineNumber
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Size = new System.Drawing.Size(12, 42);
            this.UseVisualStyleBackColor = false;
            this.ResumeLayout(false);
        }

        #endregion 组件设计器生成的代码

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RectangleF drawRect = new RectangleF(0, 0, this.Width, this.Height);

            // Create string to draw.
            string drawString = lableText;
            // Create font and brush.
            Font drawFont = this.Font;// new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(textColor);//new SolidBrush(Color.Black);
                                                             // Create point for upper-left corner of drawing.
                                                             // Set format of string.
            StringFormat drawFormat = new StringFormat();
            //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            // Draw string to screen.
            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);
        }
    }
}
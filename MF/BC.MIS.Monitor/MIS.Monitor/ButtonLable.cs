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
    public class ButtonLable : System.Windows.Forms.Button
    {
        private System.ComponentModel.IContainer components = null;
        private ToolTip ttShow = new ToolTip();

        /// <summary>
        /// 发动机号
        /// </summary>
        private string engineNumber = "";

        /// <summary>
        /// 发动机号
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

        //ToolTip显示发动机号
        private string msToolTip = "";

        /// <summary>
        /// 当鼠标在上方时出现发动机号
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        [Category("Basic_Property"), Description("当鼠标在上方时出现发动机号")]
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
        private Color textColor = Color.Black;

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
        ///
        /// </summary>
        public ButtonLable()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();

            // TODO: 在 InitializeComponent 调用后添加任何初始化
            textColor = Color.Black;
        }

        private void tagDataEngineNumber_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            if (OpName == e.TagID.ToString())
            {
                engineNumber = e.TagValue.ToString();
                msToolTip = msToolTipHead + engineNumber;
            }
        }

        private void UserControlButton_Click(object sender, EventArgs e)
        {
            GlobleData.tagDataEngineStatusShowDialog.InvokeTagData(opName, engineNumber, 0, 0, DateTime.Now);
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
            // ButtonLable
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Size = new System.Drawing.Size(14, 50);
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
            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            // Draw string to screen.
            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);
        }
    }
}
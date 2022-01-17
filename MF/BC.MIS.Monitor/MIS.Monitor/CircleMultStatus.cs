using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
using EvetnArgData;

namespace Monitor
{
    /// <summary>
    /// 增加TagKey
    /// </summary>
    public class CircleMultStatus : System.Windows.Forms.Control
    {
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
        ///  OpName
        /// 工位名称
        /// </summary>
        private string opName = "OP1000";

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
        /// 序号
        /// </summary>
        private string snText = "1";

        /// <summary>
        ///  序号
        /// </summary>
        [
        Description("序号"),
        ]
        public string SNTEXT
        {
            get
            {
                return snText;
            }
            set
            {
                snText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否保存到数据库
        /// </summary>
        private bool isSaveToDataBase = false;

        //bool isFirstDraw=true;

        /// <summary>
        ///  LineColor设置外园线颜色
        /// </summary>
        private Color lineColor = Color.Black;

        /// <summary>
        ///  Tag位置1时颜色
        /// </summary>
        private Color colorsBackGroudColorSet = Color.Red;

        /// <summary>
        ///  Tag位置0时颜色
        /// </summary>
        private Color colorsBackGroudColorNoSet = Color.FromArgb(128, 128, 255);

        /// <summary>
        /// 变量值
        /// </summary>
        private int colorsBackGroudColorTagValue;

        /// <summary>
        /// 变量值
        /// </summary>
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
                        this.BackColor = colorsBackGroudColorSet0;
                        break;

                    case 2:
                        this.BackColor = colorsBackGroudColorSet1;
                        break;

                    case 4:
                        this.BackColor = colorsBackGroudColorSet2;
                        break;

                    case 8:
                        this.BackColor = colorsBackGroudColorSet3;
                        break;

                    case 16:
                        this.BackColor = colorsBackGroudColorSet4;
                        break;

                    case 32:
                        this.BackColor = colorsBackGroudColorSet5;
                        break;

                    case 64:
                        this.BackColor = colorsBackGroudColorSet6;
                        break;

                    case 128:
                        this.BackColor = colorsBackGroudColorSet7;
                        break;

                    default:
                        this.BackColor = colorsBackGroudColorNoSet;
                        break;
                }
                //if ((colorsBackGroudColorTagValue & (int)Math.Pow(2, colorsBackGroudColorTagBit)) > 0)
                //    this.BackColor = colorsBackGroudColorSet;
                //else
                //    this.BackColor = colorsBackGroudColorNoSet;
                Invalidate();
            }
        }

        /// <summary>
        ///  TagBit变量测试位
        /// </summary>
        private int colorsBackGroudColorTagBit;

        /// <summary>
        ///  Tag位置1时颜色
        /// </summary>
        [
        Description("Tag位置1时颜色"),
        ]
        public Color ColorSet
        {
            get
            {
                return colorsBackGroudColorSet;
            }
            set
            {
                colorsBackGroudColorSet = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  Tag位置0时颜色
        /// </summary>
        [
        Description("Tag位置0时颜色"),
        ]
        public Color ColorNoSet
        {
            get
            {
                return colorsBackGroudColorNoSet;
            }
            set
            {
                colorsBackGroudColorNoSet = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  TagBit变量测试位
        /// </summary>
        [
        Description("TagBit变量测试位"),
        ]
        public int ColorTagBit
        {
            get
            {
                return colorsBackGroudColorTagBit;
            }
            set
            {
                colorsBackGroudColorTagBit = value;
                Invalidate();
            }
        }

        /// <summary>
        ///  LineColor设置外园线颜色
        /// </summary>
        [
        Description("LineColor设置外园线颜色"),
        ]
        public Color LineColor
        {
            get
            {
                return lineColor;
            }
            set
            {
                lineColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否保存到数据库
        /// </summary>
        [
        Description("是否保存到数据库"),
        ]
        public bool IsSaveToDataBase
        {
            get
            {
                return isSaveToDataBase;
            }
            set
            {
                isSaveToDataBase = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public CircleMultStatus()
            : base()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            GlobleData.tagDataCartColorMultStatus.TagDataOnChange += new DelegateClassHandle(tagDataCartColorMultStatus_TagDataOnChange);
        }

        private void tagDataCartColorMultStatus_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            if (OpName == e.OpName.ToString() & TagKey == e.TagID.ToString())
            {
                colorsBackGroudColorTagValue = Convert.ToInt32(e.TagValue);

                switch (colorsBackGroudColorTagValue)
                {
                    case 1:
                        this.BackColor = colorsBackGroudColorSet0;
                        break;

                    case 2:
                        this.BackColor = colorsBackGroudColorSet1;
                        break;

                    case 4:
                        this.BackColor = colorsBackGroudColorSet2;
                        break;

                    case 8:
                        this.BackColor = colorsBackGroudColorSet3;
                        break;

                    case 16:
                        this.BackColor = colorsBackGroudColorSet4;
                        break;

                    case 32:
                        this.BackColor = colorsBackGroudColorSet5;
                        break;

                    case 64:
                        this.BackColor = colorsBackGroudColorSet6;
                        break;

                    case 128:
                        this.BackColor = colorsBackGroudColorSet7;
                        break;

                    default:
                        this.BackColor = colorsBackGroudColorNoSet;
                        break;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// 绘制图形
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Pen pen = new Pen(lineColor, 3);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Size.Width, this.Size.Height);
            Region region = new Region(path);
            Rectangle rectangle;
            rectangle = new Rectangle(0, 0, this.Size.Width, this.Size.Height);
            this.Region = region;
            Font font;
            font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            SolidBrush br = new SolidBrush(Color.Black);
            // 取得文字大小
            SizeF sizText = e.Graphics.MeasureString(snText, font);

            e.Graphics.DrawString(snText, font, br, (rectangle.Width - sizText.Width) / 2, (rectangle.Height - sizText.Height) / 2);

            e.Graphics.DrawEllipse(pen, 0, 0, this.Size.Width, this.Size.Height);

            region.Dispose();
            pen.Dispose();
            path.Dispose();

            // 调用基类 OnPaint
            base.OnPaint(e);
        }

        private void InitializeComponent()
        {
        }
    }
}
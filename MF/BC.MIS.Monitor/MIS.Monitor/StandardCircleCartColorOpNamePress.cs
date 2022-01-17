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
    public class StandardCircleCartColorOpNamePress : System.Windows.Forms.Control
    {
        /// <summary>
        /// TagKey 显示选项
        /// </summary>
        private int tagKey = 0;

        /// <summary>
        /// TagKey 显示选项
        /// </summary>
        [
        Description("列位置(测量序号)"),
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
        private Color colorsBackGroudColorSet = Color.LimeGreen;

        /// <summary>
        ///  Tag位置0时颜色
        /// </summary>
        private Color colorsBackGroudColorNoSet = Color.Red;//Color.FromArgb(128, 128, 255);

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
                if ((colorsBackGroudColorTagValue & (int)Math.Pow(2, colorsBackGroudColorTagBit)) > 0)
                    this.BackColor = colorsBackGroudColorSet;
                else
                    this.BackColor = colorsBackGroudColorNoSet;
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
        public StandardCircleCartColorOpNamePress()
            : base()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            GlobleData.tagDataStandardCircleCartColorOpNamePress.TagDataOnChange += new DelegateClassHandle(TagDataOnChange);
        }

        private void TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            if (OpName == e.OpName.ToString() & TagKey == Convert.ToInt32(e.TagID))
            {
                colorsBackGroudColorTagValue = Convert.ToInt32(e.TagValue);
                if ((colorsBackGroudColorTagValue & (int)Math.Pow(2, colorsBackGroudColorTagBit)) > 0)
                    this.BackColor = colorsBackGroudColorSet;
                else
                    this.BackColor = colorsBackGroudColorNoSet;
                Invalidate();
            }
        }

        //Handle the paint event
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
            this.Region = region;

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
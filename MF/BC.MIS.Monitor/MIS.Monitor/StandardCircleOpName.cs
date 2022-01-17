using System;
//using System.Collections;
//using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Reflection;
//using System.Runtime.CompilerServices;
using System.Data;
using OpcData;

namespace Monitor
{


    /// <summary>
    /// 增加TagKey
    /// </summary>
    public class StandardCircleOpName : System.Windows.Forms.Control
    {
        /// <summary>
        /// TagKey 显示选项
        /// </summary>
        string tagKey;
        /// <summary>
        /// TagKey 显示选项
        /// </summary>
        [
        Description(列位置),
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
        /// TagKey 变量排序
        /// </summary>
        string tagValueID;
        /// <summary>
        /// TagKey 变量排序
        /// </summary>
        [
        Description(变量排序),
        ]
        public string TagValueID
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
        /// <summary>
        ///  OpName
        /// 工位名称
        /// </summary>
        string opName;
        /// <summary>
        ///  OpName
        /// 工位名称
        /// </summary>
        [
        Description(工位名称),
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
        bool isSaveToDataBase = false;


        //bool isFirstDraw=true;

        /// <summary>
        ///  LineColor设置外园线颜色
        /// </summary>
        Color lineColor = Color.Black;

        /// <summary>
        ///  Tag位置1时颜色
        /// </summary>
        Color colorsBackGroudColorSet = Color.Red;
        /// <summary>
        ///  Tag位置0时颜色
        /// </summary>
        Color colorsBackGroudColorNoSet = Color.FromArgb(128, 128, 255);
        /// <summary>
        /// 变量值
        /// </summary>
        int colorsBackGroudColorTagValue;
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
        int colorsBackGroudColorTagBit;

        /// <summary>
        ///  Tag位置1时颜色
        /// </summary>
        [
        Description(Tag位置1时颜色),
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
        Description(Tag位置0时颜色),
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
        Description(TagBit变量测试位),
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
        Description(LineColor设置外园线颜色),
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
        Description(是否保存到数据库),
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
        public StandardCircleOpName()
            : base()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            GlobleData.tagData_StandardCircleOpName.TagDataOnChange += new DelegateClassHandle(TagDataOnChange);
        }

        void TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            if (OpName == e.OpName.ToString() &TagKey==e.TagKey.ToString())
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
        /// <param name=e></param>
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

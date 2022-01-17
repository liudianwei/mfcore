using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using EvetnArgData;

namespace Monitor
{
    /// <summary>
    /// 发动机工位在线显示
    /// </summary>
    public class StandardRectangle : System.Windows.Forms.Control
    {
        /// <summary>
        ///  OpName
        /// 工位名称
        /// </summary>
        private string opName;

        /// <summary>
        /// 是否保存到数据库
        /// </summary>
        private bool isSaveToDataBase = false;

        /// <summary>
        ///  LineColor设置外园线颜色
        /// </summary>
        private Color lineColor = Color.Black;

        /// <summary>
        ///  Tag位置1时颜色
        /// </summary>
        private Color colorsBackGroudColorSet = Color.DarkRed;

        /// <summary>
        ///  Tag位置0时颜色
        /// </summary>
        private Color colorsBackGroudColorNoSet = Color.DimGray;

        /// <summary>
        ///
        /// </summary>
        public int OnLineEngineStatus;

        /// <summary>
        /// 显示托盘状态
        /// 1 有托盘（黄色）
        /// 0 无托盘（灰色）
        /// </summary>
        private int partPalletStatus;

        /// <summary>
        /// 显示托盘状态
        /// 1 有托盘（黄色）
        /// 0 无托盘（灰色）
        /// </summary>
        public int PartPalletStatus
        {
            get
            {
                return partPalletStatus;
            }
            set
            {
                partPalletStatus = value;
            }
        }

        /// <summary>
        /// 发动机号
        /// </summary>
        private string engineNumber;

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
            }
        }

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
                //Invalidate();
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
        ///  是否进行具体发动机号追踪
        /// </summary>
        private bool isEngineTrace = false;

        /// <summary>
        ///  是否进行具体发动机号追踪
        /// </summary>
        [
        Description("是否进行具体发动机号追踪"),
        ]
        public bool IsEngineTrace
        {
            get
            {
                return isEngineTrace;
            }
            set
            {
                isEngineTrace = value;
                Invalidate();
            }
        }

        ///// <summary>
        /////  具体发动机号追踪
        ///// 工位号
        ///// </summary>
        //string engineTraceOpName;
        ///// <summary>
        /////  具体发动机号追踪
        ///// 工位号
        ///// </summary>
        //[
        //Description("具体发动机号追踪工位号"),
        //]
        //public string EngineTraceOpName
        //{
        //    get
        //    {
        //        return engineTraceOpName;
        //    }
        //    set
        //    {
        //        engineTraceOpName = value;
        //    }
        //}
        /// <summary>
        ///  具体发动机号追踪发动机号
        /// 发动机号
        /// </summary>
        private string engineTraceEngineNumber;

        /// <summary>
        ///  具体发动机号追踪
        /// 发动机号
        /// </summary>
        [
        Description("具体发动机号追踪发动机号"),
        ]
        public string EngineTraceEngineNumber
        {
            get
            {
                return engineTraceEngineNumber;
            }
            set
            {
                engineTraceEngineNumber = value;
                Invalidate();
            }
        }

        private Color colorsBackGroudEngineTraceColorNoSet = Color.Blue;

        /// <summary>
        /// 具体发动机号追踪
        ///  Tag位置0时颜色
        /// </summary>
        [
        Description("具体发动机号追踪 Tag位置0时颜色"),
        ]
        public Color EngineTraceColorNoSet
        {
            get
            {
                return colorsBackGroudEngineTraceColorNoSet;
            }
            set
            {
                colorsBackGroudEngineTraceColorNoSet = value;
                //Invalidate();
            }
        }

        private Color colorsBackGroudEngineTraceColorSet = Color.Red;

        /// <summary>
        /// 具体发动机号追踪
        ///  Tag位置1时颜色
        /// </summary>
        [
        Description("具体发动机号追踪 Tag位置1时颜色"),
        ]
        public Color EngineTraceColorSet
        {
            get
            {
                return colorsBackGroudEngineTraceColorSet;
            }
            set
            {
                colorsBackGroudEngineTraceColorSet = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public StandardRectangle()
            : base()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            BackColor = Color.DimGray;
            //设定发动机号动态追踪
            //是否追踪
            //被追踪发动机号
            GlobleData.tagDataEngineNumberTrace.TagDataOnChange += new DelegateClassHandle(tagDataEngineNumberTrace_TagDataOnChange);
            //发动机到位或离开
            GlobleData.tagDataEngineStatus.TagDataOnChange += new DelegateClassHandle(tagDataEngineStatus_TagDataOnChange);
            //发动机号的变化
            GlobleData.tagDataEngineNumber.TagDataOnChange += new DelegateClassHandle(tagDataEngineNumber_TagDataOnChange);

            this.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// 发动机号的变化，给本控件的发动机号赋值
        /// 确定被追踪的发动机号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tagDataEngineNumber_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            //工位名称
            if (OpName == Convert.ToString(e.OpName))
            {
                EngineNumber = Convert.ToString(e.TagValue);
                ShowEngineNumberTrace();
                //if (IsEngineTrace)
                //{
                //    if (EngineTraceEngineNumber.Length < 1)
                //    {
                //        return;
                //    }
                //    if (EngineNumber.Length < 1)
                //    {
                //        this.BackColor = EngineTraceColorNoSet;
                //        Invalidate();
                //        return;
                //    }
                //    if (EngineTraceEngineNumber == EngineNumber)
                //    {
                //        this.BackColor = EngineTraceColorSet;
                //        Invalidate();
                //    }

                //}
            }
        }

        /// <summary>
        /// 发动机号动态追踪设定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tagDataEngineNumberTrace_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            //设定是否追踪发动机号
            IsEngineTrace = Convert.ToBoolean(e.TagID);
            //被追踪发动机号
            EngineTraceEngineNumber = Convert.ToString(e.TagValue);
            //显示被追踪的发动机
            ShowEngineNumberTrace();
        }

        /// <summary>
        /// 显示被追踪的发动机
        /// </summary>
        private void ShowEngineNumberTrace()
        {
            if (IsEngineTrace)
            {
                if (EngineNumber == null)
                {
                    // 显示托盘状态
                    // 1 有托盘（黄色）
                    // 0 无托盘（灰色）
                    ShowPartPalletStatus();
                    return;
                }
                if (EngineTraceEngineNumber.Length < 1)
                {
                    // 显示托盘状态
                    // 1 有托盘（黄色）
                    // 0 无托盘（灰色）
                    ShowPartPalletStatus();
                    return;
                }
                if (EngineNumber.Length < 1)
                {
                    // 显示托盘状态
                    // 1 有托盘（黄色）
                    // 0 无托盘（灰色）
                    ShowPartPalletStatus();
                    return;
                }
                if (EngineTraceEngineNumber == EngineNumber)
                {
                    this.BackColor = EngineTraceColorSet;
                    //显示追踪发动机号对应的工位
                    // 发动机号动态追踪
                    // 设定发动机号动态追踪时显示工位号
                    // 是否追踪
                    // 被追踪发动机号
                    //GlobleData.tagDataEngineNumberTraceOpName.InvokeTagData(1, OpName, EngineNumber, 1,DateTime.Now);
                    Invalidate();
                }
                else
                {
                    // 显示托盘状态
                    // 1 有托盘（黄色）
                    // 0 无托盘（灰色）
                    ShowPartPalletStatus();
                }
            }
            else
            {
                // 显示托盘状态
                // 1 有托盘（黄色）
                // 0 无托盘（灰色）
                ShowPartPalletStatus();
            }
        }

        /// <summary>
        /// 显示托盘状态
        /// 1 有托盘（黄色）
        /// 0 无托盘（灰色）
        /// </summary>
        /// <param name="e"></param>
        public void StandardRectangle_Show(CustomeEvetnArgs e)
        {
            try
            {
                partPalletStatus = Convert.ToInt32(e.TagValue);
                if (partPalletStatus == 1)
                {
                    this.BackColor = ColorSet;
                }
                else
                {
                    this.BackColor = ColorNoSet;
                }
            }
            catch { }
            Invalidate();
        }

        /// <summary>
        /// 显示托盘状态
        /// 1 有托盘（黄色）
        /// 0 无托盘（灰色）
        /// </summary>
        private void ShowPartPalletStatus()
        {
            if (partPalletStatus == 1)
            {
                this.BackColor = ColorSet;
            }
            else
            {
                this.BackColor = ColorNoSet;
            }
            Invalidate();
        }

        /// <summary>
        /// 托盘到位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tagDataEngineStatus_TagDataOnChange(object sender, CustomeEvetnArgs e)
        {
            //工位名称
            if (OpName == Convert.ToString(e.OpName))
            {
                try
                {
                    partPalletStatus = Convert.ToInt32(e.TagValue);
                    // 显示托盘状态
                    // 1 有托盘（黄色）
                    // 0 无托盘（灰色）
                    ShowPartPalletStatus();
                }
                catch
                {
                }
            }
        }

        //Handle the paint event
        /// <summary>
        /// 绘制图形
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Pen pen = new Pen(lineColor, 1);

            e.Graphics.DrawRectangle(pen, 0, 0, this.Size.Width - 1, this.Size.Height - 1);

            pen.Dispose();
            // 调用基类 OnPaint
            base.OnPaint(e);
        }

        private void InitializeComponent()
        {
        }
    }
}
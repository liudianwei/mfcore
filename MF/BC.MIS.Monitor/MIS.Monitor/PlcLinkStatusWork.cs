using EvetnArgData;
using System;
using System.ComponentModel;

namespace Monitor
{
    /// <summary>
    /// PlcLinkStatusWork 的摘要说明。
    /// </summary>
    public class PlcLinkStatusWork : PlcLinkStatus
    {
        private IContainer components;

        /// <summary>
        ///  OpName
        /// 工位名称
        /// </summary>
        private string opName;

        //private Timer timer_HeartBeat_PLC;
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

        private System.Resources.ResourceManager resources;

        /// <summary>
        ///
        /// </summary>
		public PlcLinkStatusWork()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();
        }

        /// <summary>
        /// 根据变量是否正确，显示通讯状态 通讯数据交换
        /// </summary>
        /// <param name="e"></param>
        public void PlcLinkStatusWork_Show(CustomeEvetnArgs e)
        {
            if (Convert.ToInt32(e.TagQuality) == 192)
            {
                // ApplicationLog.WriteLog("PlcLinkStatusWork_Show(e):_Class" + e.OpName + "时间：" + DateTime.Now.ToString() + "    变量值： " + e.TagValue.ToString());

                //通讯正常
                PlcLinkGood();
            }
            else
            {
                //通讯失败
                PlcLinkBad();
            }
        }

        /// <summary>
        /// 通讯正常
        /// </summary>
        private void PlcLinkGood()
        {
            resources = new System.Resources.ResourceManager(typeof(PlcLinkStatusWork));
            this.Image = ((System.Drawing.Image)(resources.GetObject("$this.PlcLinkGoodImage")));
            this.Invalidate();
        }

        /// <summary>
        /// 通讯失败
        /// </summary>
        private void PlcLinkBad()
        {
            resources = new System.Resources.ResourceManager(typeof(PlcLinkStatusWork));
            this.Image = ((System.Drawing.Image)(resources.GetObject("$this.PlcLinkBadImage")));
            this.Invalidate();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlcLinkStatusWork));
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            this.PlcLinkBadImage = ((System.Drawing.Image)(resources.GetObject("$this.PlcLinkBadImage")));
            this.PlcLinkGoodImage = ((System.Drawing.Image)(resources.GetObject("$this.PlcLinkGoodImage")));
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion 组件设计器生成的代码
    }
}
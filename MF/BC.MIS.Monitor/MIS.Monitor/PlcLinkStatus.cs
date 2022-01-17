using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Monitor
{
    /// <summary>
    /// PlcLinkStatus 的摘要说明。
    /// </summary>
    public class PlcLinkStatus : PictureBox
    {
        /// <summary>
        /// 显示PLC连接状态
        ///
        /// </summary>
        private Container components = null;

        /// <summary>
        ///  PLC连接正常时图形
        /// </summary>
        private Image plcLinkGoodImage;

        /// <summary>
        ///  PLC连接正常时图形
        /// </summary>
        [
        Description("PLC连接正常时图形"),
        ]
        public Image PlcLinkGoodImage
        {
            get
            {
                return plcLinkGoodImage;
            }
            set
            {
                plcLinkGoodImage = value;
            }
        }

        /// <summary>
        ///  PLC连接不正常时图形
        /// </summary>
        private Image plcLinkBadImage;

        /// <summary>
        ///  PLC连接不正常时图形
        /// </summary>
        [
        Description("PLC连接不正常时图形"),
        ]
        public Image PlcLinkBadImage
        {
            get
            {
                return plcLinkBadImage;
            }
            set
            {
                plcLinkBadImage = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
		public PlcLinkStatus()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();

            // TODO: 在 InitializeComponent 调用后添加任何初始化
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PlcLinkStatus));
            ((ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            //
            // PlcLinkStatus
            //
            this.Image = ((Image)(resources.GetObject("$this.Image")));
            ((ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion 组件设计器生成的代码
    }
}
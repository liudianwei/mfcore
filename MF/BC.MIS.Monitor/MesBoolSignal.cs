using System.ComponentModel;
using System.Windows.Forms;

namespace MesControl
{
    public partial class MesBoolSignal : UserControl
    {
        public MesBoolSignal()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 外部调用控件触发的信号
        /// </summary>
        /// <param name="Signal"></param>
        public void InputBoolSignal(bool Signal)
        {
            if (Signal)
            {
                pictureBox_Signal.BackgroundImage = Monitor.Properties.Resources.green;
            }
            else
            {
                pictureBox_Signal.BackgroundImage = Monitor.Properties.Resources.init;
            }
        }

        [Browsable(true)]
        [Description("控件中显示的文本内容"), Category("SignalText"), DefaultValue("")]
        public string SignalText
        {
            get
            {
                return label_SignalName.Text;
            }

            set
            {
                label_SignalName.Text = value;
            }
        }

        private SignalImage_Enum enum1;

        [Browsable(true)]
        [Description("控件用于显示状态的图片"), Category("SignalImage"), DefaultValue("none")]
        public SignalImage_Enum SignalImage
        {
            get
            {
                return enum1;
            }
            set
            {
                SetSignalImage(value);
                enum1 = value;
            }
        }

        public enum SignalImage_Enum
        {
            none, init, green, yellow, red
        }

        private void SetSignalImage(SignalImage_Enum Image)
        {
            switch (Image)
            {
                case SignalImage_Enum.none:
                    pictureBox_Signal.BackgroundImage = Monitor.Properties.Resources.none;
                    break;

                case SignalImage_Enum.init:
                    pictureBox_Signal.BackgroundImage = Monitor.Properties.Resources.init;
                    break;

                case SignalImage_Enum.green:
                    pictureBox_Signal.BackgroundImage = Monitor.Properties.Resources.green;
                    break;

                case SignalImage_Enum.yellow:
                    pictureBox_Signal.BackgroundImage = Monitor.Properties.Resources.yellow;
                    break;

                case SignalImage_Enum.red:
                    pictureBox_Signal.BackgroundImage = Monitor.Properties.Resources.red;
                    break;
            }
        }

        /// <summary>
        /// 工位号
        /// </summary>
        public string OpName { get; set; }
    }
}
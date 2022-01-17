using System.ComponentModel;
using System.Windows.Forms;

namespace MesControl
{
    public delegate void PlcStatusHandle();
    public partial class PlcConnect: UserControl
    {
        public PlcConnect()
        {
            InitializeComponent();
            
        }
        public static PlcStatusHandle PlcBreakChange,PlcConnectChange;


        #region 内部变量
        int externalTimerTimes =0;

        int testPlcBreakInterval=10000;

        int testPlcBreakTime=0;

        int externalTimerInterval = 200; 

        bool isBreakPLC = true;

        bool plcTestEnable; 

        bool plcBreakIsChanged=false; 
        bool plcConnectIsChanged = false;

        #endregion
        #region 内部方法
        
        void TimerTimeChange()
        {
            if (externalTimerTimes == 999)
            {
                externalTimerTimes = 0;
            }
            else
            {
                externalTimerTimes++;
            }

            if (testPlcBreakTime > 0)
            {
                testPlcBreakTime--;
                plcBreakIsChanged = false;

                if (PlcConnectChange!=null&&!plcConnectIsChanged)
                {
                    PlcConnectChange();
                    plcConnectIsChanged = true;
                }
            }
            else
            {
                if (plcTestEnable)
                {
                    if(PlcBreakChange != null&& !plcBreakIsChanged)
                    {
                        PlcBreakChange();
                        plcBreakIsChanged = true;
                    }
                    plcConnectIsChanged = false;
                }
                isBreakPLC = true;
            }

        }
        #endregion

        #region 外部方法
        public void ExternalTimer()
        {

            if (!isBreakPLC)
            {
                switch (externalTimerTimes % 5)
                {
                    case 0:
                        pictureBox_OpName.BackgroundImage = Monitor.Properties.Resources.PlcConnect0;
                        break;
                    case 1:
                        pictureBox_OpName.BackgroundImage = Monitor.Properties.Resources.PlcConnect1;
                        break;
                    case 2:
                        pictureBox_OpName.BackgroundImage = Monitor.Properties.Resources.PlcConnect2;
                        break;
                    case 3:
                        pictureBox_OpName.BackgroundImage = Monitor.Properties.Resources.PlcConnect3;
                        break;
                    case 4:
                        pictureBox_OpName.BackgroundImage = Monitor.Properties.Resources.PlcConnect4;
                        break;

                }
            }
            else
            {
                pictureBox_OpName.BackgroundImage = Monitor.Properties.Resources.BreakPLC;
            }


            TimerTimeChange();
        }



     
        public void ExternalPlcConnect()
        {
            testPlcBreakTime = (testPlcBreakInterval / externalTimerInterval) + 1;
            isBreakPLC = false;

        }
        
        public void PlcBreak()
        {
            pictureBox_OpName.BackgroundImage = Monitor.Properties.Resources.BreakPLC;
            isBreakPLC = true;
        }

        #endregion
        
        #region  属性

        [Browsable(true)]
        [Description("控件中显示的文本内容"), Category("外观"), DefaultValue("OP0000")]
        public string LabelText
        {
            get
            {
                return label_OpName.Text;
            }

            set
            {
                label_OpName.Text = value;
            }

        }

        
        [Browsable(true)]
        [Description("外部定时器周期"), Category("行为"), DefaultValue("200")]
        public int ExternalTimerInterval
        {
            get
            {
                return externalTimerInterval;
            }

            set
            {
                externalTimerInterval = value;
            }

        }

        [Browsable(true)]
        [Description("检测PLC在线状态超时，超过此时间，PLC为离线状态"), Category("行为"), DefaultValue("200")]
        public int TestPlcBreakInterval
        {
            get
            {
                return testPlcBreakInterval;
            }

            set
            {
                testPlcBreakInterval = value;
            }

        }

        [Browsable(true)]
        [Description("指示PLC是否为断开状态"), Category("行为"), DefaultValue("True")]
        public bool  IsBreakPLC
        {
            get
            {
                return isBreakPLC;
            }

            set
            {
                isBreakPLC = value;
            }

        }

        [Browsable(true)]
        [Description("是否启用检测PLC在线状态功能"), Category("行为"), DefaultValue("True")]
        public bool PlcTestEnable
        {
            get
            {
                return plcTestEnable;
            }

            set
            {
                plcTestEnable = value;
            }

        }

        #endregion


    }
}

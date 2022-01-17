using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MesControl
{
    public delegate void WorkStationPlcBreakChanged();
    public partial class PlcWorkStationSignal : UserControl
    {
        public PlcWorkStationSignal()
        {
            InitializeComponent();
            PlcConnect.PlcBreakChange = new PlcStatusHandle(internalPlcBreak);
            PlcConnect.PlcConnectChange = new PlcStatusHandle(internalPlcConnect);
          
        }

        public EventHandler WorkStationPlcStatusChange;

        #region 工位信号方法与属性

        public  void InitSignalControl()
        {


            for (int i = 0; i < SignalItems.Count; i++)
            {

                MesBoolSignal mesBoolSignalNew = new MesBoolSignal();
                this.groupBox_WorkStationGrp.Controls.Add(mesBoolSignalNew);//扩展GroupBox
                mesBoolSignalNew.BackColor = System.Drawing.Color.Transparent;
                mesBoolSignalNew.Location = new System.Drawing.Point(7, 46 + (25 * i));
                mesBoolSignalNew.Name = SignalItems[i].Name;
                mesBoolSignalNew.SignalImage = MesBoolSignal.SignalImage_Enum.none;
                mesBoolSignalNew.SignalText = SignalItems[i].SignalText;
                mesBoolSignalNew.Size = new System.Drawing.Size(110, 22);
                mesBoolSignalNew.TabIndex = i;

            }

            //groupbox大小
            groupBox_WorkStationGrp.Size = new Size(125, 50 + (SignalItems.Count * 25));
            //控件大小
            this.Size = new Size(136, 60 + (SignalItems.Count * 25));

        }

        private List<MesBoolSignal> signalItems = new List<MesBoolSignal>();
        [Browsable(true)]
        [Description("MES信号控件集合"), Category("数据")]
        [TypeConverter(typeof(System.ComponentModel.CollectionConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<MesBoolSignal> SignalItems
        {
            get
            {
                return signalItems;
            }

            set
            {
                signalItems = value;
                InitSignalControl();
            }
        }

        /// <summary>
        ///更改工位各个信号状态（默认）
        /// </summary>
        /// <param name="SignalName"></param>
        /// <param name="Value"></param>
        public void SignalChange(string SignalName, bool Value)
        {
            MesBoolSignal thisControl = (MesBoolSignal)Controls.Find(SignalName, true)[0];
            if (thisControl != null)
                thisControl.SignalImage = Value ? MesBoolSignal.SignalImage_Enum.green : MesBoolSignal.SignalImage_Enum.init;

        }

        #endregion

        #region  PLC在线状态属性与方法

        ///////////////////////////////////////////////方法///////////////////////////////////////////////////
        
        public void PlcConnectStatusChange(bool value)
        {
            plcConnect_WorkStation.ExternalPlcConnect();

        }
        public void ExternalTimerExcute()
        {
            plcConnect_WorkStation.ExternalTimer();
        }
        
        public void PlcBreak()
        {
            plcConnect_WorkStation.PlcBreak();
        }
        
        void internalPlcBreak()
        {
            if (WorkStationPlcStatusChange != null)
            {
                WorkStationPlcStatusChange(groupBox_WorkStationGrp.Text+"&Breaked",null);
            }
               
        }

        void internalPlcConnect()
        {
            if (WorkStationPlcStatusChange != null)
            {
                WorkStationPlcStatusChange(groupBox_WorkStationGrp.Text + "&Connected", null);
            }
        }


        //////////////////////////////////////////////////////////////属性///////////////////////////////////////////////

        [Browsable(true)]
        [Description("检测PLC在线状态开关"), Category("行为"), DefaultValue(true)]
        public bool TestPLC_Enable
        {
            get
            {
                return plcConnect_WorkStation.PlcTestEnable;
            }
            set
            {
                plcConnect_WorkStation.PlcTestEnable = value;
            }
        }

        [Browsable(true)]
        [Description("PLC心跳超时（超过此时间认为PLC离线）"), Category("行为"), DefaultValue(10000)]
        public int TestPLC_Interval
        {
            get
            {
                return plcConnect_WorkStation.TestPlcBreakInterval;
            }
            set
            {

                plcConnect_WorkStation.TestPlcBreakInterval = value;

            }
        }
        

        [Browsable(true)]
        [Description("外部定时器循环时间"), Category("行为"), DefaultValue(200)]
        public int ExternalTimerInterval
        {
            get
            {
                return plcConnect_WorkStation.ExternalTimerInterval;
            }
            set
            {
                plcConnect_WorkStation.ExternalTimerInterval = value;
            }
        }

        [Browsable(true)]
        [Description("PLC在线状态"), Category("行为"), DefaultValue(false)]
        public bool PLC_ConnectStatus
        {
            get
            {
                return plcConnect_WorkStation.IsBreakPLC;
            }
            set
            {
                plcConnect_WorkStation.IsBreakPLC = value;
            }
        }

        #endregion

        #region 当前控件方法与属性

        [Browsable(true)]
        [Description("控件显示的工位号"), Category("外观"), DefaultValue("OP0000")]
        public string WorkStationNameText
        {
            get
            {
                return groupBox_WorkStationGrp.Text;
            }
            set
            {
                groupBox_WorkStationGrp.Text = value;

            }

        }

        [Browsable(true)]
        [Description("控件中plc通讯显示的文本内容"), Category("外观"), DefaultValue("PLC连接")]
        public string WS_PlcLabelText
        {
            get
            {
                return plcConnect_WorkStation.LabelText ;
            }

            set
            {
                plcConnect_WorkStation.LabelText  = value;
            }

        }


        #endregion

    }
}

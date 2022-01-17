using System.Data;

namespace EvetnArgData
{
    /// <summary>
    /// MIS监控系统全局变量
    /// </summary>
    public class GlobleData
    {
        /// <summary>
        /// MIS监控系统显示发动机在线数量列表
        /// </summary>
        static public TagData tagDataShowEngineList = new TagData();

        /// <summary>
        /// MIS监控系统显示发动机在线数量
        /// </summary>
        static public TagData tagDataShowEngineCountOnLine = new TagData();

        /// <summary>
        /// 显示对话框，显示工位机床状态、质量数据
        /// </summary>
        static public int heartBeatPLC;

        /// <summary>
        /// 显示对话框，显示工位机床状态、质量数据
        /// </summary>
        static public TagData tagDataDelayrCall = new TagData();

        /// <summary>
        /// 显示对话框，显示工位机床状态、质量数据
        /// </summary>
        static public TagData tagDataEngineStatusShowDialog = new TagData();

        /// <summary>
        /// 发动机工作状态（到位或离开）
        /// </summary>
        static public TagData tagDataEngineStatus = new TagData();

        /// <summary>
        /// 发动机号的变化
        /// </summary>
        static public TagData tagDataEngineNumber = new TagData();

        /// <summary>
        /// 文本颜色
        /// </summary>
        static public TagData tagDataEngineNumberColor = new TagData();

        /// <summary>
        /// 发动机号动态追踪
        /// 设定发动机号动态追踪
        /// 是否追踪
        /// 被追踪发动机号
        /// </summary>
        static public TagData tagDataEngineNumberTrace = new TagData();

        /// <summary>
        /// 发动机号动态追踪
        /// 设定发动机号动态追踪时显示工位号
        /// 是否追踪
        /// 被追踪发动机号
        /// </summary>
        static public TagData tagDataEngineNumberTraceOpName = new TagData();

        /// <summary>
        /// 机床报警
        /// </summary>
        static public TagData tagDataAlarm = new TagData();

        /// <summary>
        /// 机床报警
        /// </summary>
        static public TagData tagDataAlarmListView = new TagData();

        /// <summary>
        /// 机床状态
        /// </summary>
        static public TagData tagDataMachine = new TagData();

        /// <summary>
        /// PLC通讯状态
        /// </summary>
        static public TagData tagDataHeartBeat = new TagData();

        /// <summary>
        /// PLC通讯状态
        /// </summary>
        static public TagData tagDataHeartBeat_PlcLinkStatusWork = new TagData();

        /// <summary>
        /// 卡色状态
        /// </summary>
        static public TagData tagDataCartColor = new TagData();

        /// <summary>
        /// 压装合格状态
        /// </summary>
        static public TagData tagDataStandardCircleCartColorOpNamePress = new TagData();

        /// <summary>
        /// 显示8种颜色的圆形控件
        /// 输入值，1(Blue)，2(Cyan)，4(Lime)，8(Yellow)，16(Red)，32(Brown)，64(Pink)，128(Green)
        /// </summary>
        static public TagData tagDataCartColorMultStatus = new TagData();

        /// <summary>
        /// =9010 MES 工件到位 CF1=1
        /// </summary>
        static public TagData tagDataMES_CF1 = new TagData();

        /// <summary>
        /// =9020 MES 写发动机号和发动机型号代码
        /// </summary>
        static public TagData tagDataMES_EngineNumber = new TagData();

        /// <summary>
        /// =显示变量值
        /// </summary>
        static public TagData tagData_Lable = new TagData();

        /// <summary>
        /// =显示变量值
        /// </summary>
        static public TagData tagData_Lable_OpName = new TagData();

        /// <summary>
        /// =显示变量值合格标志
        /// </summary>
        static public TagData tagData_StandardCircleOpName = new TagData();

        /// <summary>
        /// 用于激发MesServer
        /// </summary>
        static public TagData tagData_MesServer = new TagData();

        /// <summary>
        /// 用于激发MesServer_Else
        /// </summary>
        static public TagData tagData_MesServer_Else = new TagData();

        /// <summary>
        /// MES 运行方式
        /// =1 集中方式
        /// =0 分散方式
        /// </summary>
        static public int MesRunID;

        /// <summary>
        /// MES 运行方式
        /// =1 演示
        /// =0实际
        /// </summary>
        static public int TagAdrDemo;

        /// <summary>
        /// 报警最大行数
        /// </summary>
        public static int resultDataAlarmRowMaxCount = 500;

        /// <summary>
        /// 全部报警数据数据
        /// </summary>
        private static DataTable resultDataAlarmAll;

        /// <summary>
        /// 全部报警数据数据
        /// </summary>
        public static DataTable ResultDataAlarmAll
        {
            get
            {
                return resultDataAlarmAll;
            }
            set
            {
                resultDataAlarmAll = value;
            }
        }

        /// <summary>
        /// 报警最大行数
        /// </summary>
        public static int ResultDataAlarmRowMaxCount
        {
            get
            {
                return resultDataAlarmRowMaxCount;
            }
            set
            {
                resultDataAlarmRowMaxCount = value;
            }
        }
    }
}
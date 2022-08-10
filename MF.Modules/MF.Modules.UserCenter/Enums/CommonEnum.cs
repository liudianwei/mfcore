using System.ComponentModel;

namespace UserCenter.Enums
{
    enum CommonEnum
    {
        PARENT_ID=0,
        ENABLE=1,
        IS_PRODUCT=1,
        STRING_ACTIVE=1
    }

    public enum EnableState
    {
        [Description("已启用")]
        YES = 0,

        [Description("已禁用")]
        NO = 1,
    }
    /// <summary>
    /// 定时任务
    /// </summary>
    public enum JobTaskEnum
    {
        [Description("停止")]
        PopStop = 0,

        [Description("运行")]
        PopFinished = 1,

        [Description("启动中")]
        PopWait = 3,

        [Description("停止中...")]
        PopProceed = 5,
    }
}
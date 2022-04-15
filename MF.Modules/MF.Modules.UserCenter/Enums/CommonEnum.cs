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
}
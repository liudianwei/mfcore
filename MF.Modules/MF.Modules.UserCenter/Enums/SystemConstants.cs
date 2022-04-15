using MF.Core.Security;

using System;

namespace UserCenter.Enums
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public partial class SystemConstants
    {
        public static readonly string defaultPassword = SecurityUtil.ToMd5("123456").ToLower();
        public static readonly string superId = Guid.Empty.ToString();
        public static readonly string superName = "system";

        public static readonly string excelOutPath = "temp\\export\\";

        public static readonly string YES = "1";
        public static readonly string NO = "0";

        public static readonly string ALL = "all";
        public static readonly string MEASURE_DEFAULT = "Default";
        public static readonly int REMOTE_LIMIT = 10;
        public static readonly string OEE_OPNAM = "OP010";//OEE的仪表盘统计工位
    }
}
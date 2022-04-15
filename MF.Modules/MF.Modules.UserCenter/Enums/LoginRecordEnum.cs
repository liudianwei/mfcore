using Ardalis.SmartEnum;

namespace UserCenter.Enums
{
    /// <summary>
    /// 状态
    /// </summary>
    public sealed class LoginRecordEnum : SmartEnum<LoginRecordEnum>
    {
        // 登录失败
        public static readonly LoginRecordEnum loginFailed = new LoginRecordEnum("loginFailed", 0);

        // 登录成功
        public static readonly LoginRecordEnum loginIn = new LoginRecordEnum("loginIn", 1);

        // 退出
        public static readonly LoginRecordEnum loginOut = new LoginRecordEnum("loginOut", 2);

        private LoginRecordEnum(string name, int value) : base(name, value)
        {
        }

        public static LoginRecordEnum GetDefalutType(string name)
        {
            return TryFromName(name, out LoginRecordEnum userStatus) ? userStatus : loginFailed;
        }

        public static LoginRecordEnum GetDefalutType(int value)
        {
            return TryFromValue(value, out LoginRecordEnum userStatus) ? userStatus : loginFailed;
        }
    }
}
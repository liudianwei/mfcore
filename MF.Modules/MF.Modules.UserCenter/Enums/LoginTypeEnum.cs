using Ardalis.SmartEnum;

namespace UserCenter.Enums
{
    public sealed class LoginTypeEnum : SmartEnum<LoginTypeEnum>
    {
        /// <summary>
        /// 登陆
        /// </summary>
        public static readonly LoginTypeEnum Login = new LoginTypeEnum("login", 1);

        /// <summary>
        /// 退出
        /// </summary>
        public static readonly LoginTypeEnum Logout = new LoginTypeEnum("logout", 2);

        private LoginTypeEnum(string name, int value) : base(name, value)
        {
        }

        public static LoginTypeEnum GetDefalutType(string name)
        {
            return TryFromName(name, out LoginTypeEnum type) ? type : Login;
        }

        public static LoginTypeEnum GetDefalutType(int value)
        {
            return TryFromValue(value, out LoginTypeEnum type) ? type : Logout;
        }
    }
}
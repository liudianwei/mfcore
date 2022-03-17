using System;

namespace CodeGen
{
    /// <summary>
    /// 数据库连接参数
    /// </summary>
    public class DbParam
    {
        public string DbName { set; get; }

        public string Host { set; get; }

        public string Port { set; get; }

        public string Password { set; get; }

        public string UserId { set; get; }

        public String DbType { set; get; }
        public bool DbFlag { set; get; } = true;
    }
}
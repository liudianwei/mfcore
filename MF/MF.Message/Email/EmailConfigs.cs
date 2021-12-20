namespace MF.Message.Email
{
    /// <summary>
    /// 邮件配置参数
    /// </summary>
    public class EmailConfigs
    {
        public bool EnableSsl { get; set; } = false;

        public string Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
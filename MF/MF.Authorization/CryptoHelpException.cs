using System;

namespace MF.Authorization
{
    /// <summary>
    /// 异常处理类
    /// </summary>
    public class CryptoHelpException : ApplicationException
    {
        public CryptoHelpException(string msg) : base(msg)
        {
        }
    }
}
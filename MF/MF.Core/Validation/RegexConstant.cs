using System;
using System.Collections.Generic;
using System.Text;

namespace MF.Core.Validation
{
    public static class RegexConstant
    {
        /// <summary>
        /// 检查汉字的正则表达式
        /// </summary>
        public static readonly string CheckChinaPattern = @"[\u4e00-\u9fa5]";

        public static readonly string CheckEmailPattern = "^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$";
    }
}
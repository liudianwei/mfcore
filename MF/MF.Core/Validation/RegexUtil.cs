using System.Text.RegularExpressions;
using MF.Core.Extensions;

namespace MF.Core.Validation
{
    public static class RegexUtil
    {
        /// <summary>
        ///是否包含或全部是中文
        /// </summary>
        /// <param name="str"></param>
        /// <param name="match">true全中文，false含有中文</param>
        /// <returns></returns>
        public static bool IsChinese(this string str, bool match = false)
        {
            var bytes = str.ToBytes(EncodingType.gb2312);
            return match ? bytes.Length == str.Length * 2 : bytes.Length > str.Length;
        }

        public static bool IsMatch(this string value, string pattern, RegexOptions options)
        {
            return value != null && Regex.IsMatch(value, pattern, options);
        }

        public static bool IsMatch(this string value, string pattern)
        {
            return value != null && Regex.IsMatch(value, pattern);
        }
    }
}
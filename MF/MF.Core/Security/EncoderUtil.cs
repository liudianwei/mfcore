using System.Net;
using System.Web;
using MF.Core.Extensions;

namespace MF.Core.Security
{
    public static class EncoderUtil
    {
        public static string HtmlHttpUtilityEncoder(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string HtmlHttpUtilityDecoder(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        public static string UrlHttpUtilityEncoder(string str, EncodingType type = EncodingType.UTF8)
        {
            return HttpUtility.UrlEncode(str, BytesExt.GetEncoding(type));
        }

        public static string UrlHttpUtilityDecoder(string str, EncodingType type = EncodingType.UTF8)
        {
            return HttpUtility.UrlDecode(str, BytesExt.GetEncoding(type));
        }

        public static string WebUrlEncode(string str)
        {
            return WebUtility.UrlEncode(str);
        }

        public static string WebUrlDecode(string str)
        {
            return WebUtility.UrlDecode(str);
        }

        public static string WebHtmlEncode(string str)
        {
            return WebUtility.HtmlEncode(str);
        }

        public static string WebHtmlDecode(string str)
        {
            return WebUtility.HtmlDecode(str);
        }
    }
}
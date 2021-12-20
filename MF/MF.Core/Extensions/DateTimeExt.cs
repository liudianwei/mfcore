using System;

namespace MF.Core.Extensions
{
    public static class DateTimeExt
    {
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string DateTimeFormat1 = "yyyy-MM-dd HH:mm";
        public const string DateTimeFormat2 = "yyyy/MM/dd HH:mm:ss";
        public const string DateTimeFormatString = "yyyyMMddHHmmss";
        public const string DateTimeShortFormat = "yyyy-MM-dd";
        public const string DateTimeShortFormat2 = "yyyy/MM/dd";
        public const string SnokId = "yyyyMMddHHmmssffff";
        public static DateTime DateTime => DateTime.Now;

        public static DateTime ToDateTime(this string str)
        {
            return DateTime.TryParse(str, out DateTime date) == true ? date : DateTime.MinValue;
        }

        public static DateTime ToDateTimeB(this string str)
        {
            return DateTime.TryParse(str + " 00:00:00.000", out DateTime date) == true ? date : DateTime.MinValue;
        }

        public static DateTime ToDateTimeE(this string str)
        {
            return DateTime.TryParse(str + " 23:59:59.997", out DateTime date) == true ? date : DateTime.MinValue;
        }

        public static string ToDateTimeString(this DateTime dateTime, string format = DateTimeFormat)
        {
            return dateTime.ToString(format);
            //return dateTime.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        public static DateTime GetDateTime(string format = DateTimeFormat)
        {
            return DateTime.ToString(format).ToDateTime();
        }

        public static string GetDateTimeS(string format = DateTimeFormat)
        {
            return DateTime.ToString(format);
        }

        public static long GetTotalMilliseconds()
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }

        public static DateTime TimeStampToDateTime(string timeStamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1, 0, 0, 0);
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// 已重载.计算两个日期的时间间隔,返回的是时间间隔的日期差的绝对值.
        ///
        /// 第一个日期和时间
        /// 第二个日期和时间
        ///
        public static string DateDiff(DateTime? DateTime1, DateTime? DateTime2)
        {
            string dateDiff;
            try
            {
                if (DateTime1 != null && DateTime2 != null)
                {
                    TimeSpan ts1 = new TimeSpan((long)DateTime1?.Ticks);
                    TimeSpan ts2 = new TimeSpan((long)DateTime2?.Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    dateDiff = ts.TotalMinutes.ToFormat(2).ToString();
                    //var hasDot = dateDiff.IndexOf('.');
                    //if (hasDot > 0)
                    //{
                    //    if ((hasDot + 3) < dateDiff.Length)
                    //    {
                    //        dateDiff = dateDiff[..(hasDot + 3)];
                    //    }
                    //}
                }
                else
                {
                    dateDiff = "0.00";
                }
            }
            catch
            {
                dateDiff = "0.00";
            }

            return dateDiff;
        }
    }
}
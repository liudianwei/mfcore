using System;

namespace MF.Orm.SqlSugar
{
    public static class SqlFuncL
    {
        /// <summary>
        /// 将时间格式化成yyyy-MM-dd形式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDateFormat<T>(T value)
        {
            throw new NotSupportedException($"Can only be used in expressions {value}");
        }
    }
}
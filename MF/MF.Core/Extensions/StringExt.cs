using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace MF.Core.Extensions

{
    /// <summary>
    /// String扩展
    /// </summary>
    public static class StringExt
    {
        /// <summary>
        /// 是null还是string.Empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNull(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// null、空还是仅由空白字符串组成
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            //return str.AsSpan().IsWhiteSpace();
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsEmptyZero(this string str)
        {
            return string.IsNullOrWhiteSpace(str) || str == "0";
        }

        /// <summary>
        /// 判断是不是为null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullT<T>(this T t) where T : class
        {
            //return t is null;
            return t is null;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            //return source is null || source.Count <= 0;
            return source.Any();
        }

        public static bool NotNullOrEmpty<T>(this ICollection<T> source)
        {
            //return source is null || source.Count <= 0;
            return !IsNullOrEmpty(source);
        }

        /// <summary>
        /// 判断List<T>是不是为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullT<T>(this List<T> t) where T : class
        {
            return t is null || t.Count == 0;
        }

        public static bool NotNullT<T>(this List<T> t) where T : class
        {
            return !IsNullT(t);
        }

        public static bool IsNullLt<T>(this List<T> t)
        {
            return t is null || t.Count == 0;
        }

        public static bool NotNullLt<T>(this List<T> t)
        {
            return !IsNullLt(t);
        }

        public static bool IsNullDt(this DataTable dt)
        {
            return dt is null || dt.Rows.Count == 0;
        }

        public static bool IsNullT<T>(this IEnumerable<T> value)
        {
            if (value is null)
                return true;
            return !value.Any();
        }

        public static bool IsNull<T>(this T t) where T : class
        {
            if (t is null)
            {
                return true;
            }
            if (t is string[])
            {
                return (t as string[]).Length == 0;
            }
            if (t is string)
            {
                return string.IsNullOrWhiteSpace(t.ToString().Trim());
            }
            if (t is DBNull)
            {
                return true;
            }
            if (t.GetType() == typeof(DataTable))
            {
                Type entityType = typeof(T);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
                DataTable dt = new DataTable();
                foreach (PropertyDescriptor prop in properties)
                {
                    dt.Columns.Add(prop.Name);
                }
                return dt is null || dt.Rows.Count == 0;
            }
            return false;
        }

        public static bool NotNull<T>(this T t) where T : class
        {
            return !IsNull(t);
        }

        public static string[] ToSplit(this object obj, char c = '|')
        {
            return obj.ToString().Split(c);
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToFirstUpper(this string str)
        {
            if (str.IsEmpty())
            {
                throw new ArgumentNullException(nameof(str));
            }
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        /// <summary>
        /// 字符串的长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="flag">默认字符集是UTF8,</param>
        /// <returns></returns>
        public static int LengthH(this string str, EncodingType type = EncodingType.UTF8)
        {
            return str.ToBytes(type).Length;
        }

        public static bool CompareIgnoreCase(string left, string right, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            if (left == right)
            {
                return true;
            }
            return string.Equals(left, right, stringComparison);
        }

        public static string[] SplitString(string str, int len)
        {
            ArrayList subs;
            if (str.IsEmpty() || len <= 0)
            {
                subs = new ArrayList
                {
                    [0] = str
                };
            }
            else
            {
                subs = new ArrayList();
                for (int i = 0; i < str.Length; i += len)
                {
                    if ((str.Length - i) > len)
                    {
                        subs.Add(str.Substring(i, len));
                    }
                    else
                    {
                        subs.Add(str[i..]);
                    }
                }
            }
            return (string[])subs.ToArray(typeof(string));
        }

        /// <summary>
        /// 替换第一个符合条件的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue">所要替换掉的值</param>
        /// <param name="newValue">所要替换的值</param>
        /// <returns>返回替换后的值 所要替换掉的值为空或Null，返回原值</returns>
        public static string ReplaceFirst(this string value, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
                return value;

            int idx = value.IndexOf(oldValue);
            if (idx == -1)
                return value;
            value = value.Remove(idx, oldValue.Length);
            return value.Insert(idx, newValue);
        }

        /// <summary>
        /// 替换最后一个符合条件的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue">所要替换掉的值</param>
        /// <param name="newValue">所要替换的值</param>
        /// <returns>返回替换后的值 所要替换掉的值为空或Null，返回原值</returns>
        public static string ReplaceLast(this string value, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
                return value;

            int idx = value.LastIndexOf(oldValue);
            if (idx == -1)
                return value;
            value = value.Remove(idx, oldValue.Length);
            return value.Insert(idx, newValue);
        }

        /// <summary>
        /// 添加单引号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStoreString(this string value)
        {
            return $"'{value}'";
        }
    }
}
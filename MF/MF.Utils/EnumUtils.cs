using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MF.Utils
{
    public static class EnumUtils
    {
        private static readonly Dictionary<object, string> cache = new Dictionary<object, string>();
        private static readonly Dictionary<string, string> caches = new Dictionary<string, string>();

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">OrderType.Localtion</param>
        /// <returns>Description</returns>
        public static string GetEnumDescription<T>(T key)
        {
            cache.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
            {
                value = value.GetType()?.GetMember(value.ToString())?.FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description;
                cache.Add(key, value);
            }

            return value ?? string.Empty;
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">"0"</param>
        /// <returns>Description</returns>
        public static string GetEnumDescription<T>(string key)
        {
            Type type = typeof(T);
            var dictkey = type.FullName + key;
            string description = string.Empty;
            //caches.TryGetValue(dictkey, out string description);
            //if (string.IsNullOrEmpty(description))
            //{
                Dictionary<string, string> dic = EnumFieldDescription(type);
                description = string.Empty;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    if (dic.ContainsKey(Convert.ToString((T)Enum.Parse(type, key))))
                    {
                        description = dic[Convert.ToString((T)Enum.Parse(type, key))];
                    }
                }
            //    caches.Add(dictkey, description);
            //}
            return description ?? string.Empty;
        }

        /// <summary>
        /// 得到enum字段描述列表
        /// </summary>
        /// <returns>返回字典</returns>
        private static Dictionary<string, string> EnumFieldDescription(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Type type = typeof(DescriptionAttribute);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                object[] arr = field.GetCustomAttributes(type, true);
                if (arr.Length > 0)
                {
                    dic.Add(field.Name, ((DescriptionAttribute)arr[0]).Description);
                }
            }
            return dic;
        }
    }
}
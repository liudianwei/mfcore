using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MF.Core.Extensions
{
    public static class IEnumerableExt
    {
        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T> func)
        {
            foreach (var item in iEnumberable)
            {
                func(item);
            }
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T, int> func)
        {
            var array = iEnumberable.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                func(array[i], i);
            }
        }

        /// <summary>
        /// IEnumerable转换为List'T'
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static List<T> CastToList<T>(this IEnumerable source)
        {
            return new List<T>(source.Cast<T>());
        }

        /// <summary>
        /// 将字符串列表格式化为数据库格式字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Parse2StoreString(this List<string> list)
        {
            var storeStringList = new List<string>();
            foreach (var item in list)
            {
                storeStringList.Add($"'{item}'");
            }
            return String.Join(",", storeStringList.ToArray());
        }
    }
}
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MF.Utils.Excel
{

    public static class MiniExcelUtil
    {
        private static readonly int Max = 65530;

        public static void ExportToExcel<T>(IEnumerable<T> list, string path)
        {
            var count = GetSheetsCount(list.Count());
            var sheets = new Dictionary<string, object>();
            for (int i = 1; i <= count; i++)
            {
                sheets.Add("Sheet" + i, GetPageDate(list, i));
            }

            MiniExcel.SaveAs(path, sheets, configuration: new OpenXmlConfiguration()
            {
                TableStyles = TableStyles.None,     //没有样式
                AutoFilter = false                  //没有筛选                
            });
        }

        /// <summary>
        /// 获取指定列数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static IEnumerable<T> GetPageDate<T>(IEnumerable<T> list, int i)
        {
            return list.Skip(Max * (i - 1)).Take(Max);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        //private static IEnumerable<T> CreateSn<T>(IEnumerable<T> list, int startNum = 1)
        //{
        //    //int i = startNum;
        //    foreach (var item in list)
        //    {
        //        //item.Num = i;
        //        yield return item;
        //    }
        //}


        /// <summary>
        /// 计算有几个sheet
        /// </summary>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        private static int GetSheetsCount(int rowsCount)
        {
            int len = (int)Math.Ceiling((double)rowsCount / Max);
            return len < 1 ? 1 : len;
        }

    }
}
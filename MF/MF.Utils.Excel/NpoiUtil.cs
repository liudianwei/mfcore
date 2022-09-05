using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MF.Utils.Excel
{
    /// <summary>
    /// NPOI linux 报错 https://www.cnblogs.com/Robbery/p/10115234.html sudo ln -s
    /// /lib/x86_64-linux-gnu/libdl.so.2 /lib/x86_64-linux-gnu/libdl.so sudo apt install libgdiplus
    /// sudo ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
    /// </summary>
    public partial class NpoiUtil
    {
        private const int Max = 65530; //
        public static string[] excel = { ".xlsx", ".xls" };
        private static IWorkbook workbook;
        private static ICellStyle cellStyle;
        private static IFont font;

        #region NPOI excel Export

        private static void Workbook(ExcelVersion version = ExcelVersion.V2007)
        {
            switch (version)
            {
                case ExcelVersion.V2007:
                    workbook = new XSSFWorkbook();
                    cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    font = (XSSFFont)workbook.CreateFont();
                    font.FontName = "宋体";
                    cellStyle.SetFont(font);
                    break;

                case ExcelVersion.V2003:
                    workbook = new HSSFWorkbook();
                    cellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    font = (HSSFFont)workbook.CreateFont();
                    font.FontName = "宋体";
                    cellStyle.SetFont(font);
                    break;

                default:
                    workbook = new XSSFWorkbook();
                    cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    font = (XSSFFont)workbook.CreateFont();
                    font.FontName = "宋体";
                    cellStyle.SetFont(font);
                    break;
            }
        }

        /// <summary>
        /// return File(buffer, "application/ms-excel", "list.xlsx");
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="version"></param>
        /// <param name="_sheetname"></param>
        /// <returns></returns>
        public static byte[] Export(DataTable dt, ExcelVersion version = ExcelVersion.V2007, string _sheetname = "sheet1")
        {
            Workbook(version);
            ISheet sheet = workbook.CreateSheet(_sheetname);
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.CellStyle = cellStyle;
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }
            //数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.CellStyle = cellStyle;
                    cell.SetCellValue(dt.Rows[i][j]?.ToString());
                }
            }
            AutoSizeColumns(sheet);
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buffer = stream.ToArray();
            return buffer;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="version"></param>
        /// <param name="_sheetname"></param>
        /// <returns></returns>
        public static MemoryStream ExportDataTableToExcel(DataTable sourceTable, ExcelVersion version = ExcelVersion.V2007, string _sheetname = "sheet")
        {
            Workbook(version);
            int dtRowsCount = sourceTable.Rows.Count;
            //int SheetCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dtRowsCount) / Max));
            int SheetNum = 1;
            int rowIndex = 1;
            int tempIndex = 1; //标示
            ISheet sheet = workbook.CreateSheet(_sheetname + SheetNum);
            for (int i = 0; i < dtRowsCount; i++)
            {
                if (i == 0 || tempIndex == 1)
                {
                    IRow headerRow = sheet.CreateRow(0);
                    foreach (DataColumn column in sourceTable.Columns)
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
                HSSFRow dataRow = (HSSFRow)sheet.CreateRow(tempIndex);
                foreach (DataColumn column in sourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(sourceTable.Rows[i][column].ToString());
                }
                if (tempIndex == Max)
                {
                    SheetNum++;
                    sheet = workbook.CreateSheet(_sheetname + SheetNum);//
                    tempIndex = 0;
                }
                rowIndex++;
                tempIndex++;
                AutoSizeColumns(sheet);
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            //sheet = null;
            // headerRow = null;
            //workbook = null;
            return ms;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="version"></param>
        /// <param name="_sheetname"></param>
        /// <returns></returns>
        public static byte[] ExportToByte(DataTable dt, ExcelVersion version = ExcelVersion.V2007, string _sheetname = "sheet")
        {
            Workbook(version);
            int count = dt.Rows.Count;
            int sheetCount = 1;
            if (count >= Max)
            {
                sheetCount = GetSheetsCount(count);
            }
            for (int i = 0; i < sheetCount; i++)
            {
                var sheetname = _sheetname + (i + 1);

                ISheet sheet = workbook.CreateSheet(sheetname);
                IRow row = sheet.CreateRow(0);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle = cellStyle;
                    cell.SetCellValue(dt.Columns[j].ColumnName);
                }
                var newDt = dt.Clone();
                dt.Rows.Cast<DataRow>().Skip(Max * i).Take(Max).ToList().ForEach(r => newDt.ImportRow(r));
                //数据
                for (int k = 0; k < newDt.Rows.Count; k++)
                {
                    IRow row1 = sheet.CreateRow(k + 1);
                    for (int m = 0; m < newDt.Columns.Count; m++)
                    {
                        ICell cell = row1.CreateCell(m);
                        cell.CellStyle = cellStyle;
                        cell.SetCellValue(newDt.Rows[k][m]?.ToString());
                    }
                }
                AutoSizeColumns(sheet);
            }

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buffer = stream.ToArray();
            return buffer;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="version"></param>
        /// <param name="ignoreExport"></param>
        /// <param name="_sheetname"></param>
        /// <returns></returns>
        public static byte[] Export<T>(List<T> list, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null, string _sheetname = "sheet")
        {
            if (!list.Any())
            {
                return null;
            }
            Workbook(version);
            int count = list.Count;
            int sheetCount = 1;
            if (count >= Max)
            {
                sheetCount = GetSheetsCount(count);
            }
            for (int i = 0; i < sheetCount; i++)
            {
                var sheetname = _sheetname + (i + 1);
                ISheet sheet = workbook.CreateSheet(sheetname);
                IRow row = sheet.CreateRow(0);
                Type entityType = list[0].GetType();
                PropertyInfo[] entityProperties = entityType.GetProperties();
                for (int j = 0; j < entityProperties.Length; j++)
                {
                    if (ignoreExport != null && ignoreExport.Contains(entityProperties[j].Name))
                    {
                        continue;
                    }
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle = cellStyle;
                    cell.SetCellValue(entityProperties[j].Name);
                }
                var maxList = list.Skip(Max * i).Take(Max).ToList();
                if (i == sheetCount - 1)
                {
                    maxList.Take((count - i * Max)).ToList();
                }
                else
                {
                    maxList.Take(Max).ToList();
                }
                //数据
                for (int k = 0; k < maxList.Count; k++)
                {
                    IRow row1 = sheet.CreateRow(k + 1);
                    var properties = maxList[k].GetType().GetProperties();
                    for (int m = 0; m < properties.Length; m++)
                    {
                        if (ignoreExport != null && ignoreExport.Contains(properties[m].Name))
                        {
                            continue;
                        }
                        ICell cell = row1.CreateCell(m);
                        cell.CellStyle = cellStyle;
                        var id = properties[m].GetValue(maxList[k])?.ToString();
                        cell.SetCellValue(id);
                    }
                }
                AutoSizeColumns(sheet);
            }
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buffer = stream.ToArray();
            return buffer;
        }

        /// <summary>
        /// 包含序号生成 默认从首行开始
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="_sheetname"></param>
        /// <param name="version"></param>
        /// <param name="ignoreExport"></param>
        /// <param name="sn">是否包含序号</param>
        /// <returns></returns>
        public static IWorkbook ExportToExcel<T>(List<T> list, string _sheetname = "sheet", bool sn = true, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
        {
            return ExportToExcel(list, 0, _sheetname, sn, version, ignoreExport);
        }

        /// <summary>
        /// 包含序号生成 从指定行开始
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="sheetname"></param>
        /// <param name="version"></param>
        /// <param name="ignoreExport"></param>
        /// <param name="sn">是否包含序号</param>
        /// <returns></returns>
        public static IWorkbook ExportToExcel<T>(List<T> list, int startRow, string _sheetname = "sheet", bool sn = true, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
        {
            Workbook(version);
            if (!list.Any())
            {
                workbook.CreateSheet(_sheetname);
                return workbook;
            }

            int count = list.Count;
            int sheetCount = 1;
            if (count >= Max)
            {
                sheetCount = GetSheetsCount(count);
            }
            for (int i = 0; i < sheetCount; i++)
            {
                if (i > 0)
                {
                    startRow = 0;
                }
                var sheetname = _sheetname + (i + 1);
                ISheet sheet = workbook.CreateSheet(sheetname);
                IRow row = sheet.CreateRow(startRow);
                Type entityType = list[0].GetType();
                PropertyInfo[] entityProperties = entityType.GetProperties();
                int index;
                int countLen;
                if (sn)
                {
                    index = 1;
                    countLen = entityProperties.Length + 1;
                    var cell = row.CreateCell(0);
                    cell.SetCellValue("序号");
                    ICellStyle style = workbook.CreateCellStyle();
                    style.FillForegroundColor = HSSFColor.Grey25Percent.Index;

                    XSSFFont ffont = (XSSFFont)workbook.CreateFont();
                    ffont.Color = HSSFColor.Black.Index;
                    ffont.IsBold = true;
                    style.SetFont(ffont);
                    style.FillPattern = FillPattern.SolidForeground;
                    style.BorderBottom = BorderStyle.Thin;//下边框为细线边框
                    style.BorderLeft = BorderStyle.Thin;//左边框
                    style.BorderRight = BorderStyle.Thin;//上边框
                    style.BorderTop = BorderStyle.Thin;//右边框

                    cell.CellStyle = style;
                }
                else
                {
                    index = 0;
                    countLen = entityProperties.Length;
                }
                for (int j = index; j < countLen; j++)
                {
                    if (ignoreExport != null && ignoreExport.Contains(entityProperties[sn ? j - 1 : j].Name))
                    {
                        continue;
                    }
                    ICell cell = row.CreateCell(j);
                    cell.CellStyle = cellStyle;
                    var objAttr = entityProperties[sn ? j - 1 : j].GetCustomAttributes(typeof(ExcelDescriptionAttribute), true);
                    if (!(objAttr.FirstOrDefault() is ExcelDescriptionAttribute model))
                    {
                        cell.SetCellValue(entityProperties[sn ? j - 1 : j].Name);
                    }
                    else
                    {
                        cell.SetCellValue(model.Name);
                        if (!string.IsNullOrEmpty(model.Comment))
                        {
                            var patr = sheet.CreateDrawingPatriarch();
                            XSSFComment comment1 = (XSSFComment)patr.CreateCellComment(new XSSFClientAnchor(0, 0, 0, 0, j, 1, j + 1, 1));
                            comment1.String = new XSSFRichTextString(model.Comment);
                            cell.CellComment = comment1;
                        }

                        ICellStyle style = workbook.CreateCellStyle();

                        if (model.BackColor == 0)
                        {
                            style.FillForegroundColor = HSSFColor.Grey25Percent.Index;
                        }
                        else
                        {
                            style.FillForegroundColor = model.BackColor;
                        }

                        //XSSFFont ffont = (XSSFFont)workbook.CreateFont();
                        if (model.FrontColor == 0)
                        {
                            //ffont.Color = HSSFColor.White.Index;
                            style.GetFont(workbook).Color = HSSFColor.Black.Index;
                        }
                        else
                        {
                            //ffont.Color = model.FrontColor;
                            style.GetFont(workbook).Color = model.FrontColor;
                        }
                        style.GetFont(workbook).IsBold = true;

                        //style.SetFont(ffont);
                        //style.FillPattern = FillPattern.SolidForeground;
                        style.FillPattern = FillPattern.SolidForeground;

                        style.FillPattern = FillPattern.SolidForeground;
                        style.BorderBottom = BorderStyle.Thin;//下边框为细线边框
                        style.BorderLeft = BorderStyle.Thin;//左边框
                        style.BorderRight = BorderStyle.Thin;//上边框
                        style.BorderTop = BorderStyle.Thin;//右边框
                        cell.CellStyle = style;
                    }
                }

                var maxList = list.Skip(Max * i).Take(Max).ToList();
                //if (i == sheetCount - 1)
                //{
                //    maxList.Take((count - i * Max)).ToList();
                //}
                //else
                //{
                //    maxList.Take(Max).ToList();
                //}
                //数据

                for (int k = 0; k < maxList.Count; k++)
                {
                    IRow row1 = sheet.CreateRow(k + 1 + startRow);
                    var properties = maxList[k].GetType().GetProperties();
                    if (sn)
                    {
                        index = 1;
                        countLen = properties.Length + 1;
                        ICell cell1 = row1.CreateCell(0);
                        cell1.CellStyle = cellStyle;
                        cell1.SetCellValue(k + 1);
                    }
                    else
                    {
                        index = 0;
                        countLen = properties.Length;
                    }
                    for (int m = index; m < countLen; m++)
                    {
                        if (ignoreExport != null && ignoreExport.Contains(properties[sn ? m - 1 : m].Name))
                        {
                            continue;
                        }
                        ICell cell = row1.CreateCell(m);
                        cell.CellStyle = cellStyle;
                        var id = properties[sn ? m - 1 : m].GetValue(maxList[k])?.ToString();
                        cell.SetCellValue(id);
                    }
                }
                AutoSizeColumns(sheet, startRow);
            }
            return workbook;
        }
        /// <summary>
        /// 报表平台 普通报表
        /// </summary>
        /// <param name="fileds"></param>
        /// <param name="list"></param>
        /// <param name="startRow"></param>
        /// <param name="_sheetname"></param>
        /// <param name="sn"></param>
        /// <param name="version"></param>
        /// <param name="ignoreExport"></param>
        /// <returns></returns>
        public static IWorkbook ExportToExcelList(List<FiledInfo> fileds, List<dynamic> list, int startRow, string _sheetname = "sheet", bool sn = true, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
        {
            Workbook(version);
            if (!list.Any())
            {
                workbook.CreateSheet("sheet1");
                return workbook;
            }
            int count = list.Count;

            int sheetCount = 1;
            if (count >= Max)
            {
                sheetCount = GetSheetsCount(count);
            }
            for (int i = 0; i < sheetCount; i++)
            {
                if (i > 0)
                {
                    startRow = 0;
                }
                var sheetname = _sheetname + (i + 1);
                ISheet sheet = workbook.CreateSheet(sheetname);
                IRow row = sheet.CreateRow(startRow);
                int index;
                int countLen;
                if (sn)
                {
                    index = 1;
                    countLen = fileds.Count + 1;
                    var cell = row.CreateCell(0);
                    cell.SetCellValue("序号");
                    ICellStyle style = workbook.CreateCellStyle();
                    style.FillForegroundColor = HSSFColor.Grey25Percent.Index;

                    XSSFFont ffont = (XSSFFont)workbook.CreateFont();
                    ffont.Color = HSSFColor.Black.Index;
                    ffont.IsBold = true;
                    style.SetFont(ffont);
                    style.FillPattern = FillPattern.SolidForeground;
                    style.BorderBottom = BorderStyle.Thin;//下边框为细线边框
                    style.BorderLeft = BorderStyle.Thin;//左边框
                    style.BorderRight = BorderStyle.Thin;//上边框
                    style.BorderTop = BorderStyle.Thin;//右边框

                    cell.CellStyle = style;
                }
                else
                {
                    index = 0;
                    countLen = fileds.Count;
                }
                for (int j = index; j < countLen; j++)
                {
                    ICell cell = row.CreateCell(j);
                    var name = fileds[j - 1].Name;
                    cell.SetCellValue(name);

                    ICellStyle style = workbook.CreateCellStyle();

                    style.FillForegroundColor = HSSFColor.Grey25Percent.Index;

                    //XSSFFont ffont = (XSSFFont)workbook.CreateFont();
                    style.GetFont(workbook).Color = HSSFColor.Black.Index;
                    style.GetFont(workbook).IsBold = true;

                    //style.SetFont(ffont);
                    style.FillPattern = FillPattern.SolidForeground;

                    style.FillPattern = FillPattern.SolidForeground;
                    style.BorderBottom = BorderStyle.Thin;//下边框为细线边框
                    style.BorderLeft = BorderStyle.Thin;//左边框
                    style.BorderRight = BorderStyle.Thin;//上边框
                    style.BorderTop = BorderStyle.Thin;//右边框
                    cell.CellStyle = style;
                }
                //数据

                for (int k = 0; k < list.Count; k++)
                {
                    IRow row1 = sheet.CreateRow(k + 1 + startRow);
                    if (sn)
                    {
                        index = 1;
                        countLen = fileds.Count + 1;
                        ICell cell1 = row1.CreateCell(0);
                        cell1.CellStyle = cellStyle;
                        cell1.SetCellValue(k + 1);
                    }
                    else
                    {
                        index = 0;
                        countLen = fileds.Count;
                    }
                    IDictionary<string, object> dic = list[k] as IDictionary<string, object>;
                    for (int m = index; m < countLen; m++)
                    {
                        ICell cell = row1.CreateCell(m);
                        cell.CellStyle = cellStyle;
                        var name = fileds[m - 1].Key;
                        var rule = fileds[m - 1].Rule;
                        if (rule == "" || rule == null)
                        {
                            rule = "yyyy-MM-dd hh:mm:ss";
                        }
                        var key = dic.TryGetValue(name, out object value);
                        if (key && value != null && value.ToString() != "")
                        {
                            DateTime dtDate;
                            var isTime = DateTime.TryParse(value.ToString(), out dtDate);
                            cell.SetCellValue(isTime ? dtDate.ToString(rule) : value.ToString());
                        }
                        else
                        {
                            cell.SetCellValue("");
                        }
                    }
                }
                AutoSizeColumns(sheet, startRow);
            }
            return workbook;
        }


        public static IWorkbook ExportToDataTable(List<FiledInfo> fileds, DataTable dt, int startRow, string _sheetname = "sheet", bool sn = true, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
        {
            Workbook(version);
            if (dt.Rows.Count <= 0)
            {
                workbook.CreateSheet("sheet1");
                return workbook;
            }
            int count = dt.Rows.Count;

            int sheetCount = 1;
            if (count >= Max)
            {
                sheetCount = GetSheetsCount(count);
            }
            for (int i = 0; i < sheetCount; i++)
            {
                if (i > 0)
                {
                    startRow = 0;
                }
                var sheetname = _sheetname + (i + 1);
                ISheet sheet = workbook.CreateSheet(sheetname);
                IRow row = sheet.CreateRow(startRow);
                int index;
                int countLen;
                if (sn)
                {
                    index = 1;
                    countLen = fileds.Count + 1;
                    var cell = row.CreateCell(0);
                    cell.SetCellValue("序号");
                    ICellStyle style = workbook.CreateCellStyle();
                    style.FillForegroundColor = HSSFColor.Grey25Percent.Index;

                    XSSFFont ffont = (XSSFFont)workbook.CreateFont();
                    ffont.Color = HSSFColor.Black.Index;
                    ffont.IsBold = true;
                    style.SetFont(ffont);
                    style.FillPattern = FillPattern.SolidForeground;
                    style.BorderBottom = BorderStyle.Thin;//下边框为细线边框
                    style.BorderLeft = BorderStyle.Thin;//左边框
                    style.BorderRight = BorderStyle.Thin;//上边框
                    style.BorderTop = BorderStyle.Thin;//右边框

                    cell.CellStyle = style;
                }
                else
                {
                    index = 0;
                    countLen = fileds.Count;
                }
                //除序号外的列名
                for (int j = index; j < countLen; j++)
                {
                    ICell cell = row.CreateCell(j);
                    var name = fileds[j - 1].Name;
                    cell.SetCellValue(name);

                    ICellStyle style = workbook.CreateCellStyle();

                    style.FillForegroundColor = HSSFColor.Grey25Percent.Index;

                    style.GetFont(workbook).Color = HSSFColor.Black.Index;
                    style.GetFont(workbook).IsBold = true;

                    style.FillPattern = FillPattern.SolidForeground;

                    style.FillPattern = FillPattern.SolidForeground;
                    style.BorderBottom = BorderStyle.Thin;//下边框为细线边框
                    style.BorderLeft = BorderStyle.Thin;//左边框
                    style.BorderRight = BorderStyle.Thin;//上边框
                    style.BorderTop = BorderStyle.Thin;//右边框
                    cell.CellStyle = style;
                }

                //填充数据
                //数据
                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    IRow row1 = sheet.CreateRow(m + 1 + startRow);
                    if (sn)
                    {
                        index = 1;
                        countLen = fileds.Count + 1;
                        ICell cell1 = row1.CreateCell(0);
                        cell1.CellStyle = cellStyle;
                        cell1.SetCellValue(m + 1);
                    }
                    else
                    {
                        index = 0;
                        countLen = fileds.Count;
                    }

                    for (int p = index; p < countLen; p++)
                    {
                        var filedsName = fileds[p - 1].Key;
                        ICell cell = row1.CreateCell(p);
                        cell.CellStyle = cellStyle;
                        cell.SetCellValue(dt.Rows[m][filedsName]?.ToString());
                    }
                }
                AutoSizeColumns(sheet, startRow);
            }
            return workbook;
        }
        /// <summary>
        /// 设备点检
        /// </summary>
        /// <param name="checkMonth"></param>
        /// <param name="list"></param>
        /// <param name="startRow"></param>
        /// <param name="_sheetname"></param>
        /// <param name="sn"></param>
        /// <param name="version"></param>
        /// <param name="ignoreExport"></param>
        /// <returns></returns>
        public static IWorkbook ExportToExcel(string checkMonth, ResultQueryListData list, int startRow, string _sheetname = "sheet", bool sn = true, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
        {
            var arr = checkMonth.Split("-");
            int days = DateTime.DaysInMonth(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));//总共多少天
            IWorkbook workbook = new XSSFWorkbook();
            if (list.ResultItem.Count <= 0)
            {
                workbook.CreateSheet("sheet");
                return workbook;
            }
            List<ShiftInfoClass> sc = list.ShiftInfo;
            List<ResultItemListClass> ri = list.ResultItem;
            List<ResultUserClass> ru = list.ResultUser;
            int count = ri.Count;
            int countLen = sc.Count > 0 ? 3 + sc.Count * days : 3 + days;//获取列数
            var sheetname = _sheetname + "1";
            ISheet sheet = workbook.CreateSheet(sheetname);

            #region 单元格样式
            //标题样式
            XSSFFont ffont = (XSSFFont)workbook.CreateFont();
            ffont.Color = HSSFColor.Black.Index;
            ffont.IsBold = true;
            ICellStyle style = workbook.CreateCellStyle();
            style.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            style.GetFont(workbook).Color = HSSFColor.Black.Index;
            style.GetFont(workbook).IsBold = true;
            style.SetFont(ffont);
            style.FillPattern = FillPattern.SolidForeground;
            style.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style.BorderLeft = BorderStyle.Thin;//左边框
            style.BorderRight = BorderStyle.Thin;//上边框
            style.BorderTop = BorderStyle.Thin;//右边框
            style.VerticalAlignment = VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Center;

            //点检项目 单元格样式
            XSSFFont ffontU = (XSSFFont)workbook.CreateFont();
            ffontU.FontName = "宋体";
            ICellStyle cellStyleU = workbook.CreateCellStyle();
            cellStyleU.GetFont(workbook).Color = HSSFColor.Black.Index;
            cellStyleU.GetFont(workbook).IsBold = true;
            cellStyleU.SetFont(ffont);
            cellStyleU.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            cellStyleU.BorderLeft = BorderStyle.Thin;//左边框
            cellStyleU.BorderRight = BorderStyle.Thin;//上边框
            cellStyleU.BorderTop = BorderStyle.Thin;//右边框
            cellStyleU.VerticalAlignment = VerticalAlignment.Center;

            //判定标准 单元格样式
            XSSFFont ffontU1 = (XSSFFont)workbook.CreateFont();
            ffontU1.FontName = "宋体";
            ICellStyle cellStyleU1 = workbook.CreateCellStyle();
            cellStyleU1.GetFont(workbook).Color = HSSFColor.Black.Index;
            cellStyleU1.GetFont(workbook).IsBold = true;
            cellStyleU1.SetFont(ffont);
            cellStyleU1.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            cellStyleU1.BorderLeft = BorderStyle.Thin;//左边框
            cellStyleU1.BorderRight = BorderStyle.Thin;//上边框
            cellStyleU1.BorderTop = BorderStyle.Thin;//右边框
            cellStyleU1.VerticalAlignment = VerticalAlignment.Center;
            cellStyleU1.Alignment = HorizontalAlignment.Center;
            #endregion


            #region 标题配置
            List<string> str1 = new List<string>();
            List<string> str2 = new List<string>();
            List<string> str3 = new List<string>();
            str1.Add("序号"); str1.Add("点检项目"); str1.Add("判定标准");
            str2.Add(""); str2.Add(""); str2.Add("");
            str3.Add(""); str3.Add(""); str3.Add("");
            for (int i = 1; i <= days; i++)
            {
                if (sc.Count > 0)
                {
                    for (int j = 0; j < sc.Count; j++)
                    {
                        str1.Add("点检日期" + checkMonth);
                        str2.Add(i.ToString());
                        str3.Add(sc[j].ShiftName);
                    }
                }
                else
                {
                    str1.Add("点检日期" + checkMonth);
                    str2.Add(i.ToString());

                }
            }

            IRow row = sheet.CreateRow(0);//第一行
            row.Height = 30 * 20;
            for (int i = 0; i < str1.Count; i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(str1[i]);
                cell.CellStyle = style;
            }

            IRow row1 = sheet.CreateRow(1);//第二行
            row1.Height = 30 * 20;
            for (int i = 0; i < str2.Count; i++)
            {
                var cell = row1.CreateCell(i);
                if (i == 0)
                {
                    cell.Sheet.SetColumnWidth(0, 4 * 256);//每单元格默认0.95cm
                }
                cell.SetCellValue(str2[i]);
                cell.CellStyle = style;
            }
            if (sc.Count > 0)
            {
                IRow row2 = sheet.CreateRow(2);//第三行
                row2.Height = 30 * 20;
                for (int i = 0; i < str3.Count; i++)
                {
                    var cell = row2.CreateCell(i);
                    cell.SetCellValue(str3[i]);
                    cell.CellStyle = style;
                }
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 3, days * sc.Count + 2));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 0, 0));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 1, 1));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 2, 2));
                for (int i = 0; i < days; i++)
                {
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 3 + i * sc.Count, 2 + (i + 1) * sc.Count));
                }
            }
            else
            {
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 3, days + 2));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 1, 1));
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 2, 2));
            }
            #endregion

            #region 赋值点检项

            List<string> user = new List<string>();
            startRow = sc.Count > 0 ? 2 : 1;
            for (int k = 0; k < ri.Count; k++)
            {
                var rowValue = ri[k];
                var daValue = rowValue.DateList;
                IRow rowI = sheet.CreateRow(k + 1 + startRow);
                rowI.Height = 30 * 20;
                //序号赋值
                ICell cell1 = rowI.CreateCell(0);
                cell1.CellStyle = cellStyleU1;
                cell1.SetCellValue(k + 1);

                //点检项赋值
                ICell cell2 = rowI.CreateCell(1);
                cell2.CellStyle = cellStyleU1;
                cell2.SetCellValue(rowValue.CheckItem);

                //点检标准赋值
                ICell cell3 = rowI.CreateCell(2);
                cell3.CellStyle = cellStyleU;
                cell3.SetCellValue(rowValue.JudgeStandard);

                for (int i = 1; i <= days; i++)
                {
                    string dstr = i < 10 ? "0" + i.ToString() + "" : i.ToString();
                    var obj = daValue.Find(x => x.CheckDate == dstr);
                    List<CheckValueClass> vc = obj != null ? JsonConvert.DeserializeObject<List<CheckValueClass>>(obj.CheckValue) : new List<CheckValueClass>();
                    if (sc.Count > 0)//存在班次
                    {
                        for (int y = 0; y < sc.Count; y++)
                        {
                            int ceN = (i - 1) * sc.Count + y + 3;
                            ICell cell = rowI.CreateCell(ceN);
                            var sj = vc.Find(x => x.ShiftCode == sc[y].ShiftCode);
                            cell.CellStyle = cellStyleU1;
                            cell.Sheet.SetColumnWidth(ceN, 5 * 256);//每单元格默认0.95cm
                            cell.SetCellValue(sj != null ? sj.Value : "");
                            string userName = sj != null ? sj.User : "";
                            if (user.Count > ceN - 3)
                            {
                                userName = !string.IsNullOrWhiteSpace(userName) ? userName : user[ceN - 3];
                                if (user[ceN - 3] == "")
                                {
                                    user[ceN - 3] = userName;
                                }
                            }
                            else
                            {
                                user.Add(userName);
                            }
                        }
                    }
                    else
                    {
                        ICell cell = rowI.CreateCell(i + 2);
                        cell.CellStyle = cellStyleU1;
                        cell.Sheet.SetColumnWidth(i + 2, 4 * 256);//每单元格默认0.95cm
                        cell.SetCellValue(vc.Count > 0 ? vc[0].Value : "");
                        string userName = vc.Count > 0 ? vc[0].User : "";
                        if (user.Count > i - 1)
                        {
                            userName = !string.IsNullOrWhiteSpace(userName) ? userName : user[i - 1];
                            if (user[i - 1] == "")
                            {
                                user[i - 1] = userName;
                            }
                        }
                        else
                        {
                            user.Add(userName);
                        }

                    }

                }
            }
            #endregion

            #region 用户赋值
            IRow rowL = sheet.CreateRow(startRow + ri.Count + 1);
            rowL.Height = 30 * 20;
            user.Insert(0, ""); user.Insert(0, ""); user.Insert(0, "");
            for (int i = 0; i < user.Count; i++)
            {
                ICell cell = rowL.CreateCell(i);
                cell.CellStyle = cellStyleU1;//i > 2 ? cellStyleU : style;
                //cellStyleU1.Rotation= (short)-90;//-90~90
                cell.SetCellValue(i > 2 ? user[i] : "点检人");
                if (user[i] != "")
                {
                    sheet.AutoSizeColumn(i);//有值的需要自适应，没值的默认
                }

            }
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow + ri.Count + 1, startRow + ri.Count + 1, 0, 2));
            #endregion
            //AutoSizeColumns(sheet, startRow);
            sheet.AutoSizeColumn(1);//点检项自适应宽度
            sheet.AutoSizeColumn(2);//点检标准自适应宽度
            return workbook;
        }

        /// <summary>
        /// 出厂报告
        /// </summary>
        /// <param name="list"></param>
        /// <param name="startRow"></param>
        /// <param name="_sheetname"></param>
        /// <param name="sn"></param>
        /// <param name="version"></param>
        /// <param name="ignoreExport"></param>
        /// <returns></returns>
        public static IWorkbook ExportToExcelReport(dynamic list, int startRow, string _sheetname = "sheet", bool sn = true, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
        {
            IWorkbook workbook = new XSSFWorkbook();
            //workbook.CreateSheet(_sheetname);
            int count = list.Count;

            #region 第一行大标题样式
            XSSFFont ffont = (XSSFFont)workbook.CreateFont();
            ffont.Color = HSSFColor.Black.Index;
            ffont.IsBold = true;
            ffont.FontHeightInPoints = 20;
            ffont.FontName = "等线";
            ICellStyle style = workbook.CreateCellStyle();
            style.GetFont(workbook).Color = HSSFColor.Black.Index;
            style.GetFont(workbook).IsBold = true;
            style.SetFont(ffont);
            style.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style.BorderLeft = BorderStyle.Thin;//左边框
            style.BorderRight = BorderStyle.Thin;//上边框
            style.BorderTop = BorderStyle.Thin;//右边框
            style.VerticalAlignment = VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Center;
            #endregion

            #region 表格主题标题斜体样式
            XSSFFont font0 = (XSSFFont)workbook.CreateFont();
            font0.Color = HSSFColor.Black.Index;
            font0.IsBold = true;
            font0.FontHeightInPoints = 14;
            font0.FontName = "等线";
            font0.IsItalic = true;
            ICellStyle style0 = workbook.CreateCellStyle();
            style0.GetFont(workbook).Color = HSSFColor.Black.Index;
            style0.GetFont(workbook).IsBold = true;
            style0.SetFont(font0);
            style0.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style0.BorderLeft = BorderStyle.Thin;//左边框
            style0.BorderRight = BorderStyle.Thin;//上边框
            style0.BorderTop = BorderStyle.Thin;//右边框
            style0.VerticalAlignment = VerticalAlignment.Center;
            style0.Alignment = HorizontalAlignment.Center;

            XSSFFont font0_0 = (XSSFFont)workbook.CreateFont();
            font0_0.Color = HSSFColor.Black.Index;
            font0_0.IsBold = true;
            font0_0.FontHeightInPoints = 14;
            font0_0.FontName = "等线";
            ICellStyle style0_0 = workbook.CreateCellStyle();
            style0_0.GetFont(workbook).Color = HSSFColor.Black.Index;
            style0_0.GetFont(workbook).IsBold = true;
            style0_0.SetFont(font0_0);
            style0_0.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style0_0.BorderLeft = BorderStyle.Thin;//左边框
            style0_0.BorderRight = BorderStyle.Thin;//上边框
            style0_0.BorderTop = BorderStyle.Thin;//右边框
            style0_0.VerticalAlignment = VerticalAlignment.Center;
            style0_0.Alignment = HorizontalAlignment.Center;
            #endregion

            #region  单元格内容样式
            XSSFFont font1 = (XSSFFont)workbook.CreateFont();
            font1.FontName = "等线";
            font1.Boldweight = (short)FontBoldWeight.Normal;
            font1.FontHeightInPoints = 11;
            ICellStyle style1 = workbook.CreateCellStyle();
            style1.GetFont(workbook).Color = HSSFColor.Black.Index;
            style1.GetFont(workbook).IsBold = true;
            style1.SetFont(font1);
            style1.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style1.BorderLeft = BorderStyle.Thin;//左边框
            style1.BorderRight = BorderStyle.Thin;//上边框
            style1.BorderTop = BorderStyle.Thin;//右边框
            style1.VerticalAlignment = VerticalAlignment.Center;
            style1.Alignment = HorizontalAlignment.Center;
            style1.WrapText = true;

            //单元格样式
            XSSFFont font1_OK = (XSSFFont)workbook.CreateFont();
            font1_OK.FontName = "等线";
            font1_OK.Boldweight = (short)FontBoldWeight.Normal;
            font1_OK.Color = HSSFColor.Green.Index;
            font1_OK.FontHeightInPoints = 11;
            ICellStyle style1_OK = workbook.CreateCellStyle();
            style1_OK.GetFont(workbook).Color = HSSFColor.Black.Index;
            style1_OK.SetFont(font1_OK);
            style1_OK.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style1_OK.BorderLeft = BorderStyle.Thin;//左边框
            style1_OK.BorderRight = BorderStyle.Thin;//上边框
            style1_OK.BorderTop = BorderStyle.Thin;//右边框
            style1_OK.VerticalAlignment = VerticalAlignment.Center;
            style1_OK.Alignment = HorizontalAlignment.Center;
            style1_OK.WrapText = true;

            //单元格样式
            XSSFFont font1_NG = (XSSFFont)workbook.CreateFont();
            font1_NG.FontName = "等线";
            font1_NG.Boldweight = (short)FontBoldWeight.Normal;
            font1_NG.Color = HSSFColor.Red.Index;
            font1_NG.FontHeightInPoints = 11;
            ICellStyle style1_NG = workbook.CreateCellStyle();
            style1_NG.GetFont(workbook).Color = HSSFColor.Black.Index;
            style1_NG.SetFont(font1_NG);
            style1_NG.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style1_NG.BorderLeft = BorderStyle.Thin;//左边框
            style1_NG.BorderRight = BorderStyle.Thin;//上边框
            style1_NG.BorderTop = BorderStyle.Thin;//右边框
            style1_NG.VerticalAlignment = VerticalAlignment.Center;
            style1_NG.Alignment = HorizontalAlignment.Center;
            style1_NG.WrapText = true;

            //带有删除线单元格样式
            XSSFFont font2 = (XSSFFont)workbook.CreateFont();
            font2.FontName = "等线";
            font2.Boldweight = (short)FontBoldWeight.Normal;
            font2.IsStrikeout = true;
            font2.FontHeightInPoints = 11;
            ICellStyle style2 = workbook.CreateCellStyle();
            style2.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
            style2.FillPattern = FillPattern.SolidForeground;
            style2.GetFont(workbook).Color = HSSFColor.Black.Index;
            style2.SetFont(font2);
            style2.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style2.BorderLeft = BorderStyle.Thin;//左边框
            style2.BorderRight = BorderStyle.Thin;//上边框
            style2.BorderTop = BorderStyle.Thin;//右边框
            style2.VerticalAlignment = VerticalAlignment.Center;
            style2.Alignment = HorizontalAlignment.Center;
            style2.WrapText = true;

            //带有删除线单元格样式
            XSSFFont font2_OK = (XSSFFont)workbook.CreateFont();
            font2_OK.FontName = "等线";
            font2_OK.Boldweight = (short)FontBoldWeight.Normal;
            font2_OK.IsStrikeout = true;
            font2_OK.Color = HSSFColor.Green.Index;
            font2_OK.FontHeightInPoints = 11;
            ICellStyle style2_OK = workbook.CreateCellStyle();
            style2_OK.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
            style2_OK.FillPattern = FillPattern.SolidForeground;
            style2_OK.GetFont(workbook).IsBold = true;
            style2_OK.SetFont(font2_OK);
            style2_OK.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style2_OK.BorderLeft = BorderStyle.Thin;//左边框
            style2_OK.BorderRight = BorderStyle.Thin;//上边框
            style2_OK.BorderTop = BorderStyle.Thin;//右边框
            style2_OK.VerticalAlignment = VerticalAlignment.Center;
            style2_OK.Alignment = HorizontalAlignment.Center;
            style2_OK.WrapText = true;

            //带有删除线单元格样式
            XSSFFont font2_NG = (XSSFFont)workbook.CreateFont();
            font2_NG.FontName = "等线";
            font2_NG.Boldweight = (short)FontBoldWeight.Normal;
            font2_NG.IsStrikeout = true;
            font2_NG.Color = HSSFColor.Red.Index;
            font2_NG.FontHeightInPoints = 11;
            ICellStyle style2_NG = workbook.CreateCellStyle();
            style2_NG.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
            style2_NG.FillPattern = FillPattern.SolidForeground;
            style2_NG.GetFont(workbook).Color = HSSFColor.Black.Index;
            style2_NG.SetFont(font2_NG);
            style2_NG.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style2_NG.BorderLeft = BorderStyle.Thin;//左边框
            style2_NG.BorderRight = BorderStyle.Thin;//上边框
            style2_NG.BorderTop = BorderStyle.Thin;//右边框
            style2_NG.VerticalAlignment = VerticalAlignment.Center;
            style2_NG.Alignment = HorizontalAlignment.Center;
            style2_NG.WrapText = true;
            #endregion

            #region 单元格小标题样式
            XSSFFont font1_title = (XSSFFont)workbook.CreateFont();
            font1_title.FontName = "等线";
            font1_title.Boldweight = (short)FontBoldWeight.Normal;
            font1_title.FontHeightInPoints = 12;
            ICellStyle style1_title = workbook.CreateCellStyle();
            style1_title.GetFont(workbook).Color = HSSFColor.Black.Index;
            style1_title.GetFont(workbook).IsBold = true;
            style1_title.SetFont(font1_title);
            style1_title.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style1_title.BorderLeft = BorderStyle.Thin;//左边框
            style1_title.BorderRight = BorderStyle.Thin;//上边框
            style1_title.BorderTop = BorderStyle.Thin;//右边框
            style1_title.VerticalAlignment = VerticalAlignment.Center;
            style1_title.Alignment = HorizontalAlignment.Center;
            style1_title.WrapText = true;

            XSSFFont font1_title_OK = (XSSFFont)workbook.CreateFont();
            font1_title_OK.FontName = "等线";
            //font1_title_OK.Boldweight = (short)FontBoldWeight.Normal;
            font1_title_OK.Color = HSSFColor.Green.Index;
            font1_title_OK.IsBold = true;
            font1_title_OK.FontHeightInPoints = 14;
            ICellStyle style1_title_OK = workbook.CreateCellStyle();
            style1_title_OK.GetFont(workbook).Color = HSSFColor.Black.Index;
            style1_title_OK.GetFont(workbook).IsBold = true;
            style1_title_OK.SetFont(font1_title_OK);
            style1_title_OK.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style1_title_OK.BorderLeft = BorderStyle.Thin;//左边框
            style1_title_OK.BorderRight = BorderStyle.Thin;//上边框
            style1_title_OK.BorderTop = BorderStyle.Thin;//右边框
            style1_title_OK.VerticalAlignment = VerticalAlignment.Center;
            style1_title_OK.Alignment = HorizontalAlignment.Center;
            style1_title_OK.WrapText = true;

            XSSFFont font1_title_NG = (XSSFFont)workbook.CreateFont();
            font1_title_NG.FontName = "等线";
            //font1_title_NG.Boldweight = (short)FontBoldWeight.Normal;
            font1_title_NG.IsBold = true;
            font1_title_NG.Color = HSSFColor.Red.Index;
            font1_title_NG.FontHeightInPoints = 14;
            ICellStyle style1_title_NG = workbook.CreateCellStyle();
            style1_title_NG.GetFont(workbook).Color = HSSFColor.Black.Index;
            style1_title_NG.GetFont(workbook).IsBold = true;
            style1_title_NG.SetFont(font1_title_NG);
            style1_title_NG.BorderBottom = BorderStyle.Thin;//下边框为细线边框
            style1_title_NG.BorderLeft = BorderStyle.Thin;//左边框
            style1_title_NG.BorderRight = BorderStyle.Thin;//上边框
            style1_title_NG.BorderTop = BorderStyle.Thin;//右边框
            style1_title_NG.VerticalAlignment = VerticalAlignment.Center;
            style1_title_NG.Alignment = HorizontalAlignment.Center;
            style1_title_NG.WrapText = true;
            #endregion

            for (int j = 0; j < count; j++)
            {
                startRow = 0;
                var sheetname = sheetName(list[j].ComponentSn, j.ToString());
                int lengs = sheetname.Length;
                ISheet sheet = workbook.CreateSheet(sheetname);
                #region 第一行大标题配置
                IRow row0 = sheet.CreateRow(startRow);
                row0.Height = 47 * 20;
                for (int i = 0; i < 7; i++)
                {
                    var cell = row0.CreateCell(i);
                    var name = list[j].EngineType + "-" + list[j].ProductCode;
                    cell.SetCellValue(i == 0 ? name + "产品报告" : "");
                    cell.CellStyle = style;
                }
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow, startRow, 0, 6));
                #endregion

                #region 第二行
                startRow = startRow + 1;
                IRow row1 = sheet.CreateRow(startRow);
                row1.Height = 32 * 20;
                row1.CreateCell(0); row1.GetCell(0).SetCellValue("成品SN"); row1.GetCell(0).CellStyle = style1_title;
                row1.CreateCell(1); row1.GetCell(1).SetCellValue(list[j].ComponentSn); row1.GetCell(1).CellStyle = style1;
                row1.CreateCell(2); row1.GetCell(2).SetCellValue(""); row1.GetCell(2).CellStyle = style1;
                row1.CreateCell(3); row1.GetCell(3).SetCellValue("上线时间"); row1.GetCell(3).CellStyle = style1_title;
                row1.CreateCell(4); row1.GetCell(4).SetCellValue(list[j].OnlineTime?.ToString() ?? ""); row1.GetCell(4).CellStyle = style1;
                row1.CreateCell(5); row1.GetCell(5).SetCellValue("完整性"); row1.GetCell(5).CellStyle = style1_title;
                row1.CreateCell(6); row1.GetCell(6).SetCellValue(list[j].VerifyState); row1.GetCell(6).CellStyle = list[j].VerifyState == "YES" ? style1_title_OK : style1_title_NG;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow, startRow, 1, 2));
                #endregion

                #region 第三行
                startRow = startRow + 1;
                IRow row2 = sheet.CreateRow(startRow);
                row2.Height = 32 * 20;
                row2.CreateCell(0); row2.GetCell(0).SetCellValue("产品型号"); row2.GetCell(0).CellStyle = style1_title;
                row2.CreateCell(1); row2.GetCell(1).SetCellValue(list[j].EngineType); row2.GetCell(1).CellStyle = style1;
                row2.CreateCell(2); row2.GetCell(2).SetCellValue(""); row2.GetCell(2).CellStyle = style1;
                row2.CreateCell(3); row2.GetCell(3).SetCellValue("下线时间"); row2.GetCell(3).CellStyle = style1_title;
                row2.CreateCell(4); row2.GetCell(4).SetCellValue(list[j].OfflineTime?.ToString() ?? ""); row2.GetCell(4).CellStyle = style1;
                row2.CreateCell(5); row2.GetCell(5).SetCellValue("产品状态"); row2.GetCell(5).CellStyle = style1_title;
                row2.CreateCell(6); row2.GetCell(6).SetCellValue(list[j].ProductState == "1" ? "OK" : list[j].ProductState == "0" || list[j].ProductState == "2" ? "NG" : "");
                row2.GetCell(6).CellStyle = list[j].ProductState == "1" ? style1_title_OK : list[j].ProductState == "0" || list[j].ProductState == "2" ? style1_title_NG : style1_title;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow, startRow, 1, 2));
                #endregion

                if (list[j].OpNameList != null)
                {
                    for (int x = 0; x < list[j].OpNameList.Count; x++)
                    {
                        startRow = startRow + 1;//留一行空白行作为分割
                        var obj = list[j].OpNameList[x];
                        #region 工位第一行
                        startRow = startRow + 1;//留一行空白行作为分割
                        IRow rowP0 = sheet.CreateRow(startRow);
                        rowP0.Height = 32 * 20;
                        rowP0.CreateCell(0); rowP0.GetCell(0).SetCellValue(obj.OpName); rowP0.GetCell(0).CellStyle = style0_0;
                        rowP0.CreateCell(1); rowP0.GetCell(1).SetCellValue(obj.OpDesc); rowP0.GetCell(1).CellStyle = style1_title;
                        rowP0.CreateCell(2); rowP0.GetCell(2).SetCellValue(""); rowP0.GetCell(2).CellStyle = style1_title;
                        rowP0.CreateCell(3); rowP0.GetCell(3).SetCellValue("进站时间"); rowP0.GetCell(3).CellStyle = style1_title;
                        rowP0.CreateCell(4); rowP0.GetCell(4).SetCellValue(obj.StartTime?.ToString() ?? ""); rowP0.GetCell(4).CellStyle = style1_title;
                        rowP0.CreateCell(5); rowP0.GetCell(5).SetCellValue("出站时间"); rowP0.GetCell(5).CellStyle = style1_title;
                        rowP0.CreateCell(6); rowP0.GetCell(6).SetCellValue(obj.EndTime?.ToString() ?? ""); rowP0.GetCell(6).CellStyle = style1_title;
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow, startRow, 1, 2));
                        #endregion


                        #region 工位产品数据行
                        startRow = startRow + 1;
                        IRow rowP1 = sheet.CreateRow(startRow);
                        rowP1.Height = 32 * 20;
                        rowP1.CreateCell(0); rowP1.GetCell(0).SetCellValue("【产品数据】"); rowP1.GetCell(0).CellStyle = style0;
                        rowP1.CreateCell(1); rowP1.GetCell(1).SetCellValue(""); rowP1.GetCell(1).CellStyle = style1;
                        rowP1.CreateCell(2); rowP1.GetCell(2).SetCellValue(""); rowP1.GetCell(2).CellStyle = style1;
                        rowP1.CreateCell(3); rowP1.GetCell(3).SetCellValue(""); rowP1.GetCell(3).CellStyle = style1;
                        rowP1.CreateCell(4); rowP1.GetCell(4).SetCellValue(""); rowP1.GetCell(4).CellStyle = style1;
                        rowP1.CreateCell(5); rowP1.GetCell(5).SetCellValue(""); rowP1.GetCell(5).CellStyle = style1;
                        rowP1.CreateCell(6); rowP1.GetCell(6).SetCellValue(obj.OpNameState == "" || obj.OpNameState == null ? "" : obj.OpNameState == "1" ? "OK" : "NG");
                        rowP1.GetCell(6).CellStyle = obj.OpNameState == "" || obj.OpNameState == null ? style1 : obj.OpNameState == "1" ? style1_title_OK : style1_title_NG;
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow, startRow, 0, 5));

                        startRow = startRow + 1;
                        IRow rowP2 = sheet.CreateRow(startRow);
                        rowP2.Height = 32 * 20;
                        rowP2.CreateCell(0); rowP2.GetCell(0).SetCellValue("序号"); rowP2.GetCell(0).CellStyle = style1_title;
                        rowP2.CreateCell(1); rowP2.GetCell(1).SetCellValue("测量项"); rowP2.GetCell(1).CellStyle = style1_title;
                        rowP2.CreateCell(2); rowP2.GetCell(2).SetCellValue("测量值"); rowP2.GetCell(2).CellStyle = style1_title;
                        rowP2.CreateCell(3); rowP2.GetCell(3).SetCellValue("测量范围"); rowP2.GetCell(3).CellStyle = style1_title;
                        rowP2.CreateCell(4); rowP2.GetCell(4).SetCellValue("测量单位"); rowP2.GetCell(4).CellStyle = style1_title;
                        rowP2.CreateCell(5); rowP2.GetCell(5).SetCellValue("测量时间"); rowP2.GetCell(5).CellStyle = style1_title;
                        rowP2.CreateCell(6); rowP2.GetCell(6).SetCellValue("合格状态"); rowP2.GetCell(6).CellStyle = style1_title;

                        if (obj.QualityList != null)
                        {
                            for (int k = 0; k < obj.QualityList.Count; k++)
                            {
                                var pgObj = obj.QualityList[k];
                                int sindex = startRow + 1;
                                for (int z = 0; z < pgObj.QualityInfo.Count; z++)
                                {
                                    var pObj = pgObj.QualityInfo[z];
                                    startRow = startRow + 1;
                                    IRow row = sheet.CreateRow(startRow);
                                    row.Height = 29 * 20;
                                    row.CreateCell(0); row.GetCell(0).SetCellValue(k + 1); row.GetCell(0).CellStyle = pObj.State == "0" ? style1 : style2;
                                    row.CreateCell(1); row.GetCell(1).SetCellValue(pObj.MeasurePosition + "-" + pObj.MeasureItem); row.GetCell(1).CellStyle = pObj.State == "0" ? style1 : style2;
                                    row.CreateCell(2); row.GetCell(2).SetCellValue(pObj.MeasureValue); row.GetCell(2).CellStyle = pObj.State == "0" ? style1 : style2;
                                    row.CreateCell(3); row.GetCell(3).SetCellValue(pObj.LowerLimit + "-" + pObj.UpperLimit); row.GetCell(3).CellStyle = pObj.State == "0" ? style1 : style2;
                                    row.CreateCell(4); row.GetCell(4).SetCellValue(pObj.MeasureUom); row.GetCell(4).CellStyle = pObj.State == "0" ? style1 : style2;
                                    row.CreateCell(5); row.GetCell(5).SetCellValue(pObj.CreateTime?.ToString() ?? ""); row.GetCell(5).CellStyle = pObj.State == "0" ? style1 : style2;
                                    row.CreateCell(6); row.GetCell(6).SetCellValue(pObj.Status == "1" ? "OK" : "NG");
                                    if (pObj.State == "0")
                                    {
                                        row.GetCell(6).CellStyle = pObj.Status == "1" ? style1_OK : style1_NG;
                                    }
                                    else
                                    {
                                        row.GetCell(6).CellStyle = pObj.Status == "1" ? style2_OK : style2_NG;
                                    }
                                }
                                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(sindex, startRow, 0, 0));
                            }
                        }

                        #endregion

                        #region 工位物料数据行
                        startRow = startRow + 1;
                        IRow rowM1 = sheet.CreateRow(startRow);
                        rowM1.Height = 32 * 20;
                        rowM1.CreateCell(0); rowM1.GetCell(0).SetCellValue("【物料数据】"); rowM1.GetCell(0).CellStyle = style0;
                        rowM1.CreateCell(1); rowM1.GetCell(1).SetCellValue(""); rowM1.GetCell(1).CellStyle = style1;
                        rowM1.CreateCell(2); rowM1.GetCell(2).SetCellValue(""); rowM1.GetCell(2).CellStyle = style1;
                        rowM1.CreateCell(3); rowM1.GetCell(3).SetCellValue(""); rowM1.GetCell(3).CellStyle = style1;
                        rowM1.CreateCell(4); rowM1.GetCell(4).SetCellValue(""); rowM1.GetCell(4).CellStyle = style1;
                        rowM1.CreateCell(5); rowM1.GetCell(5).SetCellValue(""); rowM1.GetCell(5).CellStyle = style1;
                        rowM1.CreateCell(6); rowM1.GetCell(6).SetCellValue(""); rowM1.GetCell(6).CellStyle = style1;
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow, startRow, 0, 5));

                        startRow = startRow + 1;
                        IRow rowM2 = sheet.CreateRow(startRow);
                        rowM2.Height = 32 * 20;
                        rowM2.CreateCell(0); rowM2.GetCell(0).SetCellValue("序号"); rowM2.GetCell(0).CellStyle = style1_title;
                        rowM2.CreateCell(1); rowM2.GetCell(1).SetCellValue("物料名称"); rowM2.GetCell(1).CellStyle = style1_title;
                        rowM2.CreateCell(2); rowM2.GetCell(2).SetCellValue("物料号"); rowM2.GetCell(2).CellStyle = style1_title;
                        rowM2.CreateCell(3); rowM2.GetCell(3).SetCellValue("物料条码"); rowM2.GetCell(3).CellStyle = style1_title;
                        rowM2.CreateCell(4); rowM2.GetCell(4).SetCellValue(""); rowM2.GetCell(4).CellStyle = style1_title;
                        rowM2.CreateCell(5); rowM2.GetCell(5).SetCellValue("扫描时间"); rowM2.GetCell(5).CellStyle = style1_title;
                        rowM2.CreateCell(6); rowM2.GetCell(6).SetCellValue("批次码"); rowM2.GetCell(6).CellStyle = style1_title;
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow, startRow, 3, 4));

                        if (obj.MaterialList != null)
                        {
                            for (int k = 0; k < obj.MaterialList.Count; k++)
                            {
                                var mObj = obj.MaterialList[k];
                                startRow = startRow + 1;
                                IRow row = sheet.CreateRow(startRow);
                                row.Height = 29 * 20;
                                row.CreateCell(0); row.GetCell(0).SetCellValue(k + 1); row.GetCell(0).CellStyle = mObj.State == "0" ? style1 : style2;
                                row.CreateCell(1); row.GetCell(1).SetCellValue(mObj.MaterialDes); row.GetCell(1).CellStyle = mObj.State == "0" ? style1 : style2;
                                row.CreateCell(2); row.GetCell(2).SetCellValue(mObj.MaterialCode); row.GetCell(2).CellStyle = mObj.State == "0" ? style1 : style2;
                                row.CreateCell(3); row.GetCell(3).SetCellValue(mObj.MaterialBarcode); row.GetCell(3).CellStyle = mObj.State == "0" ? style1 : style2;
                                row.CreateCell(4); row.GetCell(4).SetCellValue(""); row.GetCell(4).CellStyle = mObj.State == "0" ? style1 : style2;
                                row.CreateCell(5); row.GetCell(5).SetCellValue(mObj.CreateTime?.ToString() ?? ""); row.GetCell(5).CellStyle = mObj.State == "0" ? style1 : style2;
                                row.CreateCell(6); row.GetCell(6).SetCellValue(mObj.MaterialLot); row.GetCell(6).CellStyle = mObj.State == "0" ? style1 : style2;
                                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(startRow, startRow, 3, 4));
                            }
                        }

                        #endregion
                    }
                }
                sheet.SetColumnWidth(0, 12 * 256); sheet.SetColumnWidth(1, 28 * 256); sheet.SetColumnWidth(2, 19 * 256); sheet.SetColumnWidth(3, 15 * 256);
                sheet.SetColumnWidth(4, 17 * 256); sheet.SetColumnWidth(5, 17 * 256); sheet.SetColumnWidth(6, 17 * 256);
                #region 打印默认设置
                sheet.PrintSetup.Landscape = true;//横向打印
                sheet.PrintSetup.PaperSize = (int)PaperSize.A4_Small;//A4打印
                sheet.HorizontallyCenter = true;//居中
                sheet.VerticallyCenter = true;//居中
                sheet.PrintSetup.Scale = 100;//无缩放
                sheet.SetMargin(MarginType.RightMargin, (double)1.4 / 3);
                sheet.SetMargin(MarginType.TopMargin, (double)2.0 / 3);
                sheet.SetMargin(MarginType.LeftMargin, (double)1.4 / 3);
                sheet.SetMargin(MarginType.BottomMargin, (double)2.0 / 3);
                #endregion
            }
            return workbook;
        }
        public static string sheetName(string str1, string str2)
        {
            string str = "";
            if (str1 != string.Empty && str1 != null)
            {
                str = str1;
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("/", "");
                str = str.Replace("?", "");
                str = str.Replace("*", "");
                str = str.Replace(@"\", "");
                str = str.Replace(":", "");
            }
            if (str2 != string.Empty && str2 != null)
            {
                if (str.Length > 31)
                {
                    str = str2 + "_" + str;
                }

            }
            return str;
        }
        /// <summary>
        /// 添加图片到excel
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="bytes"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static XSSFPicture AddPic2Excel(IWorkbook workbook, byte[] bytes, ImgPosition pos)
        {
            var sheet = workbook.GetSheetAt(0);
            var patriarch = sheet.CreateDrawingPatriarch();
            var picIdx = workbook.AddPicture(bytes, PictureType.PNG);
            var anchor = new XSSFClientAnchor(
               int.Parse(pos.X1),
                int.Parse(pos.Y1),
               int.Parse(pos.X2),
                int.Parse(pos.Y2),
               int.Parse(pos.Col1),
                int.Parse(pos.Row1),
               int.Parse(pos.Col2),
                int.Parse(pos.Row2))
            {
                AnchorType = AnchorType.MoveAndResize
            };
            return (XSSFPicture)patriarch.CreatePicture(anchor, picIdx);
        }

        /// <summary>
        /// 设置文档信息
        /// </summary>
        /// <returns></returns>
        private static DocumentSummaryInformation SetDocumentSummaryInformation()
        {
            //文档摘要信息
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "macroinf";
            return dsi;
        }

        /// <summary>
        /// 设置概要信息
        /// </summary>
        /// <returns></returns>
        private static SummaryInformation SetSummaryInformation()
        {
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Author = "MF"; //填加xls文件作者信息
            si.ApplicationName = "MF-WEBAPI"; //填加xls文件创建程序信息
            si.LastAuthor = "MF"; //填加xls文件最后保存者信息
            si.Comments = "macroinf-develop"; //填加xls文件作者信息
            si.Title = ""; //填加xls文件标题信息
            si.Subject = "product name";//填加文件主题信息
            si.CreateDateTime = DateTime.Now;
            return si;
        }

        /// <summary>
        /// 自动设置列宽
        /// </summary>
        /// <param name="sheet"></param>
        private static void AutoSizeColumns(ISheet sheet, int startRow = 0)
        {
            if (sheet.PhysicalNumberOfRows > 0)
            {
                IRow headerRow = sheet.GetRow(startRow);
                for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }
        }

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

        #endregion NPOI excel Export

        #region NPOI excel import

        private static object GetValueType(ICell cell)
        {
            if (cell is null)
            {
                return null;
            }
            CellType cellType = cell.CellType;
            if (cell.CellType == CellType.Formula)
            {
                cellType = cell.CachedFormulaResultType;
            }
            switch (cellType)
            {
                case CellType.Blank: //BLANK:
                case CellType.Unknown:
                    return null;

                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;

                case CellType.Numeric: //NUMERIC:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    return cell.NumericCellValue;

                case CellType.String: //STRING:
                    return cell.StringCellValue;

                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;

                case CellType.Formula: //FORMULA:

                default:
                    return cell.CellFormula;
            }
        }

        public static DataTable Import(string filepath)
        {
            string fileExt = Path.GetExtension(filepath).ToLower();

            var dt = new DataTable();
            using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                dt = Importdt(fs, fileExt);
            }
            return dt;
        }

        /// <summary>
        ///导入 获取dt
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="fileExt">文件名后缀</param>
        /// <returns></returns>
        public static DataTable Importdt(Stream fs, string fileExt)
        {
            fs.Seek(0, SeekOrigin.Begin);
            var dt = new DataTable();
            IWorkbook workbook;
            ISheet sheet;
            if (fileExt == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                workbook = new HSSFWorkbook(fs);
            }
            if (workbook is null)
            {
                return null;
            }
            else
            {
                int sheetCount = workbook.NumberOfSheets;
                int columns = 0;
                if (sheetCount > 0)
                {
                    sheet = workbook.GetSheetAt(0);
                    IRow header = sheet.GetRow(sheet.FirstRowNum);
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        object obj = GetValueType(header.GetCell(i));
                        if (obj is null || string.IsNullOrEmpty(obj.ToString()))
                        {
                            dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                        }
                        else
                        {
                            dt.Columns.Add(new DataColumn(obj.ToString()));
                        }
                    }
                    columns = dt.Columns.Count;
                }
                for (int i = 0; i < sheetCount; i++)
                {
                    sheet = workbook.GetSheetAt(i);
                    for (int j = sheet.FirstRowNum + 1; j <= sheet.LastRowNum; j++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        for (int k = 0; k < columns; k++)
                        {
                            dr[k] = GetValueType(sheet.GetRow(j)?.GetCell(k));
                            if (dr[k] != null && !string.IsNullOrEmpty(dr[k].ToString()))
                            {
                                hasValue = true;
                            }
                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        ///导入 获取ds
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="fileExt">文件名后缀</param>
        /// <returns></returns>
        public static DataSet Importds(Stream fs, string fileExt)
        {
            fs.Seek(0, SeekOrigin.Begin);
            var ds = new DataSet();
            IWorkbook workbook;
            ISheet sheet;
            if (fileExt == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                workbook = new HSSFWorkbook(fs);
            }
            if (workbook is null)
            {
                return null;
            }
            else
            {
                int sheetCount = workbook.NumberOfSheets;
                int columns = 0;

                for (int i = 0; i < sheetCount; i++)
                {
                    sheet = workbook.GetSheetAt(i);
                    var dt = new DataTable();
                    dt.TableName = sheet.SheetName;

                    #region 表头

                    IRow header = sheet.GetRow(sheet.FirstRowNum);
                    for (int j = 0; j < header.LastCellNum; j++)
                    {
                        object obj = GetValueType(header.GetCell(j));
                        if (obj is null || string.IsNullOrEmpty(obj.ToString()))
                        {
                            dt.Columns.Add(new DataColumn("Columns" + j.ToString()));
                        }
                        else
                        {
                            dt.Columns.Add(new DataColumn(obj.ToString()));
                        }
                    }
                    columns = dt.Columns.Count;

                    #endregion 表头

                    #region 表内容

                    for (int j = sheet.FirstRowNum + 1; j <= sheet.LastRowNum; j++)
                    {
                        DataRow dr = dt.NewRow();
                        bool hasValue = false;
                        for (int k = 0; k < columns; k++)
                        {
                            dr[k] = GetValueType(sheet.GetRow(j)?.GetCell(k));

                            if (dr[k] != null && !string.IsNullOrEmpty(dr[k].ToString()))
                            {
                                hasValue = true;
                            }
                        }
                        if (hasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }

                    #endregion 表内容

                    ds.Tables.Add(dt);
                }
            }
            return ds;
        }

        #endregion NPOI excel import
    }
}
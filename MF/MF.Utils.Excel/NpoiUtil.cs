using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

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
        /// <returns></returns>
        public static byte[] Export(DataTable dt, ExcelVersion version = ExcelVersion.V2007)
        {
            Workbook(version);
            string sheetname = "sheet1";
            ISheet sheet = workbook.CreateSheet(sheetname);
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

        public static MemoryStream ExportDataTableToExcel(DataTable sourceTable, ExcelVersion version = ExcelVersion.V2007)
        {
            Workbook(version);
            int dtRowsCount = sourceTable.Rows.Count;
            //int SheetCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dtRowsCount) / Max));
            int SheetNum = 1;
            int rowIndex = 1;
            int tempIndex = 1; //标示
            ISheet sheet = workbook.CreateSheet("sheet" + SheetNum);
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
                    sheet = workbook.CreateSheet("sheet" + SheetNum);//
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

        public static byte[] ExportToByte(DataTable dt, ExcelVersion version = ExcelVersion.V2007)
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
                var sheetname = "sheet" + (i + 1);

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

        public static byte[] Export<T>(List<T> list, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
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
                string sheetname = "sheet" + (i + 1);
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
        /// <param name="version"></param>
        /// <param name="ignoreExport"></param>
        /// <param name="sn">是否包含序号</param>
        /// <returns></returns>
        public static IWorkbook ExportToExcel<T>(List<T> list, bool sn = true, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
        {
            return ExportToExcel(list, 0, sn, version, ignoreExport);
        }

        /// <summary>
        /// 包含序号生成 从指定行开始
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="version"></param>
        /// <param name="ignoreExport"></param>
        /// <param name="sn">是否包含序号</param>
        /// <returns></returns>
        public static IWorkbook ExportToExcel<T>(List<T> list, int startRow, bool sn = true, ExcelVersion version = ExcelVersion.V2007, string[] ignoreExport = null)
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
                string sheetname = "sheet" + (i + 1);
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
                    ffont.Color = HSSFColor.White.Index;

                    style.SetFont(ffont);
                    style.FillPattern = FillPattern.SolidForeground;
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
                            style.GetFont(workbook).Color = HSSFColor.White.Index;
                        }
                        else
                        {
                            //ffont.Color = model.FrontColor;
                            style.GetFont(workbook).Color = model.FrontColor;
                        }

                        //style.SetFont(ffont);
                        //style.FillPattern = FillPattern.SolidForeground;
                        style.FillPattern = FillPattern.SolidForeground;
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
                        row1.CreateCell(0).SetCellValue(k + 1);
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
                            dr[k] = GetValueType(sheet.GetRow(j).GetCell(k));
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
                            dr[k] = GetValueType(sheet.GetRow(j).GetCell(k));

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
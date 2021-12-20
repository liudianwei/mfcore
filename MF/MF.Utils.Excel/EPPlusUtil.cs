using OfficeOpenXml;

using System;
using System.Data;
using System.IO;
using System.Linq;

namespace MF.Utils.Excel
{
    public enum ExcelVersion
    {
        V2003 = 2003,
        V2007 = 2007
    }

    public static class EPPlusUtil
    {
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dt">数据</param>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="cell">开始位置</param>
        /// <param name="dateFormat">时间格式</param>
        /// <returns></returns>
        public static byte[] Export(DataTable dt, string sheetName = "Sheet1", string cell = "A1", string dateFormat = "yyyy-M-d H:m:s")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);
            ws.Cells[cell].LoadFromDataTable(dt, true);
            var dateColumns = from DataColumn d in dt.Columns
                              where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                              select d.Ordinal + 1;
            int rowCount = dt.Rows.Count;
            foreach (var dc in dateColumns)
            {
                ws.Cells[2, dc, rowCount + 2, dc].Style.Numberformat.Format = dateFormat;
            }
            ws.Cells.AutoFitColumns();
            return pck.GetAsByteArray();
        }

        public static DataTable Import(string filePath, int headerRow = 1)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("Path cannot be empty");
            }
            if (!File.Exists(filePath))
            {
                throw new ArgumentNullException("File does not exist");
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage pck = new ExcelPackage(new FileInfo(filePath));
            var sheetCount = GetExcelWorksheetCount(pck);
            var dt = new DataTable();
            for (int i = 0; i < sheetCount; i++)
            {
                var sheet = GetExcelWorksheet(pck, i);
                var columns = from firstRowCell in sheet.Cells[headerRow, 1, headerRow, sheet.Dimension.End.Column]
                              select new DataColumn(headerRow > 0 ? firstRowCell.Value.ToString() : $"Column {firstRowCell.Start.Column}");
                dt.Columns.AddRange(columns.ToArray());
                var startRow = headerRow + 1;
                for (var rowIndex = startRow; rowIndex <= sheet.Dimension.End.Row; rowIndex++)
                {
                    var inputRow = sheet.Cells[rowIndex, 1, rowIndex, sheet.Dimension.End.Column];
                    var row = dt.Rows.Add();
                    foreach (var cell in inputRow)
                    {
                        row[cell.Start.Column - 1] = cell.Value;
                    }
                }
            }
            return dt;
        }

        public static DataTable Import(Stream stream, int headerRow = 1)
        {
            if (stream is null)
            {
                throw new ArgumentNullException("File cannot be empty");
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage pck = new ExcelPackage(stream);
            var sheetCount = GetExcelWorksheetCount(pck);
            var dt = new DataTable();
            for (int i = 0; i < sheetCount; i++)
            {
                var sheet = GetExcelWorksheet(pck, i);
                var columns = from firstRowCell in sheet.Cells[headerRow, 1, headerRow, sheet.Dimension.End.Column]
                              select new DataColumn(headerRow > 0 ? firstRowCell.Value.ToString() : $"Column {firstRowCell.Start.Column}");
                dt.Columns.AddRange(columns.ToArray());
                var startRow = headerRow + 1;
                for (var rowIndex = startRow; rowIndex <= sheet.Dimension.End.Row; rowIndex++)
                {
                    var inputRow = sheet.Cells[rowIndex, 1, rowIndex, sheet.Dimension.End.Column];
                    var row = dt.Rows.Add();
                    foreach (var cell in inputRow)
                    {
                        row[cell.Start.Column - 1] = cell.Value;
                    }
                }
            }
            return dt;
        }

        public static int GetExcelWorksheetCount(ExcelPackage excelPackage)
        {
            int sheetCount = excelPackage.Workbook.Worksheets.Count;
            return sheetCount;
        }

        public static ExcelWorksheet GetExcelWorksheet(ExcelPackage excelPackage, int workSheetIndex)
        {
            if (workSheetIndex < 0) throw new ArgumentOutOfRangeException(nameof(workSheetIndex));
            var sheetCount = GetExcelWorksheetCount(excelPackage);
            if (workSheetIndex > sheetCount)
            {
                throw new ArgumentException($@"索引 {nameof(workSheetIndex)} 大于当前Excel的工作簿数量", nameof(workSheetIndex));//指定的参数已超出有效值的范围
            }
            return excelPackage.Workbook.Worksheets[workSheetIndex];
        }
    }
}
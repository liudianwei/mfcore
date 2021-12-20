using System;
using System.Drawing;

namespace MF.Utils.Excel
{
    public class ExcelDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Excel列名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Excel列批注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Excel背景颜色
        /// </summary>
        public short BackColor { get; set; }

        /// <summary>
        /// Excel前景颜色
        /// </summary>
        public short FrontColor { get; set; }
    }
}
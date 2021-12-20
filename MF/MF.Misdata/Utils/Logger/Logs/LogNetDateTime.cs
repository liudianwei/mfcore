using System;
using System.Globalization;
using System.IO;

namespace Common.Utils.MacroInfoLogger
{
    /// <summary>
    /// 一个日志组件，可以根据时间来区分不同的文件存储
    /// </summary>
    /// <remarks>
    /// 此日志实例将根据日期时间来进行分类，支持的时间分类如下：
    /// <list type="number">
    /// <item>小时</item>
    /// <item>天</item>
    /// <item>周</item>
    /// <item>月份</item>
    /// <item>季度</item>
    /// <item>年份</item>
    /// </list>
    /// </remarks>
    public class LogNetDateTime : LogNetBase, ILogNet
    {
        #region 构造方法

        /// <summary>
        /// 实例化一个根据时间存储的日志组件
        /// </summary>
        /// <param name="filePath">文件存储的路径</param>
        /// <param name="generateMode">存储文件的间隔</param>
        public LogNetDateTime(string dirPath, string filePath, GenerateMode generateMode = GenerateMode.ByEveryYear)
        {
            m_filePath = dirPath + filePath;
            m_generateMode = generateMode;

            LogSaveMode = LogNetManagment.LogSaveModeByDateTime;

            m_filePath = CheckPathEndWithSprit(m_filePath);
        }

        private const string txt = ".log";

        #endregion 构造方法

        /// <summary>
        /// 文件的路径
        /// </summary>
        private readonly string m_filePath = string.Empty;

        /// <summary>
        /// 文件的存储模式，默认按照年份来存储
        /// </summary>
        private readonly GenerateMode m_generateMode = GenerateMode.ByEveryYear;

        /// <summary>
        /// 获取需要保存的日志文件
        /// </summary>
        /// <returns>完整的文件路径，含文件名</returns>
        protected override string GetFileSaveName()
        {
            if (string.IsNullOrEmpty(m_filePath)) return string.Empty;

            switch (m_generateMode)
            {
                case GenerateMode.ByEveryHour:
                    {
                        return $"{m_filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now:yyyyMMdd_HH}";
                    }
                case GenerateMode.ByEveryDay:
                    {
                        return $"{m_filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now:yyyyMMdd}";
                    }
                case GenerateMode.ByEveryWeek:
                    {
                        GregorianCalendar gc = new GregorianCalendar();
                        int weekOfYear = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                        return $"{m_filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now.Year}_W{weekOfYear}";
                    }
                case GenerateMode.ByEveryMonth:
                    {
                        return $"{m_filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now:yyyy_MM}";
                    }
                case GenerateMode.ByEverySeason:
                    {
                        //return $"{m_filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now.Year}_Q{(DateTime.Now.Month / 3) + 1}{defaultOP}{txt}";
                        return $"{m_filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now.Year}_Q{(DateTime.Now.Month / 3) + 1}";
                    }
                case GenerateMode.ByEveryYear:
                    {
                        return $"{m_filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now.Year}";
                    }
                default: return string.Empty;
            }
        }

        /// <summary>
        /// 获取所有的文件夹中的日志文件
        /// </summary>
        /// <returns>所有的文件路径集合</returns>
        public string[] GetExistLogFileNames()
        {
            if (!string.IsNullOrEmpty(m_filePath))
            {
                return Directory.GetFiles(m_filePath, $"{LogNetManagment.LogFileHeadString}*{txt}");
            }
            else
            {
                return Array.Empty<string>();
            }
        }
    }
}
using HslCommunication.LogNet;
using System;
using System.Globalization;
using System.IO;

namespace HslCommunication.LogNet
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
    public class LogNetDateTime : LogPathBase, ILogNet
    {
        #region 构造方法

        /// <summary>
		/// 实例化一个根据时间存储的日志组件，需要指定每个文件的存储时间范围<br />
		/// Instantiate a log component based on time, you need to specify the storage time range for each file
		/// </summary>
		/// <param name="filePath">文件存储的路径</param>
		/// <param name="generateMode">存储文件的间隔</param>
		/// <param name="fileQuantity">指定当前的日志文件数量上限，如果小于0，则不限制，文件一直增加，如果设置为10，就限制最多10个文件，会删除最近时间的10个文件之外的文件。</param>
		public LogNetDateTime(string filePath, GenerateMode generateMode = GenerateMode.ByEveryYear, int fileQuantity = -1)
        {
            this.filePath = CheckPathEndWithSprit(filePath);
            this.generateMode = generateMode;
            this.LogSaveMode = LogSaveMode.Time;
            this.controlFileQuantity = fileQuantity;

            if (!string.IsNullOrEmpty(this.filePath) && !Directory.Exists(this.filePath))
                Directory.CreateDirectory(this.filePath);
        }

        private const string txt = ".log";

        #endregion 构造方法

        /// <summary>
        /// 获取需要保存的日志文件
        /// </summary>
        /// <returns>完整的文件路径，含文件名</returns>
        protected override string GetFileSaveName()
        {
            if (string.IsNullOrEmpty(filePath)) return string.Empty;

            switch (generateMode)
            {
                case GenerateMode.ByEveryHour:
                    {
                        return $"{filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now:yyyyMMdd_HH}";
                    }
                case GenerateMode.ByEveryDay:
                    {
                        return $"{filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now:yyyyMMdd}";
                    }
                case GenerateMode.ByEveryWeek:
                    {
                        GregorianCalendar gc = new GregorianCalendar();
                        int weekOfYear = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                        return $"{filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now.Year}_W{weekOfYear}";
                    }
                case GenerateMode.ByEveryMonth:
                    {
                        return $"{filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now:yyyy_MM}";
                    }
                case GenerateMode.ByEverySeason:
                    {
                        //return $"{m_filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now.Year}_Q{(DateTime.Now.Month / 3) + 1}{defaultOP}{txt}";
                        return $"{filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now.Year}_Q{(DateTime.Now.Month / 3) + 1}";
                    }
                case GenerateMode.ByEveryYear:
                    {
                        return $"{filePath}{LogNetManagment.LogFileHeadString}{DateTime.Now.Year}";
                    }
                default: return string.Empty;
            }
        }

        #region Private Member

        private GenerateMode generateMode = GenerateMode.ByEveryYear;             // 文件的存储模式，默认按照年份来存储

        #endregion Private Member

        #region Object Override

        /// <inheritdoc/>
        public override string ToString() => $"LogNetDateTime[{generateMode}]";

        #endregion Object Override
    }
}
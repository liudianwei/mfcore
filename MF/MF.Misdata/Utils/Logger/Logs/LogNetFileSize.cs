using System;
using System.IO;

namespace HslCommunication.LogNet
{
    /// <summary>
    /// 根据文件的大小来存储日志信息
    /// </summary>
    /// <remarks>
    /// 此日志的实例是根据文件的大小储存，例如设置了2M，每隔2M，系统将生成一个新的日志文件。
    /// </remarks>
    public class LogNetFileSize : LogPathBase, ILogNet
    {
        #region Constructor

        /// <summary>
        /// 实例化一个根据文件大小生成新文件的，默认是2M大的文件<br />
        /// Instantiate a new file based on the file size. The default is 2M.
        /// </summary>
        /// <param name="filePath">日志文件的保存路径</param>
        /// <param name="fileMaxSize">每个日志文件的最大大小，默认2M</param>
        /// <param name="fileQuantity">指定当前的日志文件数量上限，如果小于0，则不限制，文件一直增加，如果设置为10，就限制最多10个文件，会删除最近时间的10个文件之外的文件。</param>
        public LogNetFileSize(string filePath, int fileMaxSize = 2 * 1024 * 1024, int fileQuantity = -1)
        {
            this.filePath = filePath;
            this.fileMaxSize = fileMaxSize;
            this.controlFileQuantity = fileQuantity;
            this.LogSaveMode = LogSaveMode.FileFixedSize;

            if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
        }

        #endregion Constructor

        /// <summary>
        /// 当前正在存储的文件名称
        /// </summary>
        private string m_fileName = string.Empty;

        private readonly string m_filePath = string.Empty;

        //private readonly int m_fileMaxSize = 2 * 1024 * 1024; //2M

        /// <summary>
        /// 获取需要保存的日志文件
        /// </summary>
        /// <returns>字符串数据</returns>
        protected override string GetFileSaveName()
        {
            // 路径没有设置则返回空
            if (string.IsNullOrEmpty(filePath)) return string.Empty;

            // 当软件重新第一次写入日志的时候，需要遍历下所有的文件信息，找一个大小合适的文件进行写入
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) fileName = GetLastAccessFileName();

            if (File.Exists(fileName))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Length > fileMaxSize)
                {
                    //新生成文件
                    fileName = GetDefaultFileName();
                }
                else
                {
                    currentFileSize = (int)fileInfo.Length;
                }
            }
            return fileName;
        }

        #region Private Method

        /// <summary>
        /// 获取之前保存的日志文件
        /// </summary>
        /// <returns></returns>
        private string GetLastAccessFileName()
        {
            try
            {
                foreach (var m in GetExistLogFileNames())
                {
                    FileInfo fileInfo = new FileInfo(m);
                    if (fileInfo.Length < fileMaxSize)
                    {
                        currentFileSize = (int)fileInfo.Length;
                        return m;
                    }
                }
            }
            catch (Exception e)
            {
                currentFileSize = 0;
            }

            // 返回一个新的默认当前时间的日志名称
            return GetDefaultFileName();
        }

        /// <summary>
        /// 获取一个新的默认的文件名称<br />
        /// Get a new default file name
        /// </summary>
        /// <returns>完整的文件名</returns>
        private string GetDefaultFileName() => Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");

        #endregion Private Method

        #region Private Member

        private int fileMaxSize = 2 * 1024 * 1024;                 // 最大的文件状态2M
        private int currentFileSize = 0;                           // 当前的文件大小

        #endregion Private Member

        #region Object Override

        /// <inheritdoc/>
        public override string ToString() => $"LogNetFileSize[{fileMaxSize}]";

        #endregion Object Override
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SystemFramework
{
    /// <summary>
    /// Class1 的摘要说明。
    /// </summary>
    public class ApplicationLog
    {
        //This object is added as a debug listener.
        private static StreamWriter debugWriter;

        /// <summary>
        /// 日志跟踪路径
        /// </summary>
        public static string TraceFolderPath { set; get; } = string.Empty;

        /// <summary>
        /// 日志保留时间(天)
        /// </summary>
        private static int SaveDate { set; get; } = 14;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="catchInfo"></param>
        /// <returns></returns>
        public static string FormatException(Exception ex, string catchInfo)
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("Message:" + DateTime.Now.ToString() + "\r\n");
            if (catchInfo != string.Empty)
            {
                strBuilder.Append(catchInfo).Append("\r\n");
            }
            strBuilder.Append(ex.Message).Append("\r\n").Append(ex.StackTrace).Append("\r\n").Append(ex.Source).Append("\r\n").Append(DateTime.Now.ToString()).Append("\r\n\r\n");

            ApplicationAssert.GetDetailMessage(strBuilder.ToString(), ApplicationAssert.LineNumber);
            return strBuilder.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="catchInfo"></param>
        public static void WriteLog(Exception ex, string catchInfo)
        {
            WriteLog(FormatException(ex, catchInfo));
        }

        /// <summary>
        ///     Determine where a string needs to be written based on the
        ///     configuration settings and the error level.
        ///     <param name="messageText">The string to be logged.</param>
        /// </summary>
        public static void WriteLog(string messageText)
        {
            //
            // Be very careful by putting a Try/Catch around the entire routine.
            //   We should never throw an exception while logging.
            //
            try
            {
                //
                // Write the message to the trace file
                //

                //Make sure a tracing file is specified.
                if (debugWriter != null)
                {
                    lock (debugWriter)
                    {
                        //写入Log文件
                        Debug.WriteLine("Time:" + DateTime.Now.ToString() + "\r\n" + messageText);
                        debugWriter.Flush();
                    }
                }
            }
            catch { } //Ignore any exceptions.
        }

        /// <summary>
        /// 是否输出Console
        /// </summary>
        public static bool IsConsole = false;

        /// <summary>
        ///
        /// </summary>
        public ApplicationLog(bool isconsole = false)
        {
            IsConsole = isconsole;
            //Protect thread locks with Try/Catch to guarantee that we let go of the lock.
            try
            {
                string MES_Code = "";
                try
                {
                    MES_Code = ConfigurationManager.AppSettings.GetValues("MES_Code")[0].ToString();
                }
                catch
                {
                    MES_Code = Guid.NewGuid().ToString();
                }
                try
                {
                    TraceFolderPath = ConfigurationManager.AppSettings.GetValues("TraceFolderPath")[0].ToString();
                }
                catch
                {
                    TraceFolderPath = "C:\\Macroinf\\";
                }

                TraceFolderPath += "Macro_" + MES_Code + "\\";
                //See if there is a debug configuration file specified and set up the
                //  tracing variables.
                bool clearSettings = true;
                try
                {
                    string tracingFile = string.Format("{0}SystemLog/{1}", TraceFolderPath, DateTime.Now.ToString("yyyyMMdd"));
                    if (!Directory.Exists(tracingFile)) Directory.CreateDirectory(tracingFile);
                    FileInfo file = new FileInfo(string.Format("{0}/{1}.log", tracingFile, "Log_" + (DateTime.Now.ToLocalTime().ToString().Replace("/", "_").Replace(':', '_').Replace(' ', '_'))));

                    debugWriter = new StreamWriter(file.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
                    Debug.Listeners.Add(new TextWriterTraceListener(debugWriter));

                    TextWriterTraceListener consoleWriter = new
                        TextWriterTraceListener(Console.Out);

                    Trace.Listeners.Add(consoleWriter);

                    clearSettings = false;
                }
                catch (Exception e)
                {
                    //Ignore the error
                }

                //Use default (empty) values if something went wrong
                if (clearSettings)
                {
                    debugWriter = null;
                }
            }
            finally
            {
                //Remove the lock from the class object
                //Monitor.Exit(myType);
                try
                {
                    SaveDate = Convert.ToInt32(ConfigurationManager.AppSettings.GetValues("SaveDate")[0]);
                }
                catch
                {
                    SaveDate = 14;
                    WriteLog("未适配BusinessLog文件保存时间(天):配置<SaveDate>将以默认值(" + SaveDate + ")启动");
                }

                DeleteDir(string.Format("{0}BusinessLog", TraceFolderPath), SaveDate);
            }
        }

        /// <summary>
        /// 记录逻辑日志
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="strContentInfo"></param>
        public static void BusinessLog(string fileName, string strContentInfo)
        {
            try
            {
                // 拼日志格式
                string str = string.Format("{0}  {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), strContentInfo);
                if (IsConsole)
                {
                    EnqueueConsole(new Msg() { Name = fileName, Content = str });
                }
                EnqueueLog(fileName, new Msg() { Name = fileName, Content = str });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 删除文件夹strDir中nDays天以前的文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="days"></param>
        public static void DeleteDir(string file, int days)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                File.SetAttributes(file, FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            var t = File.GetLastWriteTime(f);

                            var elapsedTicks = DateTime.Now.Ticks - t.Ticks;
                            var elapsedSpan = new TimeSpan(elapsedTicks);

                            if (elapsedSpan.TotalDays > days)
                                File.Delete(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f, days);
                        }
                    }

                    //删除空文件夹
                    Directory.Delete(file);
                    Console.WriteLine(file);
                }
            }
            catch (Exception ex) // 异常处理
            {
                Console.WriteLine(ex.Message.ToString());// 异常信息
            }
        }

        #region Console队列

        /// <summary>
        /// 通知队列
        /// </summary>
        public static ConcurrentQueue<Msg> concurrentQueueConsole = new ConcurrentQueue<Msg>();

        /// <summary>
        /// 通知队列 信号量
        /// </summary>
        public static AutoResetEvent autoResetQueueConsole = new AutoResetEvent(false);

        /// <summary>
        ///写入消息队列
        /// </summary>
        /// <param name="msg"></param>
        private static void EnqueueConsole(Msg msg)
        {
            concurrentQueueConsole.Enqueue(msg);
            autoResetQueueConsole.Set(); //通知task队列里有内容了  可以开始循环读取
        }

        #endregion Console队列

        #region Log队列

        /// <summary>
        /// 通知队列
        /// </summary>
        public static Dictionary<string, QueueLog> QueueLogDic { set; get; } = new Dictionary<string, QueueLog>();

        /// <summary>
        /// 写入消息队列
        /// </summary>
        /// <param name="OpName"></param>
        /// <param name="msg"></param>
        public static void EnqueueLog(string OpName, Msg msg)
        {
            if (QueueLogDic.TryGetValue(OpName, out var queueLog))
            {
                queueLog.Enqueue(msg);
            }
            else
            {
                queueLog = new QueueLog(OpName);
                queueLog.Enqueue(msg);
                QueueLogDic.Add(OpName, queueLog);
            }
        }

        /// <summary>
        /// 初始化日志线程
        /// </summary>
        public static void IntLogThread(List<string> OpNames)
        {
            foreach (var item in OpNames)
            {
                if (!QueueLogDic.ContainsKey(item))
                {
                    QueueLogDic.Add(item, new QueueLog(item));
                }
            }
        }

        #endregion Log队列
    }

    /// <summary>
    ///
    /// </summary>
    public class Msg
    {
        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class QueueLog
    {
        /// <summary>
        ///
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public ConcurrentQueue<Msg> concurrentQueueLog = new ConcurrentQueue<Msg>();

        /// <summary>
        ///
        /// </summary>
        public AutoResetEvent autoResetQueueLog = new AutoResetEvent(false);

        private Thread thread = null;

        //读写锁，当资源处于写入模式时，其他线程写入需要等待本次写入结束之后才能继续写入
        private static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        ///
        /// </summary>
        public QueueLog(string name)
        {
            Name = name;
            ThreadSaveFile();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        public void Enqueue(Msg msg)
        {
            concurrentQueueLog.Enqueue(msg);
            autoResetQueueLog.Set(); //通知task队列里有内容了  可以开始循环读取
        }

        /// <summary>
        ///
        /// </summary>
        private void ThreadSaveFile()
        {
            thread = new Thread(() =>
            {
                while (true)
                {
                    autoResetQueueLog.WaitOne();
                    while (concurrentQueueLog.Any())
                    {
                        if (concurrentQueueLog.TryDequeue(out var msg))
                        {
                            try
                            {
                                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入
                                //注意：长时间持有读线程锁或写线程锁会使其他线程发生饥饿 (starve)。 为了得到最好的性能，需要考虑重新构造应用程序以将写访问的持续时间减少到最小。
                                //从性能方面考虑，请求进入写入模式应该紧跟文件操作之前，在此处进入写入模式仅是为了降低代码复杂度
                                //因进入与退出写入模式应在同一个try finally语句块内，所以在请求进入写入模式之前不能触发异常，否则释放次数大于请求次数将会触发异常
                                LogWriteLock.EnterWriteLock();

                                string tmpLogPath = string.Format("{0}BusinessLog/{1}", ApplicationLog.TraceFolderPath, DateTime.Now.ToString("yyyyMMdd"));
                                if (!Directory.Exists(tmpLogPath)) Directory.CreateDirectory(tmpLogPath);
                                using (StreamWriter sw = File.AppendText(string.Format("{0}/{1}.log", tmpLogPath, msg.Name)))
                                {
                                    sw.Write(msg.Content);
                                    sw.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                ApplicationLog.WriteLog(ex, $"{msg.Name} BusinessLog日志记录出错");
                            }
                            finally
                            {
                                //退出写入模式，释放资源占用
                                //注意：一次请求对应一次释放
                                //若释放次数大于请求次数将会触发异常[写入锁定未经保持即被释放]
                                //若请求处理完成后未释放将会触发异常[此模式不下允许以递归方式获取写入锁定]
                                LogWriteLock.ExitWriteLock();
                            }
                        }
                    }
                    autoResetQueueLog.Reset();
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace Mes.Exe.Driver.Redis
{
    /// <summary>
    /// Redis起始检查与安装
    /// </summary>
    public class MesRedisStartCheck
    {
        /// <summary>
        /// 
        /// </summary>
        private static string redisServerName = "mesredis";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="isStopToStart"></param>
        /// <param name="isServerStart"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        private static bool CheckServer(string serverName, bool isStopToStart, out bool isServerStart, out string errorMsg)
        {
            bool flag = false;
            isServerStart = false;
            errorMsg = string.Empty;
            try
            {
                var serviceControllers = ServiceController.GetServices();
                var server = serviceControllers.FirstOrDefault(service => service.ServiceName == serverName);
                if (server == null) return flag;
                flag = true;
                if (server.Status != ServiceControllerStatus.Running)
                {
                    if (isStopToStart)
                    {
                        server.Start();
                    }
                }
                else
                {
                    isServerStart = true;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return flag;
        }
        /// <summary>
        /// 检测Redis服务
        /// </summary>
        /// <param name="errrorMsg"></param>
        /// <returns></returns>
        private static bool CheckRedisServer(out string errrorMsg)
        {
            bool flag = false;
            errrorMsg = string.Empty;
            try
            {
                bool isServerStart = false;
                bool isServerExsit = CheckServer(redisServerName, true, out isServerStart, out errrorMsg);
                if (!string.IsNullOrEmpty(errrorMsg)) return flag;
                if (!isServerExsit)
                {
                    string redisBatPath = MesStartFilePath.Get() + "Redis\\service-install.bat";
                    if (!File.Exists(redisBatPath))
                    {
                        errrorMsg = "不存在:" + redisBatPath;
                        return flag;
                    }
                    Process proc = new Process();
                    string targetDir = string.Format(MesStartFilePath.Get() + @"Redis\");
                    proc.StartInfo.WorkingDirectory = targetDir;
                    proc.StartInfo.FileName = "service-install.bat";
                    proc.StartInfo.Arguments = string.Format("10");
                    proc.Start();
                    proc.WaitForExit();
                    Thread.Sleep(1000);
                    flag = true;
                    return flag;
                }
                else
                {
                    if (isServerStart) return true;
                    Thread.Sleep(3000);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                errrorMsg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 判断Redis服务是否正常开启
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool CheckRedisServiceIsON(out string errorMsg)
        {
            bool flag = false;
            errorMsg = string.Empty;
            try
            {
                if (!CheckRedisServer(out errorMsg))
                {
                    errorMsg = "Redis服务器未开启|" + errorMsg;
                }
                else
                {
                    Thread.Sleep(1000);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Redis服务器未开启|" + ex.Message;
            }
            return flag;
        }
    }
}

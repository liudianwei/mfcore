using System;

namespace Mes.Exe.Driver.Redis
{
    /// <summary>
    ///
    /// </summary>
    public class MesStartFilePath
    {
        /// <summary>
        /// 获取EAP程序本地运行BIN目录
        /// </summary>
        /// <returns></returns>
        public static string Get()
        {
            string path = string.Empty;
            try
            {
                string location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string serviceFileName = location.Substring(0, location.LastIndexOf('\\') + 1);
                path = serviceFileName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return path;
        }
    }
}
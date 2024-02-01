using Common.Utils;
using System;
using System.IO;
using HslCommunication.Enthernet;
using System.Text;
using System.Net;
using HslCommunication;

namespace Common.Communication
{
    /// <summary>
    /// httpWeb 服务端
    /// </summary>
    public class HttpWebServerHelper
    {
        /// <summary>
        /// HttpWebServer是否启用
        /// </summary>
        public static bool HttpWebServerIsEnable { get; set; } = false;

        /// <summary>
        /// HttpWebServer端口
        /// </summary>
        public static int HttpWebServerPort { get; set; } = 18666;

        /// <summary>
        /// HttpWebServer是否跨域
        /// </summary>
        public static bool HttpWebServerIsCrossDomain { get; set; } = false;

        /// <summary>
        /// HttpWebServer返回格式 text/html,text/plain,text/xml,application/xml,application/json
        /// </summary>
        public static string HttpWebServerResultFormat { get; set; } = "application/json";

        /// <summary>
        /// HttpWeb服务器
        /// </summary>
        public static HttpServer httpWebServer = new HttpServer();

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(string ServiceName = "MisDataApi", object obj = null)
        {
            try
            {
                HttpWebServerIsEnable = Convert.ToBoolean(ConfigHelper.GetAppseting("HttpWebServer:IsEnable") ?? "False");
                HttpWebServerPort = Convert.ToInt32(ConfigHelper.GetAppseting("HttpWebServer:Port") ?? "18666");
                HttpWebServerIsCrossDomain = Convert.ToBoolean(ConfigHelper.GetAppseting("HttpWebServer:IsCrossDomain") ?? "False");
                HttpWebServerResultFormat = ConfigHelper.GetAppseting("HttpWebServer:ResultFormat") ?? "application/json";
                if (HttpWebServerIsEnable)
                {
                    httpWebServer.IsCrossDomain = HttpWebServerIsCrossDomain;
                    httpWebServer.HandleRequestFunc = HandleRequest;
                    httpWebServer.Start(HttpWebServerPort);
                    httpWebServer.RegisterHttpRpcApi(ServiceName, obj);
                    Console.WriteLine($"HttpServer:http://+:{HttpWebServerPort}/");
                    SystemLog.Info($"HttpServer:http://+:{HttpWebServerPort}/ Start Up" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Port:{HttpWebServerPort} " + ex.Message + "请检查是否以管理身份运行");
                SystemLog.Fatal($"HttpWebServerHelper,Port:{HttpWebServerPort}" + ",请检查是否以管理身份运行", ex);
            }
        }

        /// <summary>
        /// 获取WebSite
        /// </summary>
        /// <returns></returns>
        private static string GetHtmlContent(string fileName = "WebSite.html")
        {
            string filePath = Directory.GetCurrentDirectory() + $"\\{fileName}";
            string errorHttp = $"<html><head><title>WebSite</title></head><body><p style=\"color: red\">404:未找到{fileName}</p></body></html>";
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    int fsLen = (int)fs.Length;
                    byte[] heByte = new byte[fsLen];
                    int r = fs.Read(heByte, 0, heByte.Length);
                    if (r > 0)
                    {
                        return Encoding.UTF8.GetString(heByte);
                    }
                    else
                    {
                        return errorHttp;
                    }
                }
            }
            else
            {
                return errorHttp;
            }
        }

        /// <summary>
        /// 获取或设置当前的自定义的处理信息，如果不想继承实现方法，可以使用本属性来关联你自定义的方法。<br />
        /// Get or set the current custom processing information. If you don't want to inherit the implementation method, you can use this attribute to associate your custom method.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string HandleRequest(HttpListenerRequest request, HttpListenerResponse response, string data)
        {
            if (request.HttpMethod == "GET" && request.RawUrl.ToLower().StartsWith("/apis"))
            {
                response.AddHeader("Content-type", "application/json; charset=utf-8");
                return $"{httpWebServer.GetAllRpcApiInfo().ToJsonString()}";
            }
            if (request.HttpMethod == "GET" && request.RawUrl.ToLower().StartsWith("/logs"))
            {
                response.AddHeader("Content-type", "application/json; charset=utf-8");
                if (request.RawUrl.ToLower() == "/logs" || request.RawUrl.ToLower() == "/logs/")
                    return httpWebServer.LogStatistics.LogStat.GetStatisticsSnapshot().ToJsonString();
                else
                    return $"{httpWebServer.LogStatistics.GetStatisticsSnapshot(request.RawUrl.Substring(6)).ToJsonString()}";
            }
            if (request.HttpMethod == "GET" && request.RawUrl == "/")
            {
                response.AddHeader("Content-type", "text/html; charset=utf-8");
                return $"{GetHtmlContent().Replace("'Apis.Json'", httpWebServer.GetAllRpcApiInfo().ToJsonString())}";
            }
            response.AddHeader("Content-type", $"{HttpWebServerResultFormat}; charset=utf-8");
            return HttpServer.HandleObjectMethod(request, request.RawUrl, data, httpWebServer, action: null).Result;
        }
    }
}
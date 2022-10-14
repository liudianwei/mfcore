using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Helper
{
    /// <summary>
    /// 工具类：HttpWeb请求
    /// </summary>
    public class HttpWebHelper
    {
        /// <summary>
        /// Request请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data">传递的参数</param>
        /// <param name="contentType">application/x-www-form-urlencoded</param>
        /// <param name="Method">POST,GET等</param>
        /// <param name="Timeout">请求超时</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> RequestHttpAsync(string url, string data,
            string contentType = "application/json;charset=utf-8",
            string Method = "POST", int Timeout = 5000)
        {
            using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(Timeout) })
            {
                using (var request = new HttpRequestMessage(new HttpMethod(Method), url))
                {
                    request.Headers.TryAddWithoutValidation("Content-Type", contentType);
                    request.Content = new StringContent(data);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                    return await httpClient.SendAsync(request);
                }
            }
        }
		
		/// <summary>
        /// Request请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data">传递的参数</param>
        /// <param name="contentType">application/x-www-form-urlencoded</param>
        /// <param name="Method">POST,GET等</param>
        /// <param name="Timeout">请求超时</param>
        /// <returns></returns>
        public static string RequestHttp(string url, string data = "[]", 
		    string contentType = "application/json;charset=utf-8", 
			string Method = "POST",
            int Timeout = 5000)
        {
            try
            {
                using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(Timeout) })
                {
                    using (var request = new HttpRequestMessage(new HttpMethod(Method), url))
                    {
                        request.Headers.TryAddWithoutValidation("Content-Type", contentType);
                        request.Content = new StringContent(data);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                        HttpResponseMessage response = httpClient.SendAsync(request).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (HttpContent content = response.Content)
                            {
                                return content.ReadAsStringAsync().Result;
                            }
                        }
                        else
                        {
                            throw new Exception(response.RequestMessage.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
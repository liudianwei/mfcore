using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MF.Utils.Json;

namespace MF.NetCoreApp
{
    public static class HttpExtensions
    {
        public static string GetUrl(HttpRequest req)
        {
            return $"{req.Scheme}://{req.Host}{req.PathBase}{req.Path}{req.QueryString}";
        }

        /// <summary>
        /// 读取返回流
        /// Item1 参数，Item2 url,Item3 method
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static (string, string, string) ReadResultExecutingContext(this ResultExecutingContext context)
        {
            var req = context.HttpContext.Request;
            req.EnableBuffering();
            var method = req.Method;
            var url = GetUrl(req);
            var urlParam = req.QueryString.ToUriComponent();
            if (!string.IsNullOrWhiteSpace(urlParam))
            {
                var qs = QueryHelpers.ParseQuery(urlParam);
                return (qs.JilToJson(), url, method);
            }
            if (req.HasFormContentType)
            {
                var result = req.ReadFormAsync().Result;
                var pairs = new Dictionary<string, string>();
                foreach (var item in result.Keys)
                {
                    pairs.Add(item, result[item]);
                }
                return (pairs.JilToJson(), url, method);
            }
            using var ms = new MemoryStream();
            req.Body.Position = 0;
            req.Body.CopyTo(ms);
            var bytes = ms.ToArray();
            return (bytes is null ? "" : Encoding.UTF8.GetString(bytes), url, method);
        }
    }
}
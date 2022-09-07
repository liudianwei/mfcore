using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MF.NetCoreApp
{
    /// <summary>
    ///[FromBody] 请求正文 只能有一个参数 json 针对复杂类型参数进行推断
    ///[FromForm] 请求正文中的表单数据
    ///[FromHeader] 请求标头
    ///[FromQuery]     请求查询字符串参数
    ///[FromRoute] 当前请求中的路由数据
    ///[FromServices]  作为操作参数插入的请求服务
    /// url:https://docs.microsoft.com/zh-cn/aspnet/core/web-api/?view=aspnetcore-2.1
    /// </summary>
    [Authorize]
    public class BaseController : Controller
    {
        private IMemoryCache _memory;
        private IConfiguration _configuration;
        public string AppRoot { get { return CreateService<IWebHostEnvironment>().ContentRootPath; } }

        public string WebRoot { get { return CreateService<IWebHostEnvironment>().WebRootPath; } }

        protected IMemoryCache GetMemoryCache
        {
            get
            {
                if (_memory is null)
                {
                    _memory = CreateService<IMemoryCache>();
                    return _memory;
                }
                return _memory;
            }
        }

        public List<string> GetCacheKeys()
        {
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = GetMemoryCache.GetType().GetField("_entries", flags).GetValue(GetMemoryCache);
            var keys = new List<string>();
            if (!(entries is IDictionary cacheItems)) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        protected IConfiguration GetConfiguration
        {
            get
            {
                if (_configuration is null)
                {
                    _configuration = CreateService<IConfiguration>();
                    return _configuration;
                }
                return _configuration;
            }
        }

        protected virtual void ClearCache(string key)
        {
            GetMemoryCache.Remove(key);
        }

        protected virtual void ClearAllCache()
        {
            foreach (var key in GetCacheKeys())
            {
                GetMemoryCache.Remove(key);
            }
        }

        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        protected virtual T CreateService<T>()
        {
            return (T)HttpContext.RequestServices.GetService(typeof(T));
        }

        protected virtual string GetDescriptor(string key)
        {
            return ControllerContext.ActionDescriptor.Properties[key].ToString();
        }

        protected virtual object GetDescriptorObj(string key)
        {
            return ControllerContext.ActionDescriptor.Properties[key];
        }

        protected virtual string GetIp()
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            ip = ip.Replace("::ffff:", "");
            return ip == "::1" ? "127.0.0.1" : ip;
        }

        protected virtual string GetUrl()
        {
            var req = HttpContext.Request;
            return $"{req.Scheme}://{req.Host}{req.PathBase}{req.Path}{req.QueryString}";
        }

        protected virtual string GetDisplayUrl()
        {
            return UriHelper.GetDisplayUrl(HttpContext.Request);
        }

        protected virtual string GetEncodedPathAndQuery()
        {
            return UriHelper.GetEncodedPathAndQuery(HttpContext.Request);
        }

        protected virtual string GetEncodedUrl()
        {
            return UriHelper.GetEncodedUrl(HttpContext.Request);
        }

        protected virtual string GetBrowser()
        {
            return HttpContext.Request.Headers["User-Agent"].ToString();
        }

        protected virtual string GetRequestHeaders(string key)
        {
            return HttpContext.Request.Headers[key].ToString();
        }

        protected virtual void SetPubResponseHeader(string key, string value)
        {
            HttpContext.Response.Headers.Add(key, value);
        }
    }
}
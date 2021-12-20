using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace MF.NetCoreApp
{
    public class GlobalCore
    {
        private static IHttpContextAccessor _accessor;

        public GlobalCore(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public T GetRequiredService<T>()
        {
            return _accessor.HttpContext.RequestServices.GetRequiredService<T>();
        }

        public HttpContext Current => _accessor.HttpContext;

        public static GlobalCore Context => _accessor.HttpContext.RequestServices.GetService(typeof(GlobalCore)) as GlobalCore;

        public IWebHostEnvironment GetHostingEnvironment => GetRequiredService<IWebHostEnvironment>();

        public IServiceScopeFactory GetIServiceScopeFactory => GetRequiredService<IServiceScopeFactory>();

        public string LoginType => _accessor.HttpContext.User.FindFirstValue("loginType");
        public string UserFullName => _accessor.HttpContext.User.FindFirstValue("userFullName");

        //public string TenantCode => _accessor.HttpContext.User.FindFirstValue("tenantCode");
        public string UserName => _accessor.HttpContext.User.FindFirstValue("userName");

        public string UserId => _accessor.HttpContext.User.FindFirstValue("userId");
        public string Jti => _accessor.HttpContext.User.FindFirstValue("jti");
        public string Enabled => _accessor.HttpContext.User.FindFirstValue("enabled");

        //public string TenantId => _accessor.HttpContext.User.FindFirstValue(ClaimTypes.te);

        public string WebRootPath => GetHostingEnvironment?.WebRootPath;

        public string ContentRootPath => GetHostingEnvironment?.ContentRootPath;

        public string EnvironmentName => GetHostingEnvironment?.EnvironmentName;

        public string ApplicationName => GetHostingEnvironment?.ApplicationName;

        public string GetIp()
        {
            var ip = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            ip = ip.Replace("::ffff:", "");
            return ip == "::1" ? "127.0.0.1" : ip;
        }

        public string GetUrl()
        {
            var req = _accessor.HttpContext.Request;
            return $"{req.Scheme}://{req.Host}{req.PathBase}{req.Path}{req.QueryString}";
        }

        public IEnumerable<Claim> GetClaim()
        {
            var isAuthenticated = _accessor.HttpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                return _accessor.HttpContext.User.Claims;
            }
            else
            {
                return null;
            }
        }

        public string GetBrowser() => _accessor.HttpContext.Request.Headers["User-Agent"].ToString();

        public string GetHeaders(string key) => _accessor.HttpContext.Request.Headers[key].ToString();

        public string GetTraceId()
        {
            var activity = Activity.Current;
            return activity?.IdFormat switch
            {
                ActivityIdFormat.Hierarchical => activity.RootId,
                ActivityIdFormat.W3C => activity.TraceId.ToHexString(),
                _ => null ?? _accessor.HttpContext.TraceIdentifier,
            };
        }

        /// <summary>
        /// 用户连接名称
        /// </summary>
        /// <returns></returns>
        public string UserConcatName => string.Concat(UserName, "|", UserFullName);
    }
}
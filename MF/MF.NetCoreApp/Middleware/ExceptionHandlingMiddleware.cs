using Folke.Localization.Json;

using MF.FluentValidation;
using MF.Utils.Json;

using Microsoft.AspNetCore.Http;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MF.NetCoreApp.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string XResponseTimeMilliseconds = "X-Response-Time-Milliseconds";
        private const string ContentType = "application/json;charset=utf-8";

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                context.Request.EnableBuffering();
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext)state;
                    //var buffer = httpContext.Request.BodyReader.ReadAsync().Result.Buffer;
                    //httpContext.Request.BodyReader.AdvanceTo(buffer.Start, buffer.End);

                    var heads = httpContext.Response.Headers;
                    if (!heads.Any(c => c.Key == XResponseTimeMilliseconds))
                    {
                        httpContext.Response.Headers.Add(XResponseTimeMilliseconds, new[] { watch.ElapsedMilliseconds.ToString() });
                        httpContext.Response.Headers.Add("company", "www.macroinf.com");
                    }
                    var statusCode = context.Response.StatusCode;
                    var msg = "";

                    if (statusCode == 303)
                    {
                        msg = "授权码已过期，请联系苏州宏软重新授权，才能继续使用该系统";
                    }
                    else if (statusCode == 401)
                    {
                        msg = "没有token";
                    }
                    else if (statusCode == 403)
                    {
                        msg = "用户当前角色 没有授予权限";
                    }
                    else if (statusCode == 404)
                    {
                        msg = "未找到服务";
                    }
                    else if (statusCode == 502)
                    {
                        msg = "请求错误";
                    }
                    else if (statusCode != 200)
                    {
                        msg = "未知错误";
                    }

                    if (!string.IsNullOrWhiteSpace(msg) && statusCode != 304 && statusCode != 204)
                    {
                        return HandleExceptionAsync(context.Response, statusCode, msg);
                    }
                    return Task.CompletedTask;
                }, context);
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context.Response, ex);
            }
            //finally
            //{
            //    var statusCode = context.Response.StatusCode;
            //    var msg = "";
            //    if (statusCode == 401)
            //    {
            //        msg = "未授权";
            //    }
            //    else if (statusCode == 404)
            //    {
            //        msg = "未找到服务";
            //    }
            //    else if (statusCode == 502)
            //    {
            //        msg = "请求错误";
            //    }
            //    else if (statusCode != 200)
            //    {
            //        msg = "未知错误";
            //    }
            //    if (!string.IsNullOrWhiteSpace(msg) && statusCode != 304)
            //    {
            //        await HandleExceptionAsync(context, statusCode, msg);
            //    }
            //}
        }

        private async Task HandleExceptionAsync(HttpResponse httpResponse, Exception ex)
        {
            var res = new HttpResult
            {
                Code = 100,
                Message = ex.Message,
                Status = "error"
            };

            httpResponse.HttpContext.Request.Headers.TryGetValue("Accept-Language", out var lang);
            string langstr = null;
            if (lang != "")
            {
                langstr = lang.ToString().Split(",")[0].ToLower();
            }

            var localizer = new JsonStringLocalizer(culture: langstr);
            res.Message = localizer.Get(res.Message).Value;

            httpResponse.ContentType = ContentType;
            await httpResponse.WriteAsync(res.JilToJsonCamelCase());
        }

        private Task HandleExceptionAsync(HttpResponse httpResponse, int statusCode, string msg)
        {
            var res = new HttpResult
            {
                Code = statusCode,
                Message = msg,
                Status = "error"
            };
            httpResponse.ContentType = ContentType;
            return httpResponse.WriteAsync(res.JilToJsonCamelCase());
        }
    }
}
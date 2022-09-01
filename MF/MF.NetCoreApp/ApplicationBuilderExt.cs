using MF.NetCoreApp.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace MF.Extensions.DependencyInjection
{
    public static class ApplicationBuilderExt
    {
        /// <summary>
        /// 提供 Web 根目录外的文件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="secends"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDirectoryBrowserL(this IApplicationBuilder app, string fileName)
        {
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), $@"{fileName}")),
                RequestPath = new PathString($"/{fileName}"),
            });
            return app;
        }

        /// <summary>
        /// 启用目录浏览
        /// </summary>
        /// <param name="app"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseFile(this IApplicationBuilder app, string fileName)
        {
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), $@"{fileName}")),
                RequestPath = new PathString($"/{fileName}"),
                EnableDirectoryBrowsing = true
            });
            return app;
        }

        /// <summary>
        /// 添加支持less
        /// img js css 设置 HTTP 响应标头
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCacheControl(this IApplicationBuilder app, int secends = 600)
        {
            var provider = new FileExtensionContentTypeProvider();
            var providers = provider.Mappings;
            //provider.Mappings[".less"] = "text/css";
            providers.Add(".less", "text/css");
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public,max-age={secends}");
                },
                ContentTypeProvider = provider,
            });
            return app;
        }

        public static IApplicationBuilder UseCacheControlHtml(this IApplicationBuilder app, int secends = 10)
        {
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(secends)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };
                await next();
            });
            return app;
        }

        public static IApplicationBuilder UseXssProtection(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1"); //启用XSS保护，并在检测到任何XSS漏洞的情况下阻止加载页面。
                await next();
            });
            return app;
        }

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
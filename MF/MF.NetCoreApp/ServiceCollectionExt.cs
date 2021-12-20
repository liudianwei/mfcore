using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using MF.NetCoreApp;
using MF.Utils;

namespace MF.Extensions.DependencyInjection
{
    /// <summary>
    /// ServiceCollection
    /// </summary>
    public static class ServiceCollectionExt
    {
        /// <summary>
        /// HttpContext上下文
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection GetMachineCodeString(this IServiceCollection services)
        {
            //services.AddHttpContextAccessor();
            Console.WriteLine($"Machine Code: { MachineCode.GetMachineCodeString()}");
            return services;
        }

        /// <summary>
        /// HttpContext上下文
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpContext(this IServiceCollection services)
        {
            //services.AddHttpContextAccessor();
            return services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IServiceCollection AddGlobalCore(this IServiceCollection services)
        {
            return services.AddSingleton(typeof(GlobalCore));
        }

        public static IServiceCollection AddHttpClientFactory(this IServiceCollection services)
        {
            if (services.Count(x => x.ServiceType == typeof(IHttpClientFactory)) == 0)
            {
                services.AddHttpClient();
            }
            //IHttpClientFactory HttpClientFactory
            //services.AddHttpClient();
            return services;
        }

        public static IServiceCollection AddContextAccessor(this IServiceCollection services)
        {
            if (services.Count(x => x.ServiceType == typeof(IHttpContextAccessor)) == 0)
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            if (services.Count(x => x.ServiceType == typeof(IActionContextAccessor)) == 0)
                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            return services;
        }

        public static IServiceCollection AddContextFactory(this IServiceCollection services)
        {
            return services.AddSingleton<IHttpContextFactory, DefaultHttpContextFactory>();
        }

        /// <summary>
        /// url小写
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLowercaseUrls(this IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            return services;
        }

        public static IServiceCollection AddUrlHelper(this IServiceCollection services)
        {
            services
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped(it =>
                    it
                        .GetRequiredService<IUrlHelperFactory>()
                        .GetUrlHelper(it.GetRequiredService<IActionContextAccessor>().ActionContext));
            return services;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services.AddSingleton(configuration);
        }

        /// <summary>
        /// 中文乱码
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHtmlEncoder(this IServiceCollection services)
        {
            return services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
        }

        /// <summary>
        ///Br
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBr(this IServiceCollection services, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            return services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {
                   "image/svg+xml",
                   "application/json",
                });
            }).Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = compressionLevel;
            });
        }

        /// <summary>
        /// Gzip
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGzip(this IServiceCollection services, CompressionLevel compressionLevel = CompressionLevel.Fastest)
        {
            return services.AddResponseCompression(options =>
             {
                 options.Providers.Add<GzipCompressionProvider>();
                 options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                 {
                     // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml",
                    "font/woff2",
                    "application/font-woff",
                    "application/font-ttf",
                    "application/font-eot",
                    "image/jpeg",
                    "image/png"
                 });
             }).Configure<GzipCompressionProviderOptions>(options =>
             {
                 options.Level = compressionLevel;
             });
        }

        public static IServiceCollection AddXsrf(this IServiceCollection services)
        {
            return services.AddAntiforgery(options =>
            {
                //X-CSRF-TOKEN
                options.HeaderName = "X-XSRF-TOKEN";
            });
        }

        public static IServiceCollection AddOption<T>(this IServiceCollection services, string key, IConfiguration configuration) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return services.AddOptions().Configure<T>(configuration?.GetSection(key));
        }

        /// <summary>
        /// 上传大小限制
        /// </summary>
        /// <param name="services"></param>
        /// <param name="key"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMultipartBodyLengthLimit(this IServiceCollection services, IConfiguration configuration, string key = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                //throw new ArgumentNullException(nameof(key));
                services.Configure<FormOptions>(
                f =>
                {
                    f.ValueLengthLimit = int.MaxValue;
                    f.MultipartBodyLengthLimit = int.MaxValue;
                }
                );
            }
            else
            {
                services.Configure<FormOptions>(
               f =>
               {
                   _ = int.TryParse(configuration[key], out int fileLength);
                   f.ValueLengthLimit = fileLength;
                   f.MultipartBodyLengthLimit = fileLength;
               }
               );
            }
            return services;
        }

        /// <summary>
        /// 数据保护
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataProtectionL(this IServiceCollection services)
        {
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory()));
            return services;
        }

        public static IServiceCollection AddLazy(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            return lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(typeof(Lazy<>)),
                ServiceLifetime.Scoped => services.AddScoped(typeof(Lazy<>)),
                ServiceLifetime.Transient => services.AddTransient(typeof(Lazy<>)),
                _ => services.AddTransient(typeof(Lazy<>)),
            };
        }
    }
}
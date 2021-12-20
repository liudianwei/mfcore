using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MF.Swagger;

namespace MF.Extensions.DependencyInjection
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddSwaggerConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<YLSwaggerOptions>(configuration?.GetSection("Swagger"));
            var options = configuration?.GetSection("Swagger")?.Get<YLSwaggerOptions>();
            //var options = new YLSwaggerOptions();
            //action.Invoke(options);
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
            }).AddVersionedApiExplorer();
            if (options.Enabled)
            {
                services.AddSwaggerGen(c =>
                {
                    foreach (var xmlName in options.XmlNames)
                    {
                        //AppDomain.CurrentDomain.BaseDirectory
                        var filePath = Path.Combine(AppContext.BaseDirectory, xmlName);
                        if (File.Exists(filePath))
                        {
                            c.IncludeXmlComments(filePath, true);
                        }
                    }
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var item in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerDoc(item.ApiVersion.ToString(), new OpenApiInfo()
                        {
                            Title = $"API-v{item.ApiVersion}",
                            Version = item.ApiVersion.ToString(),
                            Description = "切换版本请点右上角版本切换",
                        });
                    }
                    c.MapType(typeof(IFormFile), () => new OpenApiSchema() { Type = "file", Format = "binary" });
                    c.CustomSchemaIds(type => type.FullName);
                    string authorization = "Authorization";
                    var security = new Dictionary<string, IEnumerable<string>> { { authorization, Array.Empty<string>() }, };
                    c.OperationFilter<SecurityRequirementsOperationFilter>();
                    c.OperationFilter<SwaggerDefaultValues>();
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "输入Bearer {token}（注意两者之间是一个空格）\"",
                        Name = authorization,
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new [] { string.Empty }
                        }
                    });
                });
            }
            return services;
        }

        public static IServiceCollection AddJwtConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions().Configure<JwtConfig>(configuration?.GetSection("Jwt"));
            var jwtConfig = configuration?.GetSection("Jwt")?.Get<JwtConfig>();
            //ChangeToken.OnChange(() => configuration.GetReloadToken(), () =>
            //{
            //    services.Configure<JwtConfig>(configuration?.GetSection("Jwt"));
            //    var jwtConfig = configuration?.GetSection("Jwt")?.Get<JwtConfig>();
            //    Console.WriteLine($"JwtExpireTime:{jwtConfig.JwtExpireTime}");
            //});
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        //context.Token = context.Request.Query["assess_token"];
                        //context.HttpContext.Request.Headers["Authorization"] = context.Token;
                        return Task.CompletedTask;
                    }
                };
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JwtSecurityKey)),
                    ValidateIssuer = true,//是否验证Issuer
                    ValidIssuer = jwtConfig.JwtIssuer,
                    ValidateAudience = true, //是否验证Audience
                    ValidAudience = jwtConfig.JwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }
    }
}
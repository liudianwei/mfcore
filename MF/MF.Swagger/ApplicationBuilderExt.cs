using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using MF.Swagger;

namespace MF.Extensions.DependencyInjection
{
    public static class ApplicationBuilderExt
    {
        public static IApplicationBuilder UseSwaggerConfigure(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var options = configuration?.GetSection("Swagger")?.Get<YLSwaggerOptions>();
            if (options.Enabled)
            {
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                    });
                });
                app.UseSwaggerUI(c =>
                {
                    var provider = (IApiVersionDescriptionProvider)app.ApplicationServices.GetService(typeof(IApiVersionDescriptionProvider));
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.ApiVersion}/swagger.json", description.ApiVersion.ToString());
                    }
                    c.RoutePrefix = options.RoutePrefix;
                    c.EnableFilter(); // 启用过滤
                    c.DisplayRequestDuration();
                });
            }
            return app;
        }
    }
}
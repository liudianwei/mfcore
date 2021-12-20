using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MF.NetCoreApp
{
    public static class CorsConfig
    {
        public static IServiceCollection AddConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            //.WithOrigins()
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            //.AllowCredentials()
                            ;
                    });
            });
            return services;
        }

        public static IApplicationBuilder UseConfigureCors(this IApplicationBuilder app)
        {
            app.UseCors("AllowAll");
            return app;
        }
    }
}
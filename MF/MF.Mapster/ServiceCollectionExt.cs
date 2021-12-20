using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace MF.Mapster
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddMapsterMapperConfig(this IServiceCollection services, string[] assemblyNames, Action<TypeAdapterConfig> options = null)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            assemblyNames.ToList().ForEach(c =>
            {
                var assembly = new AssemblyName(c);
                config.Scan(Assembly.Load(assembly));
            });
            options?.Invoke(config);
            services.AddSingleton(config);
            return services;
        }

        public static IServiceCollection AddMapsterMapper(this IServiceCollection services)
        {
            return services.AddScoped<IMapper, Mapper>();
        }
    }
}
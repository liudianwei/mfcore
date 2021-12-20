using CSRedis;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using MF.Cache;

namespace MF.Extensions.DependencyInjection
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheOptions>(configuration?.GetSection("Cache"));
            var options = configuration?.GetSection("Cache")?.Get<CacheOptions>();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            switch (options.Category)
            {
                case "Redis":
                    var redis = new CSRedisClient(null, options.Redis.RedisConnectionStrings);
                    RedisHelper.Initialization(redis);
                    var provide = new RedisCacheProvide();
                    services.AddSingleton<ICacheProvide>(provide);
                    services.AddSingleton(new CacheAspectFactory(provide));
                    services.AddSingleton(new CacheRemoveAspectFactory(provide));
                    break;

                case "Memory":
                    var cacheProvide = new MemoryCacheProvide();
                    services.AddSingleton<ICacheProvide>(cacheProvide);
                    services.AddSingleton(new CacheAspectFactory(cacheProvide));
                    services.AddSingleton(new CacheRemoveAspectFactory(cacheProvide));
                    break;

                default:
                    throw new NotImplementedException();
            }

            return services;
        }
    }
}
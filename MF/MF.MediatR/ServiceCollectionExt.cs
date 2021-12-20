using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Linq;
using MF.MediatR;
using MF.Core.Check;

namespace MF.Extensions.DependencyInjection
{
    public static class ServiceCollectionExt
    {
        //  Bus (MediatR)
        public static IServiceCollection AddMediatRBus(this IServiceCollection services, params Assembly[] assemblies)
        {
            CheckNull.ArgumentIsNullException(assemblies);
            services.AddMediatR(assemblies);
            if (services.Count(x => x.ServiceType == typeof(IBus)) == 0)
            {
                services.AddScoped<IBus, Bus>();
            }
            return services;
        }
    }
}
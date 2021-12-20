using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl;

namespace MF.Job
{
    public static class JobServiceCollectionExtensions
    {
        private static ServiceRunner run;

        public static IServiceCollection AddJob(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            StartJob(configuration);

            return services;
        }

        private static void StartJob(IConfiguration configuration)
        {
            run = new ServiceRunner();
            run.Start(configuration);
        }
    }
}
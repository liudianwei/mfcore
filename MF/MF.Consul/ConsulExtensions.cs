using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace MF.Consul
{
    public static class ConsulExtensions
    {
        /// <summary>
        /// 注册服务-Consul
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var lifetime = app.ApplicationServices.GetService(typeof(IHostApplicationLifetime)) as IHostApplicationLifetime;
            var options = configuration?.GetSection("Consul")?.Get<ConsulOptions>();
            var consulClient = new ConsulClient(x =>
            {
                x.Address = new Uri(options.Address);
            });
            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = options.ServiceName,
                Address = options.ServiceIP,
                Port = options.ServicePort,
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(options.DeregisterCriticalServiceAfter ?? 5),
                    Interval = TimeSpan.FromSeconds(options.Interval ?? 10),
                    HTTP = options.ServiceHealthCheck,
                    Timeout = TimeSpan.FromSeconds(options.TimeOut ?? 5)
                }
            };

            // 服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();

            // 应用程序终止时，服务取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }
    }
}
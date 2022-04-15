using System.Reflection;
using System.Runtime.Loader;

using MF.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using SimplCommerce.Modules;

namespace MF.Modules.Log

{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            string assemblyName = $"{Assembly.GetExecutingAssembly().GetName().Name}";
            serviceCollection.RegisterAssembly(assemblyName);
            //serviceCollection.RegisterAssembly("DAL");
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            serviceCollection.AddMediatRBus(assembly);
            serviceCollection.AddEntityMap(assemblyName);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}
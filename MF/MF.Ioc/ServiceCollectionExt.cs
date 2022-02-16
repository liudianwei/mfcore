using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using MF.Ioc;

namespace MF.Extensions.DependencyInjection
{
    public class Di
    {
        public Type ServiceType { get; set; }

        public Type ImplementationType { get; set; }

        public ServiceLifetime ServiceLifetime { get; set; }
    }

    public static class ServiceCollectionExt
    {
        public static IServiceCollection BannerShow(this IServiceCollection services)
        {
            //            string banner = @"
            //      ___           ___           ___           ___           ___                       ___           ___
            //     /\__\         /\  \         /\  \         /\  \         /\  \          ___        /\__\         /\  \
            //    /::|  |       /::\  \       /::\  \       /::\  \       /::\  \        /\  \      /::|  |       /::\  \
            //   /:|:|  |      /:/\:\  \     /:/\:\  \     /:/\:\  \     /:/\:\  \       \:\  \    /:|:|  |      /:/\:\  \
            //  /:/|:|__|__   /::\~\:\  \   /:/  \:\  \   /::\~\:\  \   /:/  \:\  \      /::\__\  /:/|:|  |__   /::\~\:\  \
            // /:/ |::::\__\ /:/\:\ \:\__\ /:/__/ \:\__\ /:/\:\ \:\__\ /:/__/ \:\__\  __/:/\/__/ /:/ |:| /\__\ /:/\:\ \:\__\
            // \/__/~~/:/  / \/__\:\/:/  / \:\  \  \/__/ \/_|::\/:/  / \:\  \ /:/  / /\/:/  /    \/__|:|/:/  / \/__\:\ \/__/
            //       /:/  /       \::/  /   \:\  \          |:|::/  /   \:\  /:/  /  \::/__/         |:/:/  /       \:\__\
            //      /:/  /        /:/  /     \:\  \         |:|\/__/     \:\/:/  /    \:\__\         |::/  /         \/__/
            //     /:/  /        /:/  /       \:\__\        |:|  |        \::/  /      \/__/         /:/  /
            //     \/__/         \/__/         \/__/         \|__|         \/__/                     \/__/
            //";

            var banner = @"
  __  __    _    ____ ____   ___ ___ _   _ _____
 |  \/  |  / \  / ___|  _ \ / _ \_ _| \ | |  ___|
 | |\/| | / _ \| |   | |_) | | | | ||  \| | |_
 | |  | |/ ___ \ |___|  _ <| |_| | || |\  |  _|
 |_|  |_/_/   \_\____|_| \_\\___/___|_| \_|_|

 苏州宏软信息技术有限公司
";
            //            banner = @"
            //  ███╗   ███╗  █████╗   ██████╗ ██████╗   ██████╗  ██╗ ███╗   ██╗ ███████╗
            //  ████╗ ████║ ██╔══██╗ ██╔════╝ ██╔══██╗ ██╔═══██╗ ██║ ████╗  ██║ ██╔════╝
            //  ██╔████╔██║ ███████║ ██║      ██████╔╝ ██║   ██║ ██║ ██╔██╗ ██║ █████╗
            //  ██║╚██╔╝██║ ██╔══██║ ██║      ██╔══██╗ ██║   ██║ ██║ ██║╚██╗██║ ██╔══╝
            //  ██║ ╚═╝ ██║ ██║  ██║ ╚██████╗ ██║  ██║ ╚██████╔╝ ██║ ██║ ╚████║ ██║
            //  ╚═╝     ╚═╝ ╚═╝  ╚═╝  ╚═════╝ ╚═╝  ╚═╝  ╚═════╝  ╚═╝ ╚═╝  ╚═══╝ ╚═╝
            //";
            var c = Console.ForegroundColor;
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(banner);
            Console.ForegroundColor = c;
            Console.WriteLine("请稍等 正在加载配置......");
            return services;
        }

        public static IServiceCollection BannerShowReport(this IServiceCollection services)
        {
            var banner = @"
     ______    ______   ______   ______   ______   _________  
    /_____/\  /_____/\ /_____/\ /_____/\ /_____/\ /________/\ 
    \:::_ \ \ \::::_\/_\:::_ \ \\:::_ \ \\:::_ \ \\__.::.__\/ 
     \:(_) ) )_\:\/___/\\:(_) \ \\:\ \ \ \\:(_) ) )_ \::\ \   
      \: __ `\ \\::___\/_\: ___\/ \:\ \ \ \\: __ `\ \ \::\ \  
       \ \ `\ \ \\:\____/\\ \ \    \:\_\ \ \\ \ `\ \ \ \::\ \ 
        \_\/ \_\/ \_____\/ \_\/     \_____\/ \_\/ \_\/  \__\/ 

                         苏州宏软信息技术有限公司  V1.0.0.0
";
            var c = Console.ForegroundColor;
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(banner);
            Console.ForegroundColor = c;
            Console.WriteLine("请稍等 正在加载配置......");
            return services;
        }
        public static IServiceCollection AttentionShow(this IServiceCollection services)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("注意：");
            Console.ForegroundColor = c;
            Console.WriteLine("     1. 接口不能包含 Base Bin 这2个字符串");
            Console.WriteLine("     2. 数据库表名称同理 不要有 base 字符串");
            Console.WriteLine("     3. 数据库所有表必须有前缀 例如：uc_User");
            Console.WriteLine();
            return services;
        }

        /// <summary>
        /// IDependency 继承这个统一生命周期
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyName"></param>
        /// <param name="injection"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssembly(this IServiceCollection services, string assemblyName, ServiceLifetime injection = ServiceLifetime.Scoped)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            if (assembly is null)
            {
                throw new DllNotFoundException($"\"{assemblyName}\".dll不存在");
            }
            List<Di> dis = new List<Di>();

            // 查找所有不是接口的类
            var types = assembly.GetTypes().Where(o => !o.IsInterface).ToList();

            // 从接口中找出所有继承IDependency的类
            var idependencyTypes = types.Where(o =>
            (typeof(IDependency).IsAssignableFrom(o))).ToList();

            // 从接口中找出特性 [Service] 不能继承 IDependency
            var serviceTypes = types.Where(o =>
           !typeof(IDependency).IsAssignableFrom(o) && o.GetCustomAttribute<ServiceAttribute>() != null).ToList();

            // 从接口中找出特性 [Repository] 不能继承 IDependency
            var repositoryTypes = types.Where(o =>
           !typeof(IDependency).IsAssignableFrom(o) && o.GetCustomAttribute<RepositoryAttribute>() != null).ToList();

            // 从接口中找出特性 [Component] 不能继承 IDependency
            var componentTypes = types.Where(o =>
           !typeof(IDependency).IsAssignableFrom(o) && o.GetCustomAttribute<ComponentAttribute>() != null).ToList();

            var c = Console.ForegroundColor;
            foreach (var type in idependencyTypes)
            {
                var list = type.GetInterfaces().Where(o => !o.Name.StartsWith("IBaseRepository") && !o.Name.StartsWith("IBaseServices") && o.Name.Contains("Base")).ToList();
                if (list.Count > 0)
                {
                    foreach (var i in list)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("ignore interface");

                        Console.ForegroundColor = c;
                        Console.WriteLine(": " + i.Namespace + "." + i.Name);
                    }
                    Console.WriteLine();
                }

                var faces = type.GetInterfaces().Where(o => o.Name != "IDependency" && !o.Name.Contains("Base")).ToArray();
                if (faces.Any())
                {
                    var interfaceType = faces.FirstOrDefault();
                    dis.Add(new Di
                    {
                        ServiceType = interfaceType,
                        ImplementationType = type,
                        ServiceLifetime = injection
                    });
                }
            }
            foreach (var type in serviceTypes)
            {
                var serviceType = type.GetCustomAttribute<ServiceAttribute>(false);
                var faces = type.GetInterfaces();
                if (faces.Any())
                {
                    if (faces.Length > 1)
                    {
                        dis.Add(new Di
                        {
                            ServiceType = serviceType.Type,
                            ImplementationType = type,
                            ServiceLifetime = serviceType.Lifetime
                        });
                    }
                    else
                    {
                        var interfaceType = faces.FirstOrDefault();
                        dis.Add(new Di
                        {
                            ServiceType = interfaceType,
                            ImplementationType = type,
                            ServiceLifetime = serviceType.Lifetime
                        });
                    }
                }
            }
            foreach (var type in repositoryTypes)
            {
                var serviceType = type.GetCustomAttribute<RepositoryAttribute>(false);
                var faces = type.GetInterfaces();
                if (faces.Any())
                {
                    if (faces.Length > 1)
                    {
                        dis.Add(new Di
                        {
                            ServiceType = serviceType.Type,
                            ImplementationType = type,
                            ServiceLifetime = serviceType.Lifetime
                        });
                    }
                    else
                    {
                        var interfaceType = faces.FirstOrDefault();
                        dis.Add(new Di
                        {
                            ServiceType = interfaceType,
                            ImplementationType = type,
                            ServiceLifetime = serviceType.Lifetime
                        });
                    }
                }
            }
            foreach (var type in componentTypes)
            {
                var serviceType = type.GetCustomAttribute<ComponentAttribute>(false);
                var faces = type.GetInterfaces();
                if (faces.Any())
                {
                    if (faces.Length > 1)
                    {
                        dis.Add(new Di
                        {
                            ServiceType = serviceType.Type,
                            ImplementationType = type,
                            ServiceLifetime = serviceType.Lifetime
                        });
                    }
                    else
                    {
                        var interfaceType = faces.FirstOrDefault();
                        dis.Add(new Di
                        {
                            ServiceType = interfaceType,
                            ImplementationType = type,
                            ServiceLifetime = serviceType.Lifetime
                        });
                    }
                }
            }

            foreach (var item in dis)
            {
                switch (item.ServiceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(item.ServiceType, item.ImplementationType);
                        break;

                    case ServiceLifetime.Scoped:
                        services.AddScoped(item.ServiceType, item.ImplementationType);
                        break;

                    case ServiceLifetime.Transient:
                        services.AddTransient(item.ServiceType, item.ImplementationType);
                        break;
                }
            }
            return services;
        }
    }
}
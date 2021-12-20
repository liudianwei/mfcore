using Microsoft.Extensions.DependencyInjection;
using System;

namespace MF.Ioc
{
    /// <summary>
    /// 实现多个接口 指定 type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceAttribute : Attribute
    {
        public ServiceLifetime Lifetime;
        public Type Type;

        public ServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Lifetime = lifetime;
        }

        public ServiceAttribute(Type type, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Type = type;
            Lifetime = lifetime;
        }
    }
}
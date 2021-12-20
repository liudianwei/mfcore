using Microsoft.Extensions.DependencyInjection;
using System;

namespace MF.Ioc
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ComponentAttribute : Attribute
    {
        public ServiceLifetime Lifetime;
        public Type Type;

        public ComponentAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Lifetime = lifetime;
        }

        public ComponentAttribute(Type type, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Type = type;
            Lifetime = lifetime;
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MF.Ioc
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class RepositoryAttribute : Attribute
    {
        public ServiceLifetime Lifetime;
        public Type Type;

        public RepositoryAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Lifetime = lifetime;
        }

        public RepositoryAttribute(Type type, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Type = type;
            Lifetime = lifetime;
        }
    }
}
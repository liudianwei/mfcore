using AspectInjector.Broker;
using System;

namespace MF.Cache
{
    [Injection(typeof(CacheRemoveAspect), Inherited = true)]
    [AttributeUsage(
        AttributeTargets.Method |
        AttributeTargets.Class |
        AttributeTargets.Field |
        AttributeTargets.Property |
        AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public sealed class CacheRemoveAttribute : Attribute
    {
        public string _key = "";

        public CacheRemoveAttribute(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key不能为空");
            }
            _key = key;
        }
    }
}
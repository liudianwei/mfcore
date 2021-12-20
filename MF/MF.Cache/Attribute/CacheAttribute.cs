using AspectInjector.Broker;
using System;

namespace MF.Cache
{
    [AttributeUsage(
        AttributeTargets.Method |
        AttributeTargets.Class |
        AttributeTargets.Field |
        AttributeTargets.Property |
        AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    [Injection(typeof(CacheAspect), Inherited = true)]
    public sealed class CacheAttribute : Attribute
    {
        public bool _flag = true;
        public string _key = "";

        /// <summary>
        /// -1 不设置缓存时间
        /// </summary>
        public int _expireSeconds = -1;

        public CacheAttribute()
        {
        }

        public CacheAttribute(bool flag)
        {
            _flag = flag;
        }

        public CacheAttribute(string key)
        {
            _key = key;
        }

        public CacheAttribute(bool flag, string key)
        {
            _flag = flag;
            _key = key;
        }

        public CacheAttribute(string key, int expireSeconds)
        {
            _key = key;
            _expireSeconds = expireSeconds;
        }

        public CacheAttribute(bool flag, string key, int expireSeconds)
        {
            _flag = flag;
            _key = key;
            _expireSeconds = expireSeconds;
        }
    }
}
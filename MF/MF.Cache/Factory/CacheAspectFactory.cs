using System;
using System.Collections.Generic;
using System.Text;

namespace MF.Cache
{
    public class CacheAspectFactory
    {
        public static ICacheProvide _cache { get; set; }

        public CacheAspectFactory(ICacheProvide cache)
        {
            _cache = cache;
        }

        public static object GetInstance(Type type)
        {
            if (type != typeof(CacheAspect))
            {
                throw new ArgumentException($"{nameof(CacheAspectFactory)} can create instances only of type {nameof(CacheAspect)}");
            }

            if (_cache is null)
            {
                throw new ArgumentException($"{nameof(ICacheProvide)} is null");
            }

            return new CacheAspect(_cache);
        }
    }
}
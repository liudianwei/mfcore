using System;
using System.Collections.Generic;
using System.Text;

namespace MF.Cache
{
    public class CacheRemoveAspectFactory
    {
        public static ICacheProvide _cache { get; set; }

        public CacheRemoveAspectFactory(ICacheProvide cache)
        {
            _cache = cache;
        }

        public static object GetInstance(Type type)
        {
            if (type != typeof(CacheRemoveAspect))
            {
                throw new ArgumentException($"{nameof(CacheRemoveAspectFactory)} can create instances only of type {nameof(CacheRemoveAspect)}");
            }

            if (_cache is null)
            {
                throw new ArgumentException($"{nameof(ICacheProvide)} is null");
            }

            return new CacheRemoveAspect(_cache);
        }
    }
}
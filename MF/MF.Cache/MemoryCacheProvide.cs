using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System;

namespace MF.Cache
{
    public class MemoryCacheProvide : ICacheProvide
    {
        private readonly MemoryCache _cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        private static ICacheProvide _instance;

        public static ICacheProvide GetInstance()
        {
            if (_instance is null)
            {
                _instance = new MemoryCacheProvide();
            }
            return _instance;
        }

        public void Add<T>(string key, T value)
        {
            _cache.Set(key, value);
        }

        public void Add<T>(string key, T value, int expireSeconds)
        {
            _cache.Set(key, value, TimeSpan.FromSeconds(expireSeconds));
        }

        public (bool, T) GetValue<T>(string key)
        {
            return (_cache.TryGetValue(key, out T value), value);
        }

        public void Remove(string key)
        {
            int a = _cache.Count;
            _cache.Remove(key);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
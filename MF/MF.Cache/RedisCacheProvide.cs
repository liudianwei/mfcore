using System;
using System.Threading.Tasks;

namespace MF.Cache
{
    public class RedisCacheProvide : ICacheProvide
    {
        public void Add<T>(string key, T value)
        {
            RedisHelper.Set(key, value);
        }

        public void Add<T>(string key, T value, int expireSeconds)
        {
            RedisHelper.Set(key, value, expireSeconds);
        }

        public (bool, T) GetValue<T>(string key)
        {
            T t = RedisHelper.Get<T>(key);
            if (t is null)
            {
                return (false, t);
            }
            return (true, t);
        }

        public void Remove(string key)
        {
            RedisHelper.Del(key);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
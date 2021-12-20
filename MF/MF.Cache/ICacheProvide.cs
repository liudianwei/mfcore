namespace MF.Cache
{
    public interface ICacheProvide
    {
        void Add<T>(string key, T value);

        void Add<T>(string key, T value, int expireSeconds);

        (bool, T) GetValue<T>(string key);

        void Remove(string key);

        void Clear();
    }
}
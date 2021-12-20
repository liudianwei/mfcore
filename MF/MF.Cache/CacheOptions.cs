namespace MF.Cache
{
    public class CacheOptions
    {
        public bool Enabled { get; set; }

        public string Category { get; set; }

        public string Json { get; set; }

        public Memory Memory { get; set; }

        public Redis Redis { get; set; }
    }

    public class Memory
    {
    }

    public class Redis
    {
        public string[] RedisConnectionStrings { get; set; }
    }
}
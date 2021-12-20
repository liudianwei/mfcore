using IdGen;

using System;
using System.Linq;

namespace MF.Utils
{
    public static class PubId
    {
        public static readonly DateTime DefaultEpoch = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly IdGenerator Generator = new IdGenerator(0);
        private static IdGenerator _snowflake;

        public static IdGenerator GetInstance()
        {
            if (_snowflake is null)
                _snowflake = new IdGenerator(0);
            return _snowflake;
        }

        public static long SnowflakeId
        {
            get
            {
                var id = Generator.CreateId();
                return id;
            }
        }

        public static string SnowflakeIdToString
        {
            get
            {
                //var id = Generator.CreateId().ToString();
                //return id;
                return Uuid();
            }
        }

        public static long SnowflakeId2
        {
            get
            {
                var generator = GetInstance();
                var id = generator.CreateId();
                return id;
            }
        }

        public static long[] SnowflakeIdArray(int take = 1)
        {
            var id = Generator.Take(take);
            return id.ToArray();
        }

        public static string GetUuid() => Guid.NewGuid().ToString().Replace("-", "");

        public static string Uuid() => Guid.NewGuid().ToString();
    }
}
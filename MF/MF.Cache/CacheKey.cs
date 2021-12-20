using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace MF.Cache
{
    public static class CacheKey
    {
        private const string LinkString = ":";

        public static string GetCacheKey(MethodBase methodBase, object[] arguments)
        {
            var methodInfo = (MethodInfo)methodBase;
            var methodFullName = methodInfo.ReflectedType.FullName;
            string key = $"{methodFullName}";
            //string argsKey = "";
            //Array.ForEach(arguments, t =>
            //{
            //    argsKey += t.ToString();
            //});
            //key += LinkString + argsKey;
            return $"{key}{LinkString}{MD5(key)}";
        }

        private static string MD5(string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            using MD5 md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(bytes);
            md5.Clear();
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
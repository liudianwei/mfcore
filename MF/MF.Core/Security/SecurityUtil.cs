using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using MF.Core.Check;
using MF.Core.Extensions;

namespace MF.Core.Security
{
    public static class SecurityUtil
    {
        public static string GetSalt()
        {
            string model = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder salt = new StringBuilder();
            char[] m = model.ToCharArray();
            byte[] buffer = Guid.NewGuid().ToByteArray();//生成字节数组
            int iRoot = BitConverter.ToInt32(buffer, 0);//利用BitConvert方法把字节数组转换为整数
            var ran = new Random(iRoot);
            for (int i = 0; i < 26; i++)
            {
                char c = m[(int)(ran.Next() % model.Length)];
                salt.Append(c);
            }
            return salt.ToString();
        }

        public static string ToMd5(this string str)
        {
            CheckNull.ArgumentIsNullException(str, nameof(str));
            using var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
            return data.ToHexString();
        }

        public static string ToMd5(this Stream inputStream)
        {
            using var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(inputStream);
            return data.ToHexString();
        }

        public static string ToMd5(this byte[] buffer)
        {
            using var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(buffer);
            return data.ToHexString();
        }

        public static string ToMd52(this string str)
        {
            CheckNull.ArgumentIsNullException(str, nameof(str));
            var md5Hasher = new MD5CryptoServiceProvider();
            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            return data.ToHexString();
        }

        public static string ToSha512(this string str)
        {
            CheckNull.ArgumentIsNullException(str, nameof(str));
            SHA512 shaM = new SHA512Managed();
            var result = shaM.ComputeHash(str.ToBytes());
            return result.ToHexString();
        }

        public static string ToSha1(this string str)
        {
            CheckNull.ArgumentIsNullException(str, nameof(str));
            SHA1 shaM = new SHA1Managed();
            var result = shaM.ComputeHash(str.ToBytes());
            return result.ToHexString();
        }

        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static bool PlaintextEquleEiphertext(string plaintext, string ciphertext)
        {
            //return plaintext.IsEmpty() || ciphertext.IsEmpty() ? false : ToMd5(plaintext).Equals(ciphertext);
            return !plaintext.IsEmpty() && !ciphertext.IsEmpty() && ToMd5(plaintext).Equals(ciphertext);
        }

        public static bool PlaintextEquleEiphertextIgnoreCase(string plaintext, string ciphertext)
        {
            //return plaintext.IsEmpty() || ciphertext.IsEmpty() ? false : ToMd5(plaintext).Equals(ciphertext);
            return !plaintext.IsEmpty() && !ciphertext.IsEmpty() && StringExt.CompareIgnoreCase(ToMd5(plaintext), ciphertext);
        }
    }
}
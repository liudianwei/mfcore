using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MF.Authorization
{
    public class AES_EnorDecrypt
    {
        //定义默认密钥
        private static byte[] _aesKeyByte = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static string _aesKeyStr = Encoding.UTF8.GetString(_aesKeyByte);

        /// <summary>
        /// 随机生成密钥，默认密钥长度为32，不足在加密时自动填充空格
        /// </summary>
        /// <param name="n">密钥长度</param>
        /// <returns></returns>
        public static string GetIv(int n)
        {
            string s = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] arrChar = new char[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                arrChar[i] = Convert.ToChar(s.Substring(i, 1));
            }
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            _aesKeyByte = Encoding.UTF8.GetBytes(num.ToString());
            return _aesKeyStr = Encoding.UTF8.GetString(_aesKeyByte);
        }

        /// <summary>
        /// AES加密，针对文本类文件
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">密钥向量</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string Data, string Key, string Vector)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(Data);
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] Cryptograph = null;//加密后的密文
            Aes Aes = Aes.Create();
            try
            {
                using (MemoryStream Memory = new MemoryStream())
                {
                    //把内存流对象包装成加密流对象
                    using (CryptoStream Encryptor = new CryptoStream(Memory, Aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write))
                    {
                        Encryptor.Write(plainBytes, 0, plainBytes.Length);
                        Encryptor.FlushFinalBlock();
                        Cryptograph = Memory.ToArray();
                    }
                }
            }
            catch
            {
                Cryptograph = null;
            }
            return Convert.ToBase64String(Cryptograph);
        }

        /// <summary>
        /// AES加密，任意文件
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">密钥向量</param>
        /// <returns>密文</returns>
        public static byte[] AESEncrypt(byte[] Data, string Key, string Vector)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] Cryptograph = null;//加密后的密文
            Aes Aes = Aes.Create();
            try
            {
                using (MemoryStream Memory = new MemoryStream())
                {
                    //把内存流对象包装成加密流对象
                    using (CryptoStream Encryptor = new CryptoStream(Memory, Aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write))
                    {
                        Encryptor.Write(Data, 0, Data.Length);
                        Encryptor.FlushFinalBlock();
                        Cryptograph = Memory.ToArray();
                    }
                }
            }
            catch
            {
                Cryptograph = null;
            }
            return Cryptograph;
        }

        /// <summary>
        /// AES解密，针对文本文件
        /// </summary>
        /// <param name="Data">被解密的密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">密钥向量</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(string Data, string Key, string Vector)
        {
            byte[] encryptedBytes = Convert.FromBase64String(Data);
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] original = null;//解密后的明文
            Aes Aes = Aes.Create();
            try
            {
                using (MemoryStream Memory = new MemoryStream(encryptedBytes))
                {
                    //把内存流对象包装成加密对象
                    using (CryptoStream Decryptor = new CryptoStream(Memory, Aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read))
                    {
                        //明文存储区
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            byte[] Buffer = new byte[1024];
                            int readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                originalMemory.Write(Buffer, 0, readBytes);
                            }
                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }
            return Encoding.UTF8.GetString(original);
        }

        /// <summary>
        /// AES解密，任意文件
        /// </summary>
        /// <param name="Data">被解密的密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">密钥向量</param>
        /// <returns>明文</returns>
        public static byte[] AESDecrypt(byte[] Data, string Key, string Vector)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] original = null;//解密后的明文
            Aes Aes = Aes.Create();
            try
            {
                using (MemoryStream Memory = new MemoryStream(Data))
                {
                    //把内存流对象包装成加密对象
                    using (CryptoStream Decryptor = new CryptoStream(Memory, Aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read))
                    {
                        //明文存储区
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            byte[] Buffer = new byte[1024];
                            int readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                originalMemory.Write(Buffer, 0, readBytes);
                            }
                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                original = null;
            }
            return original;
        }

        private static string _aesKeyVector = "ames_=R/*33macroinf";
        /// <summary>
        ///  AES加密
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="password"></param>
        public static void AESEncryptFile(string inFile, string outFile, string password)
        {
            string KeyVector = _aesKeyVector;//密钥向量
            #region   加密
            FileStream sr = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            FileStream sw = new FileStream(outFile, FileMode.Create, FileAccess.Write);

            if (sr.Length > 50 * 1024 * 1024)//如果文件大于50M，采取分块加密，按50MB读写
            {
                byte[] mybyte = new byte[52428800];//每50MB加密一次                  
                int numBytesRead = 52428800;//每次加密的流大小
                long leftBytes = sr.Length;//剩余需要加密的流大小
                long readBytes = 0;//已经读取的流大小
                //每50MB加密后会变成50MB+16B
                byte[] encrpy = new byte[52428816];
                while (true)
                {
                    if (leftBytes > numBytesRead)
                    {
                        sr.Read(mybyte, 0, mybyte.Length);
                        encrpy = AESEncrypt(mybyte, password, KeyVector);
                        sw.Write(encrpy, 0, encrpy.Length);
                        leftBytes -= numBytesRead;
                        readBytes += numBytesRead;
                    }
                    else//重新设定读取流大小，避免最后多余空值
                    {
                        byte[] newByte = new byte[leftBytes];
                        sr.Read(newByte, 0, newByte.Length);
                        byte[] newWriteByte;
                        newWriteByte = AESEncrypt(newByte, password, KeyVector);
                        sw.Write(newWriteByte, 0, newWriteByte.Length);
                        readBytes += leftBytes;
                        break;
                    }
                }
            }
            else
            {
                byte[] mybyte = new byte[sr.Length];
                sr.Read(mybyte, 0, (int)sr.Length);
                mybyte = AESEncrypt(mybyte, password, KeyVector);
                sw.Write(mybyte, 0, mybyte.Length);
            }

            sr.Close();
            sw.Close();

            #endregion
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        /// <param name="password"></param>
        public static void AESDecryptFile(string inFile, string outFile, string password)
        {
            string KeyVector = _aesKeyVector;//密钥向量

            #region  解密
            FileStream sr = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            FileStream sw = new FileStream(outFile, FileMode.Create, FileAccess.Write);

            if (sr.Length > 50 * 1024 * 1024)//如果文件大于50M，采取分块解密，按50MB读写
            {
                byte[] mybyte = new byte[52428816];//解密缓冲区50MB+16B
                byte[] decrpt = new byte[52428800];//解密后的50MB
                int numBytesRead = 52428816;//每次解密的流大小
                long leftBytes = sr.Length;//剩余需要解密的流大小
                long readBytes = 0;//已经读取的流大小
                try
                {
                    while (true)
                    {
                        if (leftBytes > numBytesRead)
                        {
                            sr.Read(mybyte, 0, mybyte.Length);
                            decrpt = AESDecrypt(mybyte, password, KeyVector);
                            sw.Write(decrpt, 0, decrpt.Length);
                            leftBytes -= numBytesRead;
                            readBytes += numBytesRead;
                        }
                        else//重新设定读取流大小，避免最后多余空值
                        {
                            byte[] newByte = new byte[leftBytes];
                            sr.Read(newByte, 0, newByte.Length);
                            byte[] newWriteByte;
                            newWriteByte = AESDecrypt(newByte, password, KeyVector);
                            sw.Write(newWriteByte, 0, newWriteByte.Length);
                            readBytes += leftBytes;
                            break;
                        }
                    }
                }
                catch
                {
                    sr.Close();
                    sw.Close();
                    File.Delete(outFile);
                    throw new Exception("authorization faild");
                }
                sr.Close();
                sw.Close();
            }
            else
            {
                byte[] mybyte = new byte[(int)sr.Length];
                sr.Read(mybyte, 0, (int)sr.Length);
                try
                {
                    mybyte = AESDecrypt(mybyte, password, KeyVector);
                    sw.Write(mybyte, 0, mybyte.Length);
                }
                catch
                {
                    sr.Close();
                    sw.Close();
                    File.Delete(outFile);
                    throw new Exception("authorization faild");
                }
                sr.Close();
                sw.Close();
            }
            #endregion
        }
    }

}

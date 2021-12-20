using System;
using System.IO;
using MF.Core.Check;
using MF.Core.Delegate;
using MF.Core.Extensions;
using MF.Core.Security;

namespace MF.Core.Files
{
    public static class FileUtil
    {
        /// <summary>
        ///判断文件是否支持读写操作
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns>占用返回false，反之返回true<</returns>
        public static bool IsFileCanWrite(string fileName)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Write);
                return fs.CanWrite;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// 判断文件是否存在，不存在则创建
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreateIfNotExists(string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
        }

        public static void Delete(string filePath)
        {
            CheckNull.ArgumentIsNullException(filePath, nameof(filePath));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static string ReadFileFromPath(string path, EncodingType type = EncodingType.UTF8)
        {
            string line = "";
            return DelegateUtil.TryExecute(() =>
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path, BytesExt.GetEncoding(type));
                    if (!sr.EndOfStream)
                    {
                        line = sr.ReadToEnd();
                        sr.Close();
                        return line;
                    }
                }
                return line;
            }, line);
        }

        public static byte[] ReadFileToBytes(string path)
        {
            byte[] bytes = null;
            if (File.Exists(path))
            {
                FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Close();
            }
            return bytes;
        }

        public static string ImgToBase64String(string path)
        {
            string res = "";
            return DelegateUtil.TryExecute(() =>
             {
                 return Convert.ToBase64String(ReadFileToBytes(path));
             }, res);
        }

        public static void Save(byte[] bytes, string filePath)
        {
            using FileStream stream = File.Create(filePath);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        public static string FileMd5(string filePath)
        {
            string md5 = "";
            if (IsExists(filePath))
            {
                using Stream input = File.OpenRead(filePath);
                md5 = input.ToMd5();
            }
            return md5;
        }

        public static int GetBytes(string unit = "KB", int chunkSize = 1024)
        {
            int size = unit switch
            {
                "B" => chunkSize,
                "KB" => chunkSize * 1024,
                "MB" => chunkSize * 1024 * 1024,
                "GB" => chunkSize * 1024 * 1024 * 1024,
                _ => chunkSize * 1024 * 1024,
            };
            return size;
        }

        public static long GetBytes(long bytes, string unit = "MB")
        {
            long size = unit switch
            {
                "B" => bytes,
                "KB" => bytes / 1024,
                "MB" => bytes / 1024 / 1024,
                "GB" => bytes / 1024 / 1024 / 1024,
                _ => bytes / 1024 / 1024,
            };
            return size;
        }
    }
}
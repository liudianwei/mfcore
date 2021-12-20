using System;
using System.IO;
using MF.Core.Check;

namespace MF.Core.Files
{
    public static class DirectoryUtil
    {
        public static bool IsExists(string directory)
        {
            return Directory.Exists(directory);
        }

        public static void CreateIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        /// <param name="directory"></param>
        public static string CreateDirectoryByDate(string format = "yyyy-MM-dd", string slpit = "-")
        {
            var filePath = DateTime.Now.ToString(format).Split(slpit);
            var actual = Path.Combine(filePath);
            CreateIfNotExists(actual);
            return actual;
        }

        public static bool Delete(string directory, bool isDeleteRoot = true)
        {
            CheckNull.ArgumentIsNullException(directory, nameof(directory));
            var flag = false;
            var dirPathInfo = new DirectoryInfo(directory);
            if (dirPathInfo.Exists)
            {
                //删除目录下所有文件
                foreach (var fileInfo in dirPathInfo.GetFiles())
                {
                    fileInfo.Delete();
                }
                //递归删除所有子目录
                foreach (var subDirectory in dirPathInfo.GetDirectories())
                {
                    Delete(subDirectory.FullName);
                }
                //删除目录
                if (isDeleteRoot)
                {
                    dirPathInfo.Attributes = FileAttributes.Normal;
                    dirPathInfo.Delete();
                }
                flag = true;
            }
            return flag;
        }
    }
}
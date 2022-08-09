using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using MF.Utils.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace MF.Utils
{
    /// <summary>
    /// 文件操作类 dtj
    /// </summary>
    public class FileHelper
    {
        #region 解压缩文件
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationZipFilePath"></param>
        public static void CreateZip(string sourceFilePath, string destinationZipFilePath)
        {
            if (sourceFilePath[sourceFilePath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                sourceFilePath += System.IO.Path.DirectorySeparatorChar;

            ZipOutputStream zipStream = new ZipOutputStream(File.Create(destinationZipFilePath));
            zipStream.SetLevel(6);  // 压缩级别 0-9
            CreateZipFiles(sourceFilePath, zipStream, sourceFilePath);

            zipStream.Finish();
            zipStream.Close();
        }

        /// <summary>
        /// 递归压缩文件
        /// </summary>
        /// <param name="sourceFilePath">待压缩的文件或文件夹路径</param>
        /// <param name="zipStream">打包结果的zip文件路径（类似 D:\WorkSpace\a.zip）,全路径包括文件名和.zip扩展名</param>
        /// <param name="staticFile"></param>
        private static void CreateZipFiles(string sourceFilePath, ZipOutputStream zipStream, string staticFile)
        {
            Crc32 crc = new Crc32();
            string[] filesArray = Directory.GetFileSystemEntries(sourceFilePath);
            foreach (string file in filesArray)
            {
                if (Directory.Exists(file)) //如果当前是文件夹，递归
                {
                    CreateZipFiles(file, zipStream, staticFile);
                }
                else //如果是文件，开始压缩
                {
                    FileStream fileStream = File.OpenRead(file);

                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    string tempFile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempFile);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fileStream.Length;
                    fileStream.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zipStream.PutNextEntry(entry);

                    zipStream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        #endregion

        #region gofastdfs文件服务器上传、下载、删除等操作
        /// <summary>
        /// 抓取指定的url的网络图片，并保存到本地
        /// </summary>
        /// <param name="netUrl"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        static public Bitmap Get_img(string netUrl, string savePath)
        {
            string path = Path.GetDirectoryName(savePath);
            if (false == Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Bitmap img = null;
            HttpWebRequest req;
            HttpWebResponse res = null;
            try
            {
                System.Uri httpUrl = new System.Uri(netUrl);//new System.Uri("http://xxx.com/xxx.png");
                req = (HttpWebRequest)(WebRequest.Create(httpUrl));
                req.Timeout = 180000; //设置超时值10秒
                req.UserAgent = "XXXXX";
                req.Accept = "XXXXXX";
                req.Method = "GET";
                res = (HttpWebResponse)(req.GetResponse());

                img = new Bitmap(res.GetResponseStream());//获取图片流
                img.Save(savePath);
            }
            catch (Exception ex)
            {
                string aa = ex.Message;
            }
            finally
            {
                res.Close();
            }
            return img;
        }
        /// <summary>
        /// 上传图片到文件服务器
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="url"></param>
        static public void Director(string dir, string url)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            if (Directory.Exists(d.FullName))
            {
                FileInfo[] files = d.GetFiles();//文件
                DirectoryInfo[] directs = d.GetDirectories();//文件夹

                foreach (FileInfo f in files)
                {
                    var request = new RestRequest($"{url}/upload", Method.POST);
                    request.AddParameter("output", "json");
                    request.AddParameter("scene", "image");
                    request.AddParameter("path", $"/{d.Name}");
                    var path = $"{dir}\\{f.Name}";
                    request.AddFile("file", path);
                    var restClient = new RestClient();
                    var res = restClient.ExecuteAsync(request).Result;
                    //if (res.StatusCode == HttpStatusCode.OK)
                    //{
                    //}
                }
                //获取子文件夹内的文件列表，递归遍历  
                foreach (DirectoryInfo dd in directs)
                {
                    Director(dd.FullName, url);
                }

            }
        }

        /// <summary>
        /// 获取文件服务器cdn列表
        /// </summary>
        static public List<DataResult> List_dir(string url)
        {
            var request = new RestRequest($"{url}/list_dir?dir=cdn", Method.GET);
            var restClient = new RestClient();
            var res = restClient.ExecuteAsync(request).Result;
            if (res.StatusCode == HttpStatusCode.OK)
            {
                var r1 = res.Content?.ToObj<GofastResult>();
                if (r1.Status=="")
                {
                    throw new Exception($"{r1.Message}");
                }
                return r1.Data?.ToJson().ToObj<List<DataResult>>();
            }
            else
            {
                throw new Exception($"{BaseSystemError.CONNECTION_FAILED}:{res}");
            }
        }
        /// <summary>
        /// 删除在线文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="md5"></param>
        static public bool RemoveOnlineFile(string url, string md5)
        {
            bool bl = false;
            var request = new RestRequest($"{url}/delete?md5={md5}", Method.GET);
            var restClient = new RestClient();
            var res = restClient.ExecuteAsync(request).Result;
            if (res.StatusCode == HttpStatusCode.OK)
            {
                bl = true;
            }
            return bl;
        }
        /// <summary>
        /// 上传图片到文件服务器
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="url"></param>
        static public void UploadFile(string dir, string url)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            if (Directory.Exists(d.FullName))
            {
                FileInfo[] files = d.GetFiles();//文件
                foreach (FileInfo f in files)
                {
                    var request = new RestRequest($"{url}/upload", Method.POST);
                    request.AddParameter("output", "json");
                    request.AddParameter("path", $"/cdn");
                    var path = $"{dir}\\{f.Name}";
                    request.AddFile("file", path);
                    var restClient = new RestClient();
                    var res = restClient.ExecuteAsync(request).Result;
                    if (res.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception($"{BaseSystemError.CONNECTION_FAILED}:{res}");
                    }
                }
            }
        }
        #endregion

        #region 文件流处理以及读写json文件
        /// <summary>
        /// 流转文件
        /// </summary>
        public static void StreamToFile(Stream stream, string fileName)
        {
            string path = Path.GetDirectoryName(fileName);
            if (false == Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // 把 Stream 转换成 byte[]
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 写入文件

            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// 从文件读取 Stream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 写json文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="jsonConents"></param>
        public static void WriteJsonFile(string path, string fileName, string jsonConents)
        {
            if (false == Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var OutFullPath = $"{path}/{fileName}";

            try
            {

                using (FileStream fs = new FileStream(OutFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.SetLength(0);
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine(jsonConents);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("fail to write to file");
            }
        }

        /// <summary>
        /// 获取到本地的Json文件并且解析返回对应的json字符串
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>Json内容</returns>
        public static string GetJsonFile(string filepath)
        {
            string json = "{}";
            if (File.Exists(filepath))
            {
                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        json = sr.ReadToEnd().ToString();
                    }
                }
            }
            return json;
        }

        #endregion

        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir"></param> 
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (System.IO.File.Exists(d))
                        System.IO.File.Delete(d); //直接删除其中的文件                       
                    else
                        DeleteFolder(d); //递归删除子文件夹
                }
                Directory.Delete(dir, true); //删除已空文件夹                
            }
        }

        #region DTO实体对象
        /// <summary>
        /// 文件服务返回结果
        /// </summary>
        public class GofastResult
        {
            public object Data
            {
                get;
                set;
            }
            public string Message
            {
                get;
                set;
            }
            public string Status
            {
                get;
                set;
            }
        }
        
        /// <summary>
        /// 文件服务器文件列表实体
        /// </summary>
        public class DataResult
        {
            public string md5
            {
                get;
                set;
            }
            public string name
            {
                get;
                set;
            }
        }
        
        /// <summary>
        /// 图片解析实体
        /// </summary>
        public class ImageObejct
        {
            public string BackgroundImage
            {
                get;
                set;
            }
            public string ImageAdress
            {
                get;
                set;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Runtime.InteropServices;

using MF.Core.Extensions;
using MF.Utils;
using Microsoft.Win32;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using LicenseGen;
using System.Globalization;
using System.Security.Cryptography;

namespace MF.NetCoreApp
{
    public class Esnecil
    {
        private string publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC8uoD6rE9HGYm7vYnvhJo8ckXj"
                                + "+Suioa0NXn0AVs+N4+I/wY161wOe+Q0kHxGrruZ9KmOpoDkabvXsha8/seo7kO1z"
                                + "A0oorFD7eBZrZ7IUo07Fot5P0HdRkWm5irBOJ+XSrJahf3XQexX9g2VUnWw3nsK/"
                                + "SOLptIUVxJO0pe3UiwIDAQAB";

        //private string licenseFile = "";
        private readonly Dictionary<string, string> licenseArr;

        private LicenseInfo license;
        private bool isLicensed = false;

        public string[] licKeys = new string[] {
            "Signature",
            "Product",
            "LicenseId",
            "UserName",
            "UserEmail",
            "Activation",
            "Expire",
            "Edition",
            "Version",
            "MaxLicensedDisplays"
        };

        public Esnecil()
        {
            //判断文件时候可读
            //if (File.Exists(licenseFilePath))
            //{
            //    throw new Exception("没有找到许可文件");
            //}
            //读取授权文件

            //判断字段缺少 没有就报错 对应的属性没有设置

            //
            licenseArr = null;
        }

        public bool ValidateLicense()
        {
            //签名数据
            if (!licenseArr.TryGetValue("Signature", out var sig))
            {
                return false;
            }

            licenseArr.Remove("Signature");
            var str = "";

            foreach (var item in licenseArr)
            {
                str += $"{item.Key}={item.Value}";
            }

            bool result = Verify(str, Convert.FromBase64String(sig).ToString(), publicKey);
            isLicensed = result;
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="base64"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        private bool Verify(string str, string base64, string publicKey)
        {
            throw new NotImplementedException();
        }

        private string licensePath = "";
        //private readonly string MachineId;

        public static char emblem = 'm';//密钥


        [Serializable]
        public class License
        {
            public string company = "";
            public string signal = "";
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <param name="key">8位字符的密钥字符串</param>
        /// <param name="iv">8位字符的初始化向量字符串</param>
        /// <returns></returns>
        public static string DESEncrypt(string data, string key, string iv)
        {
            byte[] byKey = ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = ASCIIEncoding.ASCII.GetBytes(iv);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param>
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static string DESDecrypt(string data, string key, string iv)
        {
            byte[] byKey = ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = ASCIIEncoding.ASCII.GetBytes(iv);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 校验是否授权
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Check()
        {
            licensePath = Environment.CurrentDirectory + "\\license.lic";
            if (File.Exists(licensePath))
            {
                string datetimeFormat = "yyyyMMdd HH:mm:ss";

                //var bytes = File.ReadAllBytes(licensePath);
                //var obj = Bytes2Object(bytes);
                //if (obj is Dictionary<string, string>)
                //{
                //    var dic = obj as Dictionary<string, string>;
                //    license = new LicenseInfo()
                //    {
                //        Product = dic["Product"],
                //        MachineId = dic["MachineId"],
                //        Company = dic["Company"],
                //        StartDateTime = dic["StartDateTime"],
                //        EndDateTime = dic["EndDateTime"],
                //        Sig = dic["Sig"]
                //    };
                //}
                //else
                //{
                //    return false;
                //}
                FileStream fileStream = new FileStream(licensePath, FileMode.OpenOrCreate, FileAccess.Read);

                //创建二进制写入流的实例
                BinaryReader br = new BinaryReader(fileStream);

                //向文件中写入图书名称
                license = new LicenseInfo
                {
                    Product = br.ReadString(),
                    MachineId = br.ReadString(),
                    StartDateTime = br.ReadString(),
                    EndDateTime = br.ReadString(),
                    Company = br.ReadString(),
                    Sig = br.ReadString()
                };
                br.Close();

                fileStream.Close();

                if (license.Product != "AMES-WebApi")
                {
                    license.Product = "";
                    return false;
                }

                if (license.MachineId != MachineCode.GetMachineCodeString())
                {
                    return false;
                }

                if (license.Company == "")
                {
                    return false;
                }

                if (DateTime.Compare(DateTime.Now, DateTime.ParseExact(license.EndDateTime, datetimeFormat, CultureInfo.CurrentCulture)) > 0)
                {
                    return false;
                }

                if (license.Sig == "")
                {
                    return false;
                }
                else
                {
                    string str = $"Product={license.Product}";
                    str += $"MachineId={license.MachineId}";
                    str += $"StartDateTime={license.StartDateTime}";
                    str += $"EndDateTime={license.EndDateTime}";
                    str += $"Company={license.Company}";

                    if (!RSAUtils.Verify(str, license.Sig, publicKey, "UTF-8"))
                    {
                        return false;
                    }
                }
            }
            else
            {
                //只有windows才能使用注册表
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return false;
                }

                //检查注册表是否过期
                RegistryKey mainkey = Registry.LocalMachine;
                RegistryKey subkey = mainkey.OpenSubKey("SOFTWARE\\AMES\\time", true);

                if (subkey == null)
                {
                    var last = DateTime.Now.AddDays(30);
                    var usetime = DESEncrypt(last.ToLongDateString(), "MACROINF", "macroinf");
                    subkey = mainkey.CreateSubKey("SOFTWARE\\AMES\\time");
                    subkey.SetValue("Usetime", usetime);
                }
                else
                {
                    try
                    {
                        DateTime usetime = Convert.ToDateTime(DESDecrypt(subkey.GetValue("Usetime").ToString(), "MACROINF", "macroinf"));
                        DateTime daytime = DateTime.Parse(DateTime.Now.ToLongDateString());
                        TimeSpan ts = usetime - daytime;
                        int day = ts.Days;
                        if (day <= 0)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        if (subkey == null)
                        {
                            subkey.Close();
                        }
                    }
                }
            }
            return true;
        }
    }
}
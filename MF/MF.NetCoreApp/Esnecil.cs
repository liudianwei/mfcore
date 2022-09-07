using LicenseGen;
using MF.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
                return false;


                //只有windows才能使用注册表
                //if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                //{
                //    return false;
                //}

                //检查注册表是否过期
                //RegistryKey mainkey = Registry.LocalMachine;
                //RegistryKey subkey = mainkey.OpenSubKey("SOFTWARE\\AMES\\time", true);

                //if (subkey == null)
                //{
                //    var last = DateTime.Now.AddDays(30);
                //    var usetime = DESEncrypt(last.ToLongDateString(), "MACROINF", "macroinf");
                //    subkey = mainkey.CreateSubKey("SOFTWARE\\AMES\\time");
                //    subkey.SetValue("Usetime", usetime);
                //}
                //else
                //{
                //    try
                //    {
                //        DateTime usetime = Convert.ToDateTime(DESDecrypt(subkey.GetValue("Usetime").ToString(), "MACROINF", "macroinf"));
                //        DateTime daytime = DateTime.Parse(DateTime.Now.ToLongDateString());
                //        TimeSpan ts = usetime - daytime;
                //        int day = ts.Days;
                //        if (day <= 0)
                //        {
                //            return false;
                //        }
                //    }
                //    catch
                //    {
                //        return false;
                //    }
                //    finally
                //    {
                //        if (subkey == null)
                //        {
                //            subkey.Close();
                //        }
                //    }
                //}
            }
            return true;
        }

        public (bool, string) CheckMisdataCr(string strMachineCode)
        {

            var templicense = Environment.CurrentDirectory + "\\license.lic";
            var tempPath = Environment.CurrentDirectory + "\\abcd.data";
            if (System.IO.File.Exists(templicense))
            {
                string pubkey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDEx9SqZNCxGH62rqcAynDrynCn"
                                + "xQ5WxYJmGpeMkgk82mgu1TBwAF1sfLWh8vwLuH2JA3XgdS+TjDzdOLLxN/UAGrAz"
                                + "KJ/onVjj7++MQnSXb1LRUwP4w3iEun2Ldt5MvMIu/duj8qXNoOnJcKxc0SfoE0mU"
                                + "YFHVD5AvsrPvOf9UgQIDAQAB";

                string datetimeFormat = "yyyyMMdd HH:mm:ss";

                DateTime StartDateTime;

                DecryptFile(templicense, tempPath, "TEST_PASSWORD_~!@#");
                FileStream fileStream = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Read);

                //创建二进制写入流的实例
                BinaryReader br = new BinaryReader(fileStream);

                //向文件中写入
                var info = new LicenseInfo
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

                File.Delete(tempPath);


                if (info.Product != null && info.Product != "AMES-Misdata")
                {
                    return (false, "invalid product");
                }

                if (info.MachineId != null && info.MachineId != strMachineCode)
                {
                    return (false, "invalid machine code");
                }

                if (info.Company != null && info.Company == "")
                {
                    return (false, "invalid company code");
                }

                if (info.StartDateTime != null && info.StartDateTime != "")
                {
                    try
                    {
                        StartDateTime = DateTime.ParseExact(info.StartDateTime, datetimeFormat, CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        return (false, "invalid start time");
                    }
                }
                else
                {
                    return (false, "start time  cannot be empty");
                }

                if (info.EndDateTime != null && info.EndDateTime != "")
                {
                    try
                    {
                        var EndDateTime = DateTime.ParseExact(info.EndDateTime, datetimeFormat, CultureInfo.CurrentCulture);

                        if (DateTime.Compare(StartDateTime, EndDateTime) > 0)
                        {
                            return (false, "authorization has expired");
                        }

                        if (DateTime.Compare(DateTime.Now, EndDateTime) > 0)
                        {
                            return (false, "authorization has expired");
                        }
                    }
                    catch
                    {
                        return (false, "authorization has expired");
                    }
                }
                else
                {
                    return (false, "authorization has expired");
                }

                if (info.Sig != null && info.Sig != "")
                {
                    string str = $"Product={info.Product}";
                    str += $"MachineId={info.MachineId}";
                    str += $"StartDateTime={info.StartDateTime}";
                    str += $"EndDateTime={info.EndDateTime}";
                    str += $"Company={info.Company}";

                    if (!RSAUtils.Verify(str, info.Sig, pubkey, "UTF-8"))
                    {
                        return (false, "signature expired");
                    }
                }
                else
                {
                    return (false, "signature cannot be empty");
                }
            }
            else
            {
                return (false, "authorization file does not exist");
            }
            return (true, "");
        }
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inFile">待解密文件</param>
        /// <param name="outFile">解密后输出文件</param>
        /// <param name="password">解密密码</param>
        public static void DecryptFile(string inFile, string outFile, string password)
        {
            // 创建打开文件流
            using (FileStream fin = System.IO.File.OpenRead(inFile),
                fout = System.IO.File.OpenWrite(outFile))
            {
                int size = (int)fin.Length;
                int BUFFER_SIZE = 128 * 1024;
                byte[] bytes = new byte[BUFFER_SIZE];
                int read = -1;
                int value = 0;
                int outValue = 0;
                byte[] IV = new byte[16];
                fin.Read(IV, 0, 16);
                byte[] salt = new byte[16];
                fin.Read(salt, 0, 16);

                SymmetricAlgorithm sma = CreateRijndael(password, salt);
                sma.IV = IV;
                value = 32;
                long lSize = -1;

                // 创建散列对象, 校验文件
                HashAlgorithm hasher = SHA256.Create();
                using (CryptoStream cin = new CryptoStream(fin, sma.CreateDecryptor(), CryptoStreamMode.Read),
                    chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                {
                    // 读取文件长度
                    BinaryReader br = new BinaryReader(cin);
                    lSize = br.ReadInt64();
                    ulong tag = br.ReadUInt64();

                    ulong FC_TAG = 0xFC010203040506CF;
                    if (FC_TAG != tag)
                        throw new Exception("文件被破坏");

                    long numReads = lSize / BUFFER_SIZE;
                    long slack = (long)lSize % BUFFER_SIZE;

                    for (int i = 0; i < numReads; ++i)
                    {
                        read = cin.Read(bytes, 0, bytes.Length);
                        fout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                        outValue += read;
                    }
                    if (slack > 0)
                    {
                        read = cin.Read(bytes, 0, (int)slack);
                        fout.Write(bytes, 0, read);
                        chash.Write(bytes, 0, read);
                        value += read;
                        outValue += read;
                    }
                    chash.Flush();
                    chash.Close();
                    fout.Flush();
                    fout.Close();
                    byte[] curHash = hasher.Hash;
                    // 获取比较和旧的散列对象
                    byte[] oldHash = new byte[hasher.HashSize / 8];
                    read = cin.Read(oldHash, 0, oldHash.Length);
                    if ((oldHash.Length != read) || (!CheckByteArrays(oldHash, curHash)))
                        throw new Exception("文件被破坏");
                }

                if (outValue != lSize)
                    throw new Exception("文件大小不匹配");
            }
        }
        /// <summary>
        /// 检验两个Byte数组是否相同
        /// </summary>
        /// <param name="b1">Byte数组</param>
        /// <param name="b2">Byte数组</param>
        /// <returns>true－相等</returns>
        private static bool CheckByteArrays(byte[] b1, byte[] b2)
        {
            if (b1.Length == b2.Length)
            {
                for (int i = 0; i < b1.Length; ++i)
                {
                    if (b1[i] != b2[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建Rijndael SymmetricAlgorithm
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="salt"></param>
        /// <returns>加密对象</returns>
        private static SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);

            SymmetricAlgorithm sma = Rijndael.Create();
            sma.KeySize = 256;
            sma.Key = pdb.GetBytes(32);
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }
    }
}
/*
 * @Author: chenlingbin
 * @Date: 2020-11-12 11:34:00
 * @Last Modified by: chenlingbin
 * @Last Modified time: 2021-02-18 22:37:00
 */

using DeviceId;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace LicenseCore
{
    /// <summary>
    /// 授权帮助类
    /// </summary>
    public partial class LicenseHelper
    {
        /// <summary>
        /// 机器码
        /// </summary>
        private static string machineCodeString = "";

        /// <summary>
        /// 获取机器码
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCodeString()
        {
            if (machineCodeString != "")
            {
                return machineCodeString;
            }
            machineCodeString = new DeviceIdBuilder()
            .AddMachineName()
            .AddOsVersion()
            .OnWindows(windows => windows
                .AddProcessorId()
                .AddMotherboardSerialNumber()
                .AddSystemDriveSerialNumber())
            .ToString();
            return machineCodeString;
        }

        /// <summary>
        /// 检查授权
        /// </summary>
        /// <param name="machineCode"></param>
        /// <param name="product"></param>
        /// <param name="msg"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool Check(string machineCode, string product, out string msg, out LicenseInfo info)
        {
            bool flag = false;
            msg = "";
            info = new LicenseInfo();
            var templicense = Environment.CurrentDirectory + "\\license.lic";
            var tempPath = Environment.CurrentDirectory + "\\abcd.data";
            string datetimeFormat = "yyyyMMdd HH:mm:ss";
            DateTime StartDateTime = DateTime.Now;
            DateTime EndDateTime = DateTime.Now;
            if (File.Exists(templicense))
            {
                if (!pubkeys.TryGetValue(product, out string pubkey))
                {
                    msg = "ProductKey unknown";
                    return flag;
                }

                DecryptFile(templicense, tempPath, "TEST_PASSWORD_~!@#");
                FileStream fileStream = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Read);

                //创建二进制写入流的实例
                BinaryReader br = new BinaryReader(fileStream);

                //向文件中写入
                info = new LicenseInfo
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

                if (info.Product != null && info.Product != product)
                {
                    msg = "Product invalid";
                    return flag;
                }

                if (info.MachineId != null && info.MachineId != machineCode)
                {
                    msg = "MachineId invalid";
                    return flag;
                }

                if (info.Company != null && info.Company == "")
                {
                    msg = "Company invalid";
                    return flag;
                }

                if (info.StartDateTime != null && info.StartDateTime != "")
                {
                    try
                    {
                        StartDateTime = DateTime.ParseExact(info.StartDateTime, datetimeFormat, CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        msg = "StartDateTime invalid";
                        return flag;
                    }
                }
                else
                {
                    msg = "StartDateTime is null";
                    return flag;
                }

                if (info.EndDateTime != null && info.EndDateTime != "")
                {
                    try
                    {
                        EndDateTime = DateTime.ParseExact(info.EndDateTime, datetimeFormat, CultureInfo.CurrentCulture);

                        if (DateTime.Compare(StartDateTime, EndDateTime) > 0)
                        {
                            msg = "license expired";
                            return flag;
                        }

                        if (DateTime.Compare(DateTime.Now, EndDateTime) > 0)
                        {
                            msg = "license expired";
                            return flag;
                        }
                    }
                    catch
                    {
                        msg = "license expired";
                        return flag;
                    }
                }
                else
                {
                    msg = "license expired";
                    return flag;
                }

                if (info.Sig != null && info.Sig != "")
                {
                    string str = $"Product={info.Product}";
                    str += $"MachineId={info.MachineId}";
                    str += $"StartDateTime={info.StartDateTime}";
                    str += $"EndDateTime={info.EndDateTime}";
                    str += $"Company={info.Company}";

                    if (!Verify(str, info.Sig, pubkey, "UTF-8"))
                    {
                        msg = "Sig expired";
                        return flag;
                    }
                }
                else
                {
                    msg = "Sig is null";
                    return flag;
                }
            }
            else
            {
                msg = "unauthorized";
                return flag;
            }
            flag = true;
            msg = $"许可证到期:{DateTime.ParseExact(info.EndDateTime, datetimeFormat, CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm")}";
            return flag;
        }

        /// <summary>
        /// 公钥集
        /// </summary>
        protected static Dictionary<string, string> pubkeys = new Dictionary<string, string>()
        {
            {
                "AMES-SCADA-CORE","MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDIvD9XGCrzKWKXkLuIoC+Y9KWV"
                    + "4oxbivDL9027paeFEUTKUPKOFhA91RPVC/+DuWSrgr/SeMDBTPm3GwhPJA27kHaC"
                    + "NAmbjucoijRrNB1WCKRSoOpQ6p903SEbLB/qlvmvJMNKRWIPOYHqo/Ai/nIAt7HC"
                    + "bMzxlS3u8TqNaDoPcQIDAQAB"
            },
            {   "AMES-Misdata","MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDEx9SqZNCxGH62rqcAynDrynCn"
                                + "xQ5WxYJmGpeMkgk82mgu1TBwAF1sfLWh8vwLuH2JA3XgdS+TjDzdOLLxN/UAGrAz"
                                + "KJ/onVjj7++MQnSXb1LRUwP4w3iEun2Ldt5MvMIu/duj8qXNoOnJcKxc0SfoE0mU"
                                + "YFHVD5AvsrPvOf9UgQIDAQAB"
            },
            {   "AMES-WebApi","MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC8uoD6rE9HGYm7vYnvhJo8ckXj"
                                + "+Suioa0NXn0AVs+N4+I/wY161wOe+Q0kHxGrruZ9KmOpoDkabvXsha8/seo7kO1z"
                                + "A0oorFD7eBZrZ7IUo07Fot5P0HdRkWm5irBOJ+XSrJahf3XQexX9g2VUnWw3nsK/"
                                + "SOLptIUVxJO0pe3UiwIDAQAB"
            }
        };

        #region 解密文件

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inFile">待解密文件</param>
        /// <param name="outFile">解密后输出文件</param>
        /// <param name="password">解密密码</param>
        public static void DecryptFile(string inFile, string outFile, string password)
        {
            // 创建打开文件流
            using (FileStream fin = File.OpenRead(inFile),
                fout = File.OpenWrite(outFile))
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
            // 填充无效,无法被移除的问题,禁用一下代码,从PKCS7改成None
            //sma.Padding = PaddingMode.PKCS7;
            sma.Padding = PaddingMode.None;
            return sma;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pempublicKey"></param>
        /// <returns></returns>
        internal static RSAParameters ConvertFromPublicKey(string pempublicKey)
        {
            byte[] keyData = Convert.FromBase64String(pempublicKey);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        #endregion 解密文件

        #region 验签函数

        /// <summary>
        /// 验签函数
        /// </summary>
        /// <param name="content"></param>
        /// <param name="signedString"></param>
        /// <param name="publicKey"></param>
        /// <param name="inputCharset"></param>
        /// <returns></returns>
        public static bool Verify(string content, string signedString, string publicKey, string inputCharset)
        {
            bool result = false;

            Encoding code = Encoding.GetEncoding(inputCharset);
            byte[] data = code.GetBytes(content);
            byte[] soureData = Convert.FromBase64String(signedString);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);
            SHA1 sh = new SHA1CryptoServiceProvider();
            result = rsaPub.VerifyData(data, sh, soureData);
            return result;
        }

        #endregion 验签函数

        /// <summary>
        /// 授权信息
        /// </summary>
        [Serializable]
        public class LicenseInfo
        {
            /// <summary>
            /// 产品名称
            /// </summary>
            public string Product { get; set; }

            /// <summary>
            /// 设备ID
            /// </summary>
            public string MachineId { get; set; }

            /// <summary>
            /// 授权开始时间
            /// </summary>
            public string StartDateTime { get; set; }

            /// <summary>
            /// 授权结束时间
            /// </summary>
            public string EndDateTime { get; set; }

            /// <summary>
            /// 授权公司
            /// </summary>
            public string Company { get; set; }

            /// <summary>
            /// 标识
            /// </summary>
            public string Sig { get; set; }
        }
    }
}
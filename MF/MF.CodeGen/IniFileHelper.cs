using MF.Authorization;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CodeGen
{
    public class IniFileHelper
    {
        private readonly string strIniFilePath;  // ini配置文件路径
        public readonly string CrMsg;  // ini配置文件路径

        // 返回0表示失败，非0为成功
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 返回取得字符串缓冲区的长度
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern long GetPrivateProfileString(string section, string key, string strDefault, StringBuilder retVal, int size, string filePath);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileInt(string section, string key, int nDefault, string filePath);

        /// <summary>
        /// 无参构造函数
        /// </summary>
        /// <returns></returns>
        public IniFileHelper()
        {
            this.strIniFilePath = Directory.GetCurrentDirectory() + "\\config.ini";
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="strIniFilePath">ini配置文件路径</param>
        /// <returns></returns>
        public IniFileHelper(string strIniFilePath)
        {
            if (strIniFilePath != null)
            {
                this.strIniFilePath = strIniFilePath;
            }
            try
            {
                var strMachineCode = MachineCode.GetMachineCodeString();
                Console.WriteLine($"机器码:{strMachineCode}");
                var item = CheckCodeGenCr(strMachineCode);
                if (!item.Item1)
                {
                    var msg = $"授权失败,请联系管理员进行授权!Warning Message ===>{item.Item2}；机器码为===>AMES-CodeGen:{strMachineCode}";
                    Console.WriteLine(msg);
                    throw new Exception(msg);
                }
                CrMsg = item.Item2;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取ini配置文件中的字符串
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="strDefault">默认值</param>
        /// <param name="retVal">结果缓冲区</param>
        /// <param name="size">结果缓冲区大小</param>
        /// <returns>成功true,失败false</returns>
        public bool GetIniString(string section, string key, string strDefault, StringBuilder retVal, int size)
        {
            long liRet = GetPrivateProfileString(section, key, strDefault, retVal, size, strIniFilePath);
            return (liRet >= 1);
        }

        /// <summary>
        /// 获取ini配置文件中的整型值
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="nDefault">默认值</param>
        /// <returns></returns>
        public int GetIniInt(string section, string key, int nDefault)
        {
            return GetPrivateProfileInt(section, key, nDefault, strIniFilePath);
        }

        /// <summary>
        /// 往ini配置文件写入字符串
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="val">要写入的字符串</param>
        /// <returns>成功true,失败false</returns>
        public bool WriteIniString(string section, string key, string val)
        {
            long liRet = WritePrivateProfileString(section, key, val, strIniFilePath);
            return (liRet != 0);
        }

        /// <summary>
        /// 往ini配置文件写入整型数据
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="val">要写入的数据</param>
        /// <returns>成功true,失败false</returns>
        public bool WriteIniInt(string section, string key, int val)
        {
            return WriteIniString(section, key, val.ToString());
        }

        /// <summary>
        /// 代码生成器授权
        /// </summary>
        /// <param name="strMachineCode"></param>
        /// <returns></returns>
        public (bool, string) CheckCodeGenCr(string strMachineCode)
        {
            var templicense = Environment.CurrentDirectory + "\\license.lic";
            var tempPath = Environment.CurrentDirectory + "\\abcd.data";
            string expiredTime = "";
            if (File.Exists(templicense))
            {
                string pubkey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDa4loF1nxk1lWPbvCllU10XB3d"
                                + "CNtY5GgxMLKbEO2O18PeQrPRzfit8lokPvC/2D4CFG7OqqVzdMKj1DzZYsy+4Y3o"
                                + "ACx8q9iCiyFIsDlYBOs9a/Zi6ViPRltHyHjhAgT8LilrKhRBsmATKwqNhqw2QBks"
                                + "wCFG0zt5Y2HSJ51NdwIDAQAB";

                string datetimeFormat = "yyyyMMdd HH:mm:ss";

                DateTime StartDateTime;

                //AES_EnorDecrypt.AESDecryptFile(templicense, tempPath, "TEST_PASSWORD_~!@#");
                CryptoHelp.DecryptFile(templicense, tempPath, "TEST_PASSWORD_~!@#");//net6有问题
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


                if (info.Product != null && info.Product != "AMES-CodeGen")
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
                expiredTime = info.EndDateTime;
            }
            else
            {
                return (false, "authorization file does not exist");
            }
            return (true, $"authorization expiration time: {expiredTime}");
        }
    }
}
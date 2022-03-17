using System;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;

namespace MF.Authorization
{
    public class Esnecil
    {
        public Esnecil()
        {
        }

        [Serializable]
        public class License
        {
            public string company = "";
            public string signal = "";
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
            if (System.IO.File.Exists(templicense))
            {
                string pubkey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDa4loF1nxk1lWPbvCllU10XB3d"
                                + "CNtY5GgxMLKbEO2O18PeQrPRzfit8lokPvC/2D4CFG7OqqVzdMKj1DzZYsy+4Y3o"
                                + "ACx8q9iCiyFIsDlYBOs9a/Zi6ViPRltHyHjhAgT8LilrKhRBsmATKwqNhqw2QBks"
                                + "wCFG0zt5Y2HSJ51NdwIDAQAB";

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


                if (info.Product != null && info.Product != "AMES-CodeGen")
                {
                    return (false, "Product invalid");
                }

                if (info.MachineId != null && info.MachineId != strMachineCode)
                {
                    return (false, "MachineId invalid");
                }

                if (info.Company != null && info.Company == "")
                {
                    return (false, "Company invalid");
                }

                if (info.StartDateTime != null && info.StartDateTime != "")
                {
                    try
                    {
                        StartDateTime = DateTime.ParseExact(info.StartDateTime, datetimeFormat, CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        return (false, "StartDateTime invalid");
                    }
                }
                else
                {
                    return (false, "StartDateTime is null");
                }

                if (info.EndDateTime != null && info.EndDateTime != "")
                {
                    try
                    {
                        var EndDateTime = DateTime.ParseExact(info.EndDateTime, datetimeFormat, CultureInfo.CurrentCulture);

                        if (DateTime.Compare(StartDateTime, EndDateTime) > 0)
                        {
                            return (false, "license expired");
                        }

                        if (DateTime.Compare(DateTime.Now, EndDateTime) > 0)
                        {
                            return (false, "license expired");
                        }
                    }
                    catch
                    {
                        return (false, "license expired");
                    }
                }
                else
                {
                    return (false, "license expired");
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
                        return (false, "Sig expired");
                    }
                }
                else
                {
                    return (false, "Sig is nu");
                }
                expiredTime = info.EndDateTime;
            }
            else
            {
                return (false, "license not found");
            }
            return (true, $"authorization deadline: {expiredTime}");
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
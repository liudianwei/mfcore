using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace MF.Utils
{
    public class MachineCode
    {
        private static MachineCode machineCode;
        private static string machineCodeString = "";

        public static string GetMachineCodeString()
        {
            if (machineCodeString != "")
            {
                return machineCodeString;
            }

            if (machineCode == null)
            {
                machineCode = new MachineCode();
            }
            machineCodeString = "MACROINF." + machineCode.GetCpuInfo()
                + "." + machineCode.GetHDid();
            //+ "." + machineCode.GetMoAddress();
            machineCodeString = ToReverse(To_md5(machineCodeString)).ToUpper();
            return machineCodeString;
        }

        /// <summary>
        /// 根据字符串生成MD5
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="up">是否大小写</param>
        /// <returns>生成后的MD5</returns>
        public static string To_md5(string str, bool up = false)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 =
            BitConverter.ToString(md5.ComputeHash(
            UTF8Encoding.Default.GetBytes(str)));
            t2 = t2.Replace("-", "");
            if (!up)
                return t2.ToLower();
            else
                return t2.ToUpper();
        }

        public static string ToReverse(string str)
        {
            return new string(str.ToCharArray().Reverse().ToArray());
        }

        ///   <summary>
        ///   获取cpu序列号
        ///   </summary>
        ///   <returns> string </returns>
        public string GetCpuInfo()
        {
            string cpuInfo = "";
            try
            {
                using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection moc = cimobject.GetInstances();

                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return cpuInfo.ToString();
        }

        ///   <summary>
        ///   获取硬盘ID
        ///   </summary>
        ///   <returns> string </returns>
        public string GetHDid()
        {
            string HDid = "";
            try
            {
                using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
                {
                    ManagementObjectCollection moc1 = cimobject1.GetInstances();
                    foreach (ManagementObject mo in moc1)
                    {
                        HDid = (string)mo.Properties["Model"].Value;
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return HDid.ToString();
        }

        ///   <summary>
        ///   获取网卡硬件地址
        ///   </summary>
        ///   <returns> string </returns>
        public string GetMoAddress()
        {
            string MoAddress = "";
            try
            {
                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    ManagementObjectCollection moc2 = mc.GetInstances();
                    foreach (ManagementObject mo in moc2)
                    {
                        if ((bool)mo["IPEnabled"] == true)
                            MoAddress = mo["MacAddress"].ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return MoAddress.ToString();
        }
    }
}
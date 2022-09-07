using LicenseCore;
using System;
using System.Runtime.InteropServices;

namespace GetAuthorization
{
    public class Program
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public static string machineCode { set; get; }

        /// <summary>
        /// 产品
        /// </summary>
        public static string product { set; get; } = "AMES-SCADA-CORE";

        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                product = args[0];
            }
            machineCode = LicenseHelper.GetMachineCodeString();
            if (!LicenseHelper.Check(machineCode, product, out string msg, out var info))
            {
                FrmAuthorizationInfo frmAuthorizationInfo = new FrmAuthorizationInfo(machineCode, product);
                if (frmAuthorizationInfo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Console.WriteLine($"产品:{product},机器码:{machineCode}");
                    Console.WriteLine("License:PASS");
                }
            }
            else
            {
                Console.WriteLine($"产品:{product},机器码:{machineCode}");
                Console.WriteLine("License:PASS");
                Console.WriteLine(msg);
            }
        }
    }
}
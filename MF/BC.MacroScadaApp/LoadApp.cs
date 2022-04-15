using System;
using SystemFramework;
using Mes.Exe.Driver.Redis;
using System.Runtime.InteropServices;
using System.Reflection;
using EvetnArgData;
using System.Collections.Generic;

namespace ScadaAppCore
{
    /// <summary>
    ///
    /// </summary>
    public partial class BaseScadaApp
    {
        /// <summary>
        /// 内核版本号
        /// </summary>
        /// <returns></returns>
        public static string Version()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        #region Authentication

        /// <summary>
        /// 判断当前操作系统是否64位或32位
        /// </summary>
        public static bool is64bit = (IntPtr.Size == 8);

        /// <summary>
        ///
        /// </summary>
        [DllImport("Authentication x64.dll", EntryPoint = "VerifyLicenseSN", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void VerifyLicenseSN64();

        /// <summary>
        ///
        /// </summary>
        [DllImport("Authentication x32.dll", EntryPoint = "VerifyLicenseSN", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void VerifyLicenseSN32();

        #endregion Authentication

        /// <summary>
        /// ScadaApp
        /// </summary>
        public static ScadaApp _ScadaApp;

        /// <summary>
        /// 是否允许工作
        /// </summary>
        /// <returns></returns>
        public bool IsCanWork() => ScadaApp.IsCanWork;

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public bool InitAllTag() => _ScadaApp.InitTags();

        /// <summary>
        /// 加载ScadaApp通讯模块
        /// </summary>
        public bool LoadScadaApp(List<string> OpNames = null, List<string> ChangeTagNames = null)
        {
            bool Start = false;

            try
            {
                #region 验证授权码方法

                //VerifyLicenseSN();

                if (is64bit)
                {
                    Console.WriteLine("win x64");
                    VerifyLicenseSN64();
                }
                else
                {
                    Console.WriteLine("win x32");
                    VerifyLicenseSN32();
                }

                #endregion 验证授权码方法

                #region 初始化配置文件

                _ScadaApp = new ScadaApp();
                _ScadaApp.ChangeTagNames = ChangeTagNames;
                _ScadaApp.TagDataOnChange += new ScadaApp.GetDataHandler(TagData);
                if (!_ScadaApp.InitConfig()) return Start;
                if (!_ScadaApp.InitRedisClient()) return Start;
                if (!_ScadaApp.InitTags(OpNames)) return Start;

                #endregion 初始化配置文件

                ScadaApp.IsCanWork = true;
                Start = true;
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, err.Message);
            }
            return Start;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseServer()
        {
            try
            {
                _ScadaApp.CloseRedisClient();
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err.ToString());
            }
        }

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void GetDataHandler(object sender, CustomeEvetnArgs e);

        /// <summary>
        /// 委托
        /// </summary>
        public event GetDataHandler TagData;
    }
}
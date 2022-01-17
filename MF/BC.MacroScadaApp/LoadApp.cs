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

        #region PassKey

        /// <summary>
        /// 验证授权码方法，此方法为阻塞类型（验证不通过，无法往下执行代码）；
        /// </summary>
        [DllImport(@"Authentication.dll", EntryPoint = "VerifyLicenseSN", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern void VerifyLicenseSN();

        #endregion PassKey

        /// <summary>
        /// ScadaApp
        /// </summary>
        private ScadaApp _ScadaApp;

        /// <summary>
        /// 是否允许工作
        /// </summary>
        /// <returns></returns>
        public bool IsCanWork()
        {
            return _ScadaApp.IsCanWork;
        }

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

                VerifyLicenseSN();

                #endregion 验证授权码方法

                #region 初始化配置文件

                _ScadaApp = new ScadaApp();
                _ScadaApp.ChangeTagNames = ChangeTagNames;
                _ScadaApp.TagDataOnChange += new ScadaApp.GetDataHandler(TagData);
                if (!_ScadaApp.InitConfig()) return Start;
                if (!_ScadaApp.InitRedisClient()) return Start;
                if (!_ScadaApp.InitTags(OpNames)) return Start;

                #endregion 初始化配置文件

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
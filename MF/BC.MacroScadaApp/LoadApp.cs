using System;
using SystemFramework;
using Mes.Exe.Driver.Redis;
using System.Runtime.InteropServices;
using System.Reflection;
using EvetnArgData;
using System.Collections.Generic;
using System.Threading;

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
        /// 获取机器码
        /// </summary>
        public string MachineCode { set; get; } = AuthorizationManager.GetMachineCodeString();

        /// <summary>
        /// 加载ScadaApp通讯模块
        /// </summary>
        /// <param name="OpNames"></param>
        /// <param name="ChangeTagNames"></param>
        /// <param name="Product"></param>
        /// <returns></returns>
        public bool LoadScadaApp(List<string> OpNames = null, List<string> ChangeTagNames = null, string Product = "AMES-Misdata")
        {
            bool Start = false;

            try
            {
                #region 验证授权

                ApplicationLog.SystemLog("common", $"产品：{Product} 机器码:{MachineCode}", "INFO");
                Authorization(Product);

                #endregion 验证授权

                #region 初始化配置文件

                _ScadaApp = new ScadaApp();
                _ScadaApp.ChangeTagNames = ChangeTagNames;
                _ScadaApp.TagDataOnChange += new ScadaApp.GetDataHandler(TagData);
                if (!_ScadaApp.InitConfig()) return Start;
                if (!_ScadaApp.InitRedisClient(Product)) return Start;
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
        /// 授权
        /// </summary>
        private void Authorization(string Product)
        {
            var authorization = new Thread(() =>
             {
                 while (true)
                 {
                     var res = AuthorizationManager.Check(MachineCode, Product, out string Msg, out var info);
                     ScadaApp.IsCanWork = res;
                     if (!res)
                     {
                         Common.Frm.FrmAuthorizationInfo authorizationInfo = new Common.Frm.FrmAuthorizationInfo(MachineCode, Product);
                         authorizationInfo.ShowDialog();
                     }
                     Thread.Sleep(3000);
                 }
             });
            authorization.SetApartmentState(ApartmentState.STA);
            authorization.Start();
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
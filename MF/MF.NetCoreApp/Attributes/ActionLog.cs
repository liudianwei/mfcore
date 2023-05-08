using System;
using System.Collections;
using System.Diagnostics;

using Common.Commands;
using DynamicExpresso;
using MF.FluentValidation;
using MF.MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;

namespace MF.NetCoreApp.Attributes
{
    public sealed class ActionLog : ActionFilterAttribute
    {
        /// <summary>
        /// 执行动作描述
        /// </summary>
        public string ActionContent { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        private string ActionArguments { get; set; }
        /// <summary>
        /// 请求体中的所有值
        /// </summary>
        private string RequestBody { get; set; }

        private Stopwatch Stopwatch { get; set; }

        private IBus _bus;
        /// <summary>
        /// 解析器
        /// </summary>
        private Interpreter ExpressionEval { get; set; } = new Interpreter();
        /// <summary>
        /// 自定义参数格式为 LineCode:{command.LineCode}:ProductCode:{command.ProductCode}
        /// </summary>
        public string Parm { get; set; }
        /// <summary>
        /// 自定义参数 临时存储字段
        /// </summary>
        public string InitParm { get; set; }
        /// <summary>
        ///  执行动作描述 临时存储字段
        /// </summary>
        public string InitActionContent { get; set; }
        /// <summary>
        ///  执行动作支持携带参数
        /// </summary>
        /// <param name="_ActionContent">执行动作描述</param>
        /// <param name="_p">参数携带</param>
        public ActionLog(string _ActionContent, string _p = "")
        {
            var arraylist = new ArrayList();
            if (_p != "")
            {
                var par = _p.Split(":");
                foreach (var item in par)
                {
                    if (item.Contains("{"))
                    {
                        arraylist.Add(item.Replace("{", "").Replace("}", ""));
                    }
                }
            }
            ExpressionEval.SetVariable("s", arraylist);
            InitActionContent = ActionContent = _ActionContent;
            InitParm = Parm = _p;
        }

        public ActionLog()
        {
            InitActionContent = ActionContent = "";
            InitParm = Parm = "";
        }

        //private bool HasToken = true;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //授权
            bool ignore = false;
            foreach (var item in context.Filters)
            {
                if (item.GetType() == typeof(IgnoreValid))
                {
                    ignore = true;
                }
            }

            //授权
            if (!ignore)
            {
                if (!new Esnecil().Check())
                {
                    context.Result = new ObjectResult("") { StatusCode = 303 };
                    base.OnActionExecuting(context);
                    return;
                }
            }
            //执行动作 参数解析
            if (InitParm != "")
            {
                //重置初始值
                Parm = InitParm;
                ActionContent = InitActionContent;

                //解析
                var interpreter = new Interpreter();
                var parameters = context.ActionArguments;
                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        interpreter.SetVariable(parameter.Key, parameter.Value);
                    }
                }
                var list = ExpressionEval.Eval<ArrayList>("s");
                foreach (var item in list)
                {
                    var tyu = interpreter.Eval(item.ToString()).ToString();
                    if (item.ToString().Contains("State"))
                    {
                        tyu = (tyu == "0") ? "启用" : "禁用";
                    }
                    Parm = Parm.Replace("{" + item + "}", tyu);
                }
            }
            RequestBody = JsonConvert.SerializeObject(context.ActionArguments);
            ActionContent += Parm;
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            Stopwatch.Stop();

            string method = context.HttpContext.Request.Method;
            string url = method + ":" + context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

            dynamic result = context.Result?.GetType().Name == "EmptyResult" ? new { Value = "无返回结果" } : context.Result as dynamic;

            string res = "";
            string status = "success";
            try
            {
                if (result != null)
                {
                    if (result.Content != null)
                    {
                        HttpResult a = JsonConvert.DeserializeObject<HttpResult>(result.Content);
                        status = a.Status;
                    }
                    res = result.Content;
                }
            }
            catch (Exception)
            {
                res = "未获取到结果，返回的数据无法序列化";
            }

            CreateOperationLogCommand cmd = new CreateOperationLogCommand
            {
                BusinessName = ActionContent,
                Url = url,
                Header = JsonConvert.SerializeObject(context.HttpContext.Request.Headers),
                Params = RequestBody,
                Result = status == "success" ? 1 : 0,
                Response = "",
            };

            cmd.Response = res;
            _bus = (IBus)context.HttpContext.RequestServices.GetService(typeof(IBus));
            _bus.SendAsync(cmd);
        }
    }
}
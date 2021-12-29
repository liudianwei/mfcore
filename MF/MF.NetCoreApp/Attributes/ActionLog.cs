using System;
using System.Diagnostics;

using Common.Commands;

using MF.FluentValidation;
using MF.MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;

namespace MF.NetCoreApp.Attributes
{
    public sealed class ActionLog : ActionFilterAttribute
    {
        public string ActionContent { get; set; }

        private string ActionArguments { get; set; }

        /// <summary>
        /// 请求体中的所有值
        /// </summary>
        private string RequestBody { get; set; }

        private Stopwatch Stopwatch { get; set; }

        private IBus _bus;

        public ActionLog(string _ActionContent)
        {
            ActionContent = _ActionContent;
        }

        public ActionLog()
        {
            ActionContent = "";
        }

        //private bool HasToken = true;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token);

            //Console.WriteLine(token.ToString());

            //if (token.ToString().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length != 2)
            //{
            //    context.HttpContext.Response.StatusCode = 403;
            //}
            //else
            //{
            //    // 解析token 获取用户名称
            //    IConfiguration _configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
            //    JwtConfig jwt = _configuration?.GetSection("Jwt")?.Get<JwtConfig>();
            //    //var jwtSecurityToken = jwt.ReadToken(token.ToString());

            //    // 拿用户名称从缓存里面获取token

            //    // 缓存里面没有 返回401
            //    // 缓存里面和当前不一样 返回401
            //    // 缓存存在就刷新过期时间

            //    // 用户名获取权限 和当前的接口进行对比  判断是否有权限 没有 返回403

            //    base.OnActionExecuting(context);
            //}

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

            //查询缓存中是否存在TOKEN 没有就返回错误

            // 后续添加了获取请求的请求体，如果在实际项目中不需要删除即可
            //long contentLen = context.HttpContext.Request.ContentLength is null ? 0 : context.HttpContext.Request.ContentLength.Value;
            //if (contentLen > 0)
            //{
            //    // 读取请求体中所有内容
            //    System.IO.Stream stream = context.HttpContext.Request.Body;
            //    if (context.HttpContext.Request.Method == "POST")
            //    {
            //        stream.Position = 0;
            //    }
            //    //byte[] buffer = new byte[contentLen];
            //    //stream.ReadAsync(buffer, 0, buffer.Length);
            //    //// 转化为字符串
            //    //RequestBody = System.Text.Encoding.UTF8.GetString(buffer);

            //    Encoding encoding = Encoding.UTF8;
            //    var reader = new StreamReader(stream, encoding);
            //    string result = reader.ReadToEnd();
            //    stream.Position = 0;
            //}
            RequestBody = JsonConvert.SerializeObject(context.ActionArguments);

            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //if (!HasToken) { return; }
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
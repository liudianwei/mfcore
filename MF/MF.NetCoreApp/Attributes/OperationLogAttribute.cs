using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Threading.Tasks;

namespace MF.NetCoreApp.Attributes
{
    /// <summary>
    /// 记录操作日志
    /// </summary>
    public sealed class OperationLogAttribute : ResultFilterAttribute
    {
        /// <summary>
        /// 默认启用
        /// </summary>
        public bool Eable { get; set; } = true;

        /// <summary>
        /// 忽略返回值,比如查询的返回值
        /// </summary>
        public bool Ignore { get; set; } = false;

        /// <summary>
        /// 模块
        /// </summary>

        public string Module { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public OperationLogAttribute(bool eable)
        {
            Eable = eable;
        }

        public OperationLogAttribute(string desc, bool eable = true)
        {
            Description = desc;
            Eable = eable;
        }

        public OperationLogAttribute(string module, string desc)
        {
            Module = module;
            Description = desc;
        }

        public OperationLogAttribute(string module, string desc, bool eable = true)
        {
            Module = module;
            Description = desc;
            Eable = eable;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (Eable)
            {
                // 传入参数
                var parameters = context.ReadResultExecutingContext();
                // 返回值
                var result = context.Result;
                object res;
                if (result is ObjectResult objectResult)
                {
                    res = objectResult.Value;
                }
                else if (result is ContentResult contentResult)
                {
                    res = contentResult.Content;
                }
                else if (result is EmptyResult emptyResult)
                {
                    res = emptyResult;
                }
                else if (result is StatusCodeResult statusCodeResult)
                {
                    res = statusCodeResult;
                }
                else if (result is JsonResult jsonResult)
                {
                    res = jsonResult.Value.ToString();
                }
                else if (result is FileResult fileResult)
                {
                    res = fileResult.FileDownloadName ?? fileResult.ContentType;
                }
                else if (result is ViewResult viewResult)
                {
                    res = viewResult.Model;
                }
                else if (result is RedirectResult redirectResult)
                {
                    res = redirectResult.Url;
                }
                if (!Ignore)
                {
                    res = "";
                }
                // 待处理
            }
            base.OnResultExecuting(context);
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }
    }
}
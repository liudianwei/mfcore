using MF.FluentValidation;
using MF.Utils.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace MF.NetCoreApp.Attributes
{
    /// <summary>
    /// api权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 授权标识
        /// </summary>
        public string[] Perms { get; set; }

        /// <summary>
        ///默认启用
        /// </summary>
        public bool Eable { get; set; } = true;

        private readonly string ContentType = "application/json;charset=utf-8";

        public PermissionAttribute(params string[] perms)
        {
            this.Perms = perms;
        }

        public PermissionAttribute(bool eable, params string[] perms)
        {
            this.Perms = perms;
            this.Eable = eable;
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (Eable && !IsAuth(context))
            {
                var res = new HttpResult
                {
                    Code = 500,
                    Message = "对不起，您无权限访问！",
                    Status = "error"
                };
                context.Result = new ContentResult()
                {
                    Content = res.JilToJsonCamelCase(),
                    ContentType = ContentType,
                };
            }
            return Task.CompletedTask; 
        }

        private bool IsAuth(AuthorizationFilterContext actionContext)
        {
            var context = actionContext.HttpContext;
            if (!context.User.Identity.IsAuthenticated)
            {
                return false;
            }
            // 从缓存或数据库中获取用户的授权标识
            // 比对
            return true;
        }
    }
}
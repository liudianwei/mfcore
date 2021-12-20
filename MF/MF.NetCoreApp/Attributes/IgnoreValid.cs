using System;
using System.Diagnostics;

using Common.Commands;

using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;

using MF.MediatR;
using MF.Swagger;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IO;
using MF.FluentValidation;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MF.NetCoreApp.Attributes
{
    public sealed class IgnoreValid : ActionFilterAttribute
    {
        public IgnoreValid()
        {
        }

        //private bool HasToken = true;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
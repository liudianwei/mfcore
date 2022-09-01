
using Microsoft.AspNetCore.Mvc.Filters;

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
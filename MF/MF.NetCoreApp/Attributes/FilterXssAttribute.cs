using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MF.NetCoreApp.Attributes
{
    public sealed class FilterXssAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 默认开启验证
        /// </summary>
        public bool Ignore { get; set; } = true;

        public string[] WhiteList = Array.Empty<string>();

        public FilterXssAttribute()
        {
        }

        public FilterXssAttribute(bool ignore)
        {
            Ignore = ignore;
        }

        public FilterXssAttribute(bool ignore, params string[] whiteList)
        {
            Ignore = ignore;
            WhiteList = whiteList;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Ignore)
            {
                var ps = context.ActionDescriptor.Parameters;
                foreach (var p in ps)
                {
                    if (context.ActionArguments[p.Name] != null && !WhiteList.Any(c => c == p.Name))
                    {
                        if (p.ParameterType.Equals(typeof(string)))
                        {
                            // 过滤
                            context.ActionArguments[p.Name] = context.ActionArguments[p.Name].ToString();
                        }
                        else if (p.ParameterType.Equals(typeof(string[])))
                        {
                            FormatArrayFieldFilter(p.ParameterType, context.ActionArguments[p.Name]);
                        }
                        else if (p.ParameterType.IsClass)
                        {
                            ModelFieldFilter(p.ParameterType, context.ActionArguments[p.Name]);
                        }
                    }
                }
            }
            base.OnActionExecuting(context);
        }

        private object FormatArrayFieldFilter(Type type, object obj)
        {
            if (obj != null)
            {
                var arr = obj as string[];
                for (int i = 0; i < (arr?.Length ?? 0); i++)
                {
                    arr[i] = arr[i];
                }
            }
            return obj;
        }

        private object ModelFieldFilter(Type type, object obj)
        {
            if (obj != null)
            {
                var infos = type.GetProperties();
                foreach (var info in infos)
                {
                    var IsGenericType = info.PropertyType.IsGenericType;
                    var list = info.PropertyType.GetInterface("IEnumerable", false);
                    if (IsGenericType && list != null)
                    {
                        if (!(info.GetValue(obj) is IEnumerable<object> listValue))
                        {
                            continue;
                        }
                        else
                        {
                            foreach (var item in listValue)
                            {
                                var itemc = item.GetType();
                                foreach (var child in itemc.GetProperties())
                                {
                                    var oldValue = child.GetValue(item);
                                    if (oldValue != null)
                                    {
                                        if (child.PropertyType == typeof(string))
                                        {
                                            child.SetValue(item, oldValue);
                                        }
                                        else if (child.PropertyType.Equals(typeof(string[])))
                                        {
                                            FormatArrayFieldFilter(child.PropertyType, oldValue);
                                        }
                                        else if (child.PropertyType.IsClass)
                                        {
                                            child.SetValue(item, ModelFieldFilter(child.PropertyType, oldValue));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var oldValue = info.GetValue(obj);
                        if (oldValue != null)
                        {
                            if (info.PropertyType == typeof(string))
                            {
                                info.SetValue(obj, oldValue);
                            }
                            else if (info.PropertyType.Equals(typeof(string[])))
                            {
                                FormatArrayFieldFilter(info.PropertyType, oldValue);
                            }
                            else if (info.PropertyType.IsClass)
                            {
                                info.SetValue(obj, ModelFieldFilter(info.PropertyType, oldValue));
                            }
                        }
                    }
                }
            }
            return obj;
        }
    }
}
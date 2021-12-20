using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MF.Core.Extensions
{
    public static class MethodBaseExt
    {
        public static bool IsTask(this MethodBase method)
        {
            var methodInfo = (MethodInfo)method;
            return typeof(Task).IsAssignableFrom(methodInfo.ReturnType);
        }

        public static bool IsGenericTask(this MethodBase method)
        {
            var methodInfo = (MethodInfo)method;
            return method.IsTask()
               && methodInfo.ReturnType.IsGenericType
               && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }

        public static Type[] GetGenericReturnTypeArguments(this MethodBase method)
        {
            var methodInfo = (MethodInfo)method;
            return methodInfo.ReturnType.GetGenericArguments();
        }

        public static Type GetGenericReturnTypeArgument(this MethodBase method, int index)
        {
            var methodInfo = (MethodInfo)method;
            return methodInfo.ReturnType.GenericTypeArguments[index];
        }

        public static Type GetReturnType(this MethodBase method)
        {
            var methodInfo = (MethodInfo)method;
            return method.IsGenericTask() ? method.GetGenericReturnTypeArgument(0)
                : methodInfo.ReturnType;
        }
    }
}
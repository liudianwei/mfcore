using System;

using MF.Core.Exceptions;
using MF.Core.Extensions;

namespace MF.Core.Check
{
    public static class CheckNull
    {
        public static void ArgumentIsNullException<TArgument>(TArgument argument, string argumentName = "不能为空")
            where TArgument : class
        {
            if (argument is null)
                throw new ArgumentNullException(argumentName);
        }

        public static void ArgumentIsNullException(string argument, string argumentName = "不能为空")
        {
            if (argument.IsEmpty())
                throw new ArgumentException(argumentName);
        }

        public static void ArgumentNewException(string argument)
        {
            throw new ArgumentException(argument);
        }

        public static void ArgumentException(Func<bool> func, string argumentName)
        {
            if (func.Invoke())
            {
                throw new ArgumentException(argumentName);
            }
        }

        public static void BusinessException<TArgument>(TArgument argument, string argumentName = "不能为空")
            where TArgument : class
        {
            if (argument is null)
                throw new BusinessException(argumentName);
        }

        public static void BusinessException(string argument, string argumentName = "不能为空")
        {
            if (argument.IsEmpty())
                throw new BusinessException(argumentName);
        }

        public static void BusinessException(string argument)
        {
            throw new BusinessException(argument);
        }

        public static void BusinessException(bool func, string argumentName)
        {
            if (func)
            {
                throw new BusinessException(argumentName);
            }
        }

        public static void BusinessException(Func<bool> func, string argumentName)
        {
            if (func.Invoke())
            {
                throw new BusinessException(argumentName);
            }
        }
    }
}
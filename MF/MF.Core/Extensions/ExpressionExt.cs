using System;
using System.Linq.Expressions;

namespace MF.Core.Extensions
{
    public static class ExpressionExt
    {
        public static Expression<Func<T, bool>> Init<T>()
        {
            return express => true;
        }

        public static Expression<Func<T, T1, T2, bool>> Init<T, T1, T2>()
        {
            return (s, c, u) => true;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> oldExpression, Expression<Func<T, bool>> newExpression)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.Or(oldExpression.Body, newExpression.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> oldExpression, Expression<Func<T, bool>> newExpression)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(oldExpression.Body, newExpression.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> oldExpression, string queryParameter, Expression<Func<T, bool>> newExpression)
        {
            if (queryParameter.IsEmpty())
            {
                return oldExpression;
            }
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(oldExpression.Body, newExpression.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> oldExpression, string[] queryParameter, Expression<Func<T, bool>> newExpression)
        {
            if (queryParameter.IsNull())
            {
                return oldExpression;
            }
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(oldExpression.Body, newExpression.Body);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
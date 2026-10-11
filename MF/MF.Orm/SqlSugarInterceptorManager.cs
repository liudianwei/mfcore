using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MF.Orm
{
    public static class SqlSugarInterceptorManager
    {
        // 注册时在锁内发布新的不可变快照，使 SQL 执行热路径无需加锁。
        private static Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>[] _interceptors =
            Array.Empty<Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>>();
        private static readonly HashSet<string> _registrationKeys =
            new HashSet<string>(StringComparer.Ordinal);

        private static readonly object _lock = new object();

        /// <summary>
        /// 注册自定义 SQL 改写逻辑
        /// </summary>
        public static void RegisterInterceptor(
            Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> interceptor,
            string registrationKey)
        {
            if (string.IsNullOrWhiteSpace(registrationKey))
                throw new ArgumentException("拦截器注册键不能为空", nameof(registrationKey));

            lock (_lock)
            {
                if (!_registrationKeys.Add(registrationKey))
                    return;

                var interceptors = new Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>[_interceptors.Length + 1];
                Array.Copy(_interceptors, interceptors, _interceptors.Length);
                interceptors[interceptors.Length - 1] = interceptor;
                Volatile.Write(ref _interceptors, interceptors);
            }
        }

        /// <summary>
        /// 统一初始化 AOP 拦截管道（在 AddSqlSugarScope 的 db 回调里只呼叫一次）
        /// </summary>
        public static void InitPipeline(this ISqlSugarClient db)
        {
            // 💡 只写属性没有 getter 没关系，我们只赋值一次，内部搞定循环调用！
            db.Aop.OnExecutingChangeSql = (sql, pars) =>
            {
                // 顺着注册的顺序，像流水线一样依次加工 SQL
                var interceptors = Volatile.Read(ref _interceptors);
                foreach (var interceptor in interceptors)
                {
                    var result = interceptor(sql, pars);
                    sql = result.Key;
                    pars = result.Value;
                }

                return KeyValuePair.Create(sql, pars);
            };
        }
    }
}

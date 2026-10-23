using SqlSugar;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MF.Orm
{
    public static class SqlSugarInterceptorManager
    {
        private static readonly ConditionalWeakTable<ISqlSugarClient, InterceptorState> _instanceStates =
            new ConditionalWeakTable<ISqlSugarClient, InterceptorState>();

        private sealed class InterceptorState
        {
            internal readonly object Lock = new object();
            internal readonly HashSet<string> RegistrationKeys = new HashSet<string>(StringComparer.Ordinal);
            internal Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>[] Interceptors =
                Array.Empty<Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>>();
        }

        /// <summary>
        /// 为指定数据库客户端注册 SQL 改写逻辑。注册键在当前客户端内唯一。
        /// </summary>
        public static void RegisterInterceptor(
            ISqlSugarClient db,
            Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> interceptor,
            string registrationKey)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));
            if (interceptor == null)
                throw new ArgumentNullException(nameof(interceptor));
            if (string.IsNullOrWhiteSpace(registrationKey))
                throw new ArgumentException("拦截器注册键不能为空", nameof(registrationKey));

            var state = _instanceStates.GetValue(db, _ => new InterceptorState());
            lock (state.Lock)
            {
                if (!state.RegistrationKeys.Add(registrationKey))
                    return;

                var interceptors = new Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>[state.Interceptors.Length + 1];
                Array.Copy(state.Interceptors, interceptors, state.Interceptors.Length);
                interceptors[interceptors.Length - 1] = interceptor;
                Volatile.Write(ref state.Interceptors, interceptors);
            }
        }

        /// <summary>
        /// 统一初始化 AOP 拦截管道（在 AddSqlSugarScope 的 db 回调里只呼叫一次）
        /// </summary>
        public static void InitPipeline(this ISqlSugarClient db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            var state = _instanceStates.GetValue(db, _ => new InterceptorState());
            // 💡 只写属性没有 getter 没关系，我们只赋值一次，内部搞定循环调用！
            db.Aop.OnExecutingChangeSql = (sql, pars) =>
            {
                // 顺着注册的顺序，像流水线一样依次加工 SQL
                var instanceInterceptors = Volatile.Read(ref state.Interceptors);
                foreach (var interceptor in instanceInterceptors)
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

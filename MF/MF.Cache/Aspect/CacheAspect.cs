using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

using AspectInjector.Broker;

namespace MF.Cache
{
    [Aspect(Scope.Global, Factory = typeof(CacheAspectFactory))]
    public class CacheAspect
    {
        private readonly ICacheProvide _cache;

        //private readonly ICacheProvide _cache = MemoryCacheProvide.GetInstance();
        private static readonly MethodInfo _asyncHandler = typeof(CacheAspect).GetMethod(nameof(CacheAspect.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo TaskResultMethod = typeof(Task).GetMethods().FirstOrDefault(p => p.Name == "FromResult" && p.ContainsGenericParameters);
        private static readonly MethodInfo _asyncHandlerResult = typeof(CacheAspect).GetMethod(nameof(CacheAspect.WrapAsyncResult), BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo _syncHandler = typeof(CacheAspect).GetMethod(nameof(CacheAspect.WrapSync), BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly Type _voidTaskResult = Type.GetType("System.Threading.Tasks.VoidTaskResult");

        public CacheAspect(ICacheProvide cache)
        {
            _cache = cache;
        }

        [Advice(Kind.Around, Targets = Target.Any)]
        public object HandleMethod(
           [Argument(Source.Instance)] object instance,
           [Argument(Source.Type)] Type type,
           [Argument(Source.Metadata)] MethodBase methodBase,
           [Argument(Source.Target)] Func<object[], object> target,
           [Argument(Source.Name)] string name,
           [Argument(Source.Arguments)] object[] arguments,
           [Argument(Source.ReturnType)] Type retType,
           [Argument(Source.Triggers)] Attribute[] triggers
            )
        {
            object result = null;
            var resultFound = false;
            var key = CacheKey.GetCacheKey(methodBase, arguments);
            var cacheTrigger = triggers.OfType<CacheAttribute>().FirstOrDefault();
            var isAsync = IsAsync(retType);
            if (!string.IsNullOrWhiteSpace(cacheTrigger._key))
            {
                key = cacheTrigger._key;
            }
            var returnType = GetReturnType(retType);
            if (cacheTrigger._flag)
            {
                // 缓存
                var cache = _cache.GetValue<byte[]>(key);
                if (cache.Item1)
                {
                    var cacheValue = JsonSerializer.Deserialize(cache.Item2, returnType);
                    // 存在缓存，不继续执行
                    resultFound = true;
                    result = isAsync
                    ? TaskResultMethod.MakeGenericMethod(returnType).Invoke(null, new object[] { cacheValue })
                    : cacheValue;
                }
            }
            var sw = Stopwatch.StartNew();
            // 执行方法
            if (!resultFound)
            {
                if (isAsync)
                {
                    var syncResultType = retType.IsConstructedGenericType ? retType.GenericTypeArguments[0] : _voidTaskResult;
                    result = _asyncHandler.MakeGenericMethod(syncResultType).Invoke(this, new object[] { target, arguments, name });
                }
                else
                {
                    retType = retType == typeof(void) ? typeof(object) : retType;
                    result = _syncHandler.MakeGenericMethod(retType).Invoke(this, new object[] { target, arguments, name });
                }
                var cacheValue = isAsync ? _asyncHandlerResult.MakeGenericMethod(returnType).Invoke(this, new object[] { result }) : result;
                var bytes = JsonSerializer.SerializeToUtf8Bytes(cacheValue, returnType);

                if (cacheTrigger._expireSeconds != -1)
                {
                    _cache.Add(key, bytes, cacheTrigger._expireSeconds);
                }
                else
                {
                    _cache.Add(key, bytes);
                }
            }
            sw.Stop();
            Console.WriteLine($"Executed method {name} in {sw.ElapsedMilliseconds} ms");
            return result;
        }

        private string GetKey(object instance, MethodInfo method, object[] args) => $"{instance?.GetHashCode() ?? method.DeclaringType.GetHashCode()}{method.GetHashCode()}{args.Select(a => a.GetHashCode()).Sum()}";

        private bool IsAsync(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
        }

        private Type GetReturnType(Type retType)
        {
            var returnType = IsAsync(retType)
                 ? retType.GetGenericArguments().First()
                 : retType;
            return returnType;
        }

        private static T WrapSync<T>(Func<object[], object> target, object[] args, string name)
        {
            try
            {
                var result = (T)target(args);
                //Console.WriteLine($"Sync method `{name}` completes successfuly.");
                return result;
            }
            catch (Exception)
            {
                //Console.WriteLine($"Sync method `{name}` throws {e.GetType()} exception.");
                return default;
            }
        }

        private static async Task<T> WrapAsync<T>(Func<object[], object> target, object[] args, string name)
        {
            try
            {
                var result = await (Task<T>)target(args);
                //Console.WriteLine($"Async method `{name}` completes successfuly.");
                return result;
            }
            catch (Exception)
            {
                //Console.WriteLine($"Async method `{name}` throws {e.GetType()} exception.");
                return default;
            }
        }

        private static T WrapAsyncResult<T>(Task<T> t)
        {
            try
            {
                var result = t.Result;
                return result;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
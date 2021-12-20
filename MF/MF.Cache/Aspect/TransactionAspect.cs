using AspectInjector.Broker;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MF.Orm.UnitOfWork;
using MF.NetCoreApp;

namespace MF.NetCore
{
    [Aspect(Scope.Global)]
    public class TransactionAspect
    {
        private static readonly MethodInfo _asyncHandler = typeof(TransactionAspect).GetMethod(nameof(TransactionAspect.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo TaskResultMethod = typeof(Task).GetMethods().FirstOrDefault(p => p.Name == "FromResult" && p.ContainsGenericParameters);
        private static readonly MethodInfo _asyncHandlerResult = typeof(TransactionAspect).GetMethod(nameof(TransactionAspect.WrapAsyncResult), BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo _syncHandler = typeof(TransactionAspect).GetMethod(nameof(TransactionAspect.WrapSync), BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly Type _voidTaskResult = Type.GetType("System.Threading.Tasks.VoidTaskResult");
        private static IUnitOfWork _unitOfWork;

        public TransactionAspect()
        {
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
            var isAsync = IsAsync(retType);
            object result;
            var cacheTrigger = triggers.OfType<TransactionAttribute>().FirstOrDefault();

            if (isAsync)
            {
                var syncResultType = retType.IsConstructedGenericType ? retType.GenericTypeArguments[0] : _voidTaskResult;
                result = _asyncHandler.MakeGenericMethod(syncResultType).Invoke(this, new object[] { target, arguments, name, cacheTrigger });
            }
            else
            {
                retType = retType == typeof(void) ? typeof(object) : retType;
                result = _syncHandler.MakeGenericMethod(retType).Invoke(this, new object[] { target, arguments, name, cacheTrigger });
            }
            return result;
        }

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

        private static T WrapSync<T>(Func<object[], object> target, object[] args, string name, TransactionAttribute transaction)
        {
            _unitOfWork = GlobalCore.Context.GetRequiredService<IUnitOfWork>();
            var _db = _unitOfWork.GetDbClient();
            try
            {
                _db.Ado.BeginTran(transaction.isolationLevel);
                var result = (T)target(args);
                _db.Ado.CommitTran();
                return result;
            }
            catch (Exception ex)
            {
                _db.Ado.RollbackTran();
                throw new Exception(ex.Message);
            }
        }

        private static async Task<T> WrapAsync<T>(Func<object[], object> target, object[] args, string name, TransactionAttribute transaction)
        {
            _unitOfWork = GlobalCore.Context.GetRequiredService<IUnitOfWork>();
            var _db = _unitOfWork.GetDbClient();
            try
            {
                _db.Ado.BeginTran(transaction.isolationLevel);
                var result = await (Task<T>)target(args);
                _db.Ado.CommitTran();
                return result;
            }
            catch (Exception ex)
            {
                _db.Ado.RollbackTran();
                throw new Exception(ex.Message);
            }
        }

        private static T WrapAsyncResult<T>(Task<T> t)
        {
            try
            {
                var result = t.Result;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
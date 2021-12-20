using AspectInjector.Broker;

using System;
using System.Data;

namespace MF.NetCore
{
    [Injection(typeof(TransactionAspect), Inherited = true)]
    [AttributeUsage(
        AttributeTargets.Method,
         AllowMultiple = true, Inherited = true)]
    public sealed class TransactionAttribute : Attribute
    {
        public IsolationLevel isolationLevel = IsolationLevel.ReadCommitted;

        public TransactionAttribute()
        {
        }

        public TransactionAttribute(IsolationLevel isolation)
        {
            isolationLevel = isolation;
        }
    }
}
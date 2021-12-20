using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MF.Cache
{
    [Aspect(Scope.Global, Factory = typeof(CacheRemoveAspectFactory))]
    public class CacheRemoveAspect
    {
        //private readonly ICacheProvide _cache = MemoryCacheProvide.GetInstance();
        private readonly ICacheProvide _cache;

        public CacheRemoveAspect(ICacheProvide cache)
        {
            _cache = cache;
        }

        [Advice(Kind.After, Targets = Target.Any)]
        public void HandleAfter(
           [Argument(Source.Instance)] object instance,
           [Argument(Source.Type)] Type type,
           [Argument(Source.Name)] string name,
           [Argument(Source.Arguments)] object[] arguments,
           [Argument(Source.ReturnType)] Type retType,
           [Argument(Source.ReturnValue)] object result,
           [Argument(Source.Triggers)] Attribute[] triggers
            )
        {
            var cacheTrigger = triggers.OfType<CacheRemoveAttribute>().FirstOrDefault();
            _cache.Remove(cacheTrigger._key);
        }
    }
}
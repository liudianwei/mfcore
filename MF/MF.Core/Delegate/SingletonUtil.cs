using System;
using System.Collections.Generic;
using System.Text;

namespace MF.Core.Delegate
{
    public class SingletonUtil<T> where T : class, new()
    {
        private static T _instance;
        private static readonly object _locker = new object();

        private SingletonUtil()
        {
        }

        public static T GetInstance()
        {
            if (_instance is null)
            {
                _instance = new T();
            }
            return _instance;
        }

        public static T GetInstanceLock()
        {
            if (_instance is null)
            {
                lock (_locker)
                {
                    if (_instance is null) { _instance = new T(); }
                }
            }
            return _instance;
        }
    }

    public class SingletonLazy<T> where T : class, new()
    {
        private static readonly Lazy<T> _instance =
        new Lazy<T>(() => new T());

        public static T Instance
        {
            get { return _instance.Value; }
        }
    }
}
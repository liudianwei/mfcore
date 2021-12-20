using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MF.Core.Delegate
{
    public class DelegateUtil
    {
        public static void TryExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                throw new Exception(nameof(action), ex);
            }
        }

        public static void TryExecute(Action action, Action<string> actionException)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                actionException.Invoke(ex.ToString());
            }
        }

        public static void TryExecute(Action action, Action<Exception> actionException)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                actionException.Invoke(ex);
            }
        }

        public static T TryExecute<T>(Func<T> fun)
        {
            try
            {
                return fun();
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static T TryExecute<T>(Func<T> func, T defaultValue = default)
        {
            try
            {
                return func();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static T TryExecute<T>(Func<T> func, Func<T> exception)
        {
            try
            {
                return func();
            }
            catch (Exception)
            {
                return exception();
            }
        }

        public static void TryExecute(Action action, Action<Exception> onException = null, Action onFinally = null)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                onException?.Invoke(e);
            }
            finally
            {
                onFinally?.Invoke();
            }
        }

        public static void TryExecuteTask(Action action, Action<string> actionException)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    actionException.Invoke(ex.ToString());
                }
            });
        }

        public static Task<T> TryExecuteSaftTask<T>(Func<T> func)
        {
            TaskCompletionSource<T> ts = new TaskCompletionSource<T>();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    T result = func();
                    ts.SetResult(result);
                }
                catch (Exception ex)
                {
                    ts.SetException(ex);
                }
            });
            return ts.Task;
        }

        public static void TryExecuteThread(Action action)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    throw new Exception(nameof(action), ex);
                }
            });
        }

        public static long TryStopWatch(Action action)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                action();
                watch.Stop();
                return watch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                throw new Exception(nameof(action), ex);
            }
        }

        public static void Check(bool flag, Action action)
        {
            if (flag)
            {
                action();
            }
        }
    }
}
using System;
using System.Threading;

namespace HslCommunication.LogNet
{
    #region 简单的混合锁

    /// <summary>
    /// 一个简单的混合线程同步锁，采用了基元用户加基元内核同步构造实现<br />
    /// A simple hybrid thread editing lock, implemented by the base user plus the element kernel synchronization.
    /// </summary>
    /// <remarks>
    /// 当前的锁适用于，竞争频率比较低，锁部分的代码运行时间比较久的情况，当前的简单混合锁可以达到最大性能。
    /// </remarks>
    /// <example>
    /// 以下演示常用的锁的使用方式，还包含了如何优雅的处理异常锁
    /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\Core\ThreadLock.cs" region="SimpleHybirdLockExample1" title="SimpleHybirdLock示例" />
    /// </example>
    public sealed class SimpleHybirdLock : IDisposable
    {
        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //  释放托管状态(托管对象)。
                }

#if NET35 || NET20
				m_waiterLock.Close();
#else
                m_waiterLock.Value.Close();
#endif

                disposedValue = true;
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support

        /// <summary>
        /// 基元用户模式构造同步锁
        /// </summary>
        private int m_waiters = 0;

#if NET35 || NET20
		/// <summary>
		/// 基元内核模式构造同步锁
		/// </summary>
		private AutoResetEvent m_waiterLock = new AutoResetEvent(false);
#else

        /// <summary>
        /// 基元内核模式构造同步锁
        /// </summary>
        private readonly Lazy<AutoResetEvent> m_waiterLock = new Lazy<AutoResetEvent>(() => new AutoResetEvent(false));

#endif

        /// <summary>
        /// 获取锁
        /// </summary>
        public void Enter()
        {
            Interlocked.Increment(ref simpleHybirdLockCount);
            if (Interlocked.Increment(ref m_waiters) == 1) return;      // 用户锁可以使用的时候，直接返回，第一次调用时发生
                                                                        // 当发生锁竞争时，使用内核同步构造锁
            Interlocked.Increment(ref simpleHybirdLockWaitCount);
#if NET35 || NET20
			m_waiterLock.WaitOne();
#else
            m_waiterLock.Value.WaitOne();
#endif
        }

        /// <summary>
        /// 离开锁
        /// </summary>
        public void Leave()
        {
            Interlocked.Decrement(ref simpleHybirdLockCount);
            if (Interlocked.Decrement(ref m_waiters) == 0) return;     // 没有可用的锁的时候

            Interlocked.Decrement(ref simpleHybirdLockWaitCount);
#if NET35 || NET20
			m_waiterLock.Set( );
#else
            m_waiterLock.Value.Set();
#endif
        }

        /// <summary>
        /// 获取当前锁是否在等待当中
        /// </summary>
        public bool IsWaitting => m_waiters != 0;

        #region Static Value

        private static long simpleHybirdLockCount = 0;      // 当前总的锁的进入次数
        private static long simpleHybirdLockWaitCount = 0;  // 当前锁的等待的次数，此时已经开始竞争了

        /// <summary>
        /// 获取当前总的所有进入锁的信息<br />
        /// Get the current total information of all access locks
        /// </summary>
        public static long SimpleHybirdLockCount => simpleHybirdLockCount;

        /// <summary>
        /// 当前正在等待的锁的统计信息，此时已经发生了竞争了
        /// </summary>
        public static long SimpleHybirdLockWaitCount => simpleHybirdLockWaitCount;

        #endregion Static Value
    }

    #endregion 简单的混合锁
}
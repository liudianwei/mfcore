using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace SystemFramework
{
    /// <summary>
    /// 订阅消息
    /// </summary>
    public class NetMQ_subscribe
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public delegate void GetDataHandler(string message);

        /// <summary>
        ///
        /// </summary>
        public event GetDataHandler OnGetData;

        /// <summary>
        /// 断线重连日志
        /// </summary>
        public event GetDataHandler OnConnectionError;

        /// <summary>
        /// IP和端口号
        /// </summary>
        public string IpPort;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="ipPort"></param>
        public NetMQ_subscribe(string ipPort)
        {
            IpPort = @"tcp://" + ipPort;
        }

        /// <summary>
        /// 超时时间
        /// </summary>
        private TimeSpan timeout { get; set; } = new TimeSpan(0, 5, 0);

        /// <summary>
        /// 创建一个线程接收接收远程主机发来的信息
        /// </summary>
        private Thread recDataThread { get; set; }

        /// <summary>
        /// 轮询开始
        /// </summary>
        private bool start { get; set; } = true;

        /// <summary>
        /// socket
        /// </summary>
        private SubscriberSocket subscriberSocket { get; set; } = new SubscriberSocket(null);

        /// <summary>
        /// 开始订阅消息
        /// </summary>
        public void StartRecData(TimeSpan _timeout = default)
        {
            timeout = _timeout != default ? _timeout : timeout;
            recDataThread = new Thread(new ThreadStart(RecData));
            //将线程设为后台运行
            recDataThread.IsBackground = true;
            recDataThread.Start();
        }

        /// <summary>
        /// 移除订阅消息
        /// </summary>
        public void AbortRecData()
        {
            start = false;
            subscriberSocket.Close();
            subscriberSocket.Dispose();
            recDataThread?.Abort();
            recDataThread = null;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void RecData()
        {
            try
            {
                if (start)
                {
                    try
                    {
                        subscriberSocket?.Connect(IpPort);
                        subscriberSocket?.Subscribe("");
                        while (start)
                        {
                            var message = subscriberSocket?.ReceiveFrameString();
                            OnGetData?.Invoke(message);
                        }
                    }
                    finally
                    {
                        subscriberSocket?.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                OnConnectionError?.Invoke(e.Message);
                Thread.Sleep(5000);
                ReConnection();
            }
        }

        /// <summary>
        /// 重连
        /// </summary>
        /// <param name="sub"></param>
        private void ReConnection()
        {
            if (subscriberSocket != null)
            {
                try
                {
                    subscriberSocket.Close();
                    subscriberSocket.Dispose();
                }
                catch (Exception ee)
                {
                    OnConnectionError?.Invoke(ee.Message);
                }
            }
            RecData();
        }
    }
}
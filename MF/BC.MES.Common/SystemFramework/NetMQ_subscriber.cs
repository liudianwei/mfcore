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

        private TimeSpan timeout = new TimeSpan(0, 5, 0);

        /// <summary>
        /// 开始订阅消息
        /// </summary>
        public void StartRecData(TimeSpan _timeout = default)
        {
            timeout = _timeout != default ? _timeout : timeout;
            //创建一个线程接收接收远程主机发来的信息
            Thread mythread = new Thread(new ThreadStart(RecData));
            //将线程设为后台运行
            mythread.IsBackground = true;
            mythread.Start();
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void RecData()
        {
            SubscriberSocket subscriberSocket = null;
            try
            {
                subscriberSocket = new SubscriberSocket(null);
                try
                {
                    subscriberSocket.Connect(IpPort);
                    subscriberSocket.Subscribe("");
                    while (true)
                    {
                        var message = subscriberSocket.ReceiveFrameString();
                        OnGetData?.Invoke(message);
                    }
                }
                finally
                {
                    if (subscriberSocket != null)
                    {
                        ((IDisposable)subscriberSocket).Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                OnConnectionError?.Invoke(e.Message);
                Thread.Sleep(5000);
                ReConnection(subscriberSocket);
            }
        }

        /// <summary>
        /// 重连
        /// </summary>
        /// <param name="sub"></param>
        private void ReConnection(SubscriberSocket sub)
        {
            if (sub != null)
            {
                try
                {
                    sub.Close();
                    sub.Dispose();
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
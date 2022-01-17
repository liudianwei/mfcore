using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemFramework
{
    /// <summary>
    /// 公共通讯类
    /// </summary>
    public partial class MacroCommunication
    {
        /// <summary>
        /// 发布
        /// </summary>
        public static NetMQ_Publisher netMQ_Publisher;
    }

    /// <summary>
    ///
    /// </summary>
    public class NetMQ_Publisher
    {
        private PublisherSocket publisher;

        private NetMQ_Publisher()
        {
        }

        /// <summary>
        /// tcp://127.0.0.1:5556
        /// </summary>
        /// <param name="IpPort"></param>
        public NetMQ_Publisher(string IpPort)
        {
            try
            {
                publisher = new PublisherSocket();
                publisher.Bind("tcp://" + IpPort);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// information
        /// </summary>
        /// <param name="information"></param>
        public void Send(string information)
        {
            publisher.SendFrame(information);
        }

        /// <summary>
        ///
        /// </summary>
        public void close()
        {
            publisher.Close();
            publisher.Dispose();
        }
    }
}
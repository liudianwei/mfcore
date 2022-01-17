using System;
//添加的命名空间引用
using System.Net;
using System.Net.Sockets;

namespace SystemFramework
{
    /// <summary>
    /// 利用UDP向端口port发送数据
    /// </summary>
    public class UDPClientSend
    {
        /// <summary>
        /// 向指定IP发送UDP
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="text"></param>
        public static void Send(string ip, int port, string text)
        {
            UdpClient myUdpclient = new UdpClient();
            string sendMessage;
            try
            {
                DateTime m_DateTime = DateTime.Now;
                IPEndPoint iep;
                if (ip == "")
                {
                    iep = new IPEndPoint(IPAddress.Broadcast, port);
                }
                else
                {
                    iep = new IPEndPoint(IPAddress.Parse(ip), port);
                }
                sendMessage = text;

                byte[] bytes;
                bytes = System.Text.Encoding.UTF8.GetBytes(sendMessage);
                myUdpclient.Send(bytes, bytes.Length, iep);
                myUdpclient.Close();
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, "UDPClientSend Send");
            }
            finally
            {
                myUdpclient.Close();
            }
        }
        /// <summary>
        /// 利用UDP向端口port发送数据
        /// </summary>
        /// <param name="port"></param>
        /// <param name="text"></param>
        public static void Send(int port, string text)
        {
            UdpClient myUdpclient = new UdpClient();
            string sendMessage;
            try
            {
                DateTime m_DateTime = DateTime.Now;
                IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, port);
                sendMessage = text;

                byte[] bytes;
                bytes = System.Text.Encoding.UTF8.GetBytes(sendMessage);
                myUdpclient.Send(bytes, bytes.Length, iep);
                myUdpclient.Close();

            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, "UDPClientSend Send");
            }
            finally
            {
                myUdpclient.Close();
            }
        }

    }
}

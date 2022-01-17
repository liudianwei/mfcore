using System;
using System.Text;
//添加的命名空间引用
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SystemFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DelegateClassHandle_UDPServerReceive(object sender, UDPServerReceiveEvetnArgs e);
    /// <summary>
    /// 
    /// </summary>
    public class UDPServerReceiveEvetnArgs : EventArgs
    {
        object tagKey = 0;
        object opName = "";
        object tagValue = 0;
        object tagQuality = 0;
        /// <summary>
        /// 
        /// </summary>
        public UDPServerReceiveEvetnArgs()
        { }
        /// <summary>
        /// 
        /// </summary>
        public object TagKey
        {
            get { return this.tagKey; }
            set { this.tagKey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object TagValue
        {
            get { return this.tagValue; }
            set { this.tagValue = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object OpName
        {
            get { return this.opName; }
            set { this.opName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object TagQuality
        {
            get { return this.tagQuality; }
            set { this.tagQuality = value; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TagDataUDPServerReceive
    {


        private object tagKey;
        /// <summary>
        /// 
        /// </summary>
        public object TagKey
        {
            get { return tagKey; }
            set { tagKey = value; }
        }
        private object tagValue;
        /// <summary>
        /// 
        /// </summary>
        public object TagValue
        {
            get { return tagValue; }
            set { tagValue = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public event DelegateClassHandle_UDPServerReceive TagDataOnChange;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void InvokeTagData(UDPServerReceiveEvetnArgs e)
        {
            if (TagDataOnChange != null)
            {
                TagDataOnChange(this, e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        public void InvokeTagData(object tagValue)
        {
            if (TagDataOnChange != null)
            {
                UDPServerReceiveEvetnArgs e = new UDPServerReceiveEvetnArgs();
                e.TagValue = tagValue;
                TagDataOnChange(this, e);
            }
        }

    }

    /// <summary>
    /// 使用的接收端口 
    /// </summary>
    public class UDPServerReceive
    {
        Thread mythread;
        //使用的接收端口 
        /// <summary>
        /// 端口号
        /// </summary>
        private int port;
        /// <summary>
        /// udp连接对象
        /// </summary>
        private UdpClient udpclient;
        /// <summary>
        /// 
        /// </summary>
        public TagDataUDPServerReceive m_TagDataUDPServerReceive;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port_In"></param>
        public UDPServerReceive(int port_In)
        {
            m_TagDataUDPServerReceive = new TagDataUDPServerReceive();
            port = port_In;
            //创建一个线程接收接收远程主机发来的信息
            mythread = new Thread(new ThreadStart(RecData));
            //将线程设为后台运行
            mythread.IsBackground = true;
            mythread.Start();

        }
        /// <summary>
        /// 开始接受新的端口数据
        /// </summary>
        /// <param name="port_In"></param>
        public void Start(int port_In)
        {
            port = port_In;
            if (mythread != null)
            {
                try
                {
                    if (udpclient != null)
                    {
                       udpclient.Close();
                    }
                    mythread.Abort();
                }
                catch { }

            }
            //创建一个线程接收接收远程主机发来的信息
            mythread = new Thread(new ThreadStart(RecData));
            //将线程设为后台运行
            mythread.IsBackground = true;
            mythread.Start();

        }
        /// <summary>
        /// 开始接受新的端口数据
        /// </summary>
        /// <param name="port_In"></param>
        public void Close(int port_In)
        {
            port = port_In;
            if (mythread != null)
            {
                try
                {
                    if (udpclient != null)
                    {
                        udpclient.Close();
                    }
                    //mythread.Abort();
                }
                catch { }

            }
        }
        /// <summary>
        /// 在后台运行的接收线程
        /// </summary>
        private void RecData()
        {
            try
            {
                //本机指定端口接收
                udpclient = new UdpClient(port);
            }
            catch (Exception err)
            {
                ApplicationLog.WriteLog(err, "RecData:new UdpClient(port)=" + port.ToString());
            }

            IPEndPoint remote = null;
            while (true)
            {
                try
                {
                    //接收从远程主机发送过来的信息
                    string sendMessage;
                    //关闭udpclient时此句会产生异常
                    byte[] bytes = udpclient.Receive(ref remote);
                    string strSource = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    sendMessage = strSource;
                    m_TagDataUDPServerReceive.InvokeTagData(sendMessage);

                }
                catch (Exception err)
                {
                    ApplicationLog.WriteLog(err, err.ToString());
                }
            }
        }
    }
}

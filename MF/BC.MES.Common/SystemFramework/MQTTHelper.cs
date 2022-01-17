using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemFramework;

namespace SystemFramework
{
    /// <summary>
    /// 公共通讯类
    /// </summary>
    public partial class MacroCommunication
    {
        /// <summary>
        /// 定义Mqtt客户端
        /// </summary>
        public static MQTTClient mqtt;
    }

    /// <summary>
    /// MQTT客户端
    /// </summary>
    public class MQTTClient
    {
        /// <summary>
        /// 初始化客户端
        /// </summary>
        private static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

        /// <summary>
        /// 接收到订阅消息的事件
        /// </summary>
        private static event Action<string> Recive;

        /// <summary>
        /// mqttClient的IP
        /// </summary>
        private static string IP = ConfigurationManager.AppSettings["MQTTAddress"].Split(':')[0];

        /// <summary>
        /// mqttClient的端口号
        /// </summary>
        private static int PORT = Convert.ToInt32(ConfigurationManager.AppSettings["MQTTAddress"].Split(':')[1]);

        /// <summary>
        /// mqttClient的用户名
        /// </summary>
        private static string UserName = "master";

        /// <summary>
        /// mqttClientID
        /// </summary>
        private static string ClientID = "MqttClient_[MES_Code]" + ConfigurationManager.AppSettings["MES_Code"] + "_" + Guid.NewGuid().ToString();

        /// <summary>
        /// 订阅通道
        /// </summary>
        private static string MQTTRecvTopic = ConfigurationManager.AppSettings["MQTTRecvTopic"];

        /// <summary>
        /// 发布通道
        /// </summary>
        private static string MQTTSentTopic = ConfigurationManager.AppSettings["MQTTSentTopic"];

        /// <summary>
        /// 会话重用机制
        /// </summary>
        private static bool CleanSession = Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["CleanSession"]));

        /// <summary>
        /// Client配置
        /// </summary>
        private static IMqttClientOptions options = null;

        /// <summary>
        /// 通讯地址
        /// </summary>
        public string Address => IP + ":" + PORT;

        /// <summary>
        ///  MQTT客户端
        /// </summary>
        /// <returns></returns>
        public MQTTClient()
        {
            mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(new Func<MqttClientConnectedEventArgs, Task>(MqttClient_Connected));
            mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(new Func<MqttClientDisconnectedEventArgs, Task>(MqttClient_Disconnected));
            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(new Action<MqttApplicationMessageReceivedEventArgs>(MqttClient_ApplicationMessageReceived));
            options = new MqttClientOptionsBuilder()
                    .WithClientId(ClientID)
                    .WithTcpServer(IP, PORT)
                    .WithCredentials(UserName, "")
                    .WithCleanSession(CleanSession)
                    .Build();
            try
            {
                mqttClient.ConnectAsync(options, CancellationToken.None);
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("### MQTT CREATE FAILED ###" + ex.Message);
                ApplicationLog.WriteLog("### MQTT CREATE FAILED ###" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// mqtt客户端连接成功触发事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task MqttClient_Connected(MqttClientConnectedEventArgs e)
        {
            try
            {
                await mqttClient.PublishAsync(
                    new MqttApplicationMessageBuilder()
                .WithTopic(MQTTSentTopic)
                .WithPayload(ClientID + " StartUp")
                .WithAtMostOnceQoS()
                .Build()
                );
                while (!mqttClient.IsConnected)
                {
                    ApplicationLog.WriteLog("### MQTT RECONNECTING FAILE,SLEEP 2S,UNTIL CONNECTED ###");
                    Thread.Sleep(2000);
                }
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(MQTTRecvTopic).WithExactlyOnceQoS().Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine("### MQTT PUBLISH FAILED ###" + ex.Message);
                ApplicationLog.WriteLog("### MQTT PUBLISH FAILED ###" + ex.Message);
            }
        }

        /// <summary>
        /// mqtt客户端断开连接触发事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task MqttClient_Disconnected(MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine("### MQTT DISCONNECTED FROM SERVER ###");
            await Task.Delay(TimeSpan.FromSeconds(5));
            try
            {
                await mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                Console.WriteLine("### MQTT RECONNECTING FAILED ###" + ex.Message);
                ApplicationLog.WriteLog("### MQTT RECONNECTING FAILE ###" + ex.Message);
            }
        }

        /// <summary>
        /// mqtt客户端订阅消息，回调事件
        /// </summary>
        /// <param name="e"></param>
        private void MqttClient_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            Recive?.Invoke(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
        }

        /// <summary>
        /// mqtt订阅
        /// </summary>
        /// <param name="CallBack"></param>
        /// <returns></returns>
        public async void Sub(Action<string> CallBack)
        {
            Recive += CallBack;
        }

        /// <summary>
        /// mqtt发布
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async void Pub(string message)
        {
            try
            {
                await mqttClient.PublishAsync(
               new MqttApplicationMessageBuilder()
                   .WithTopic(MQTTSentTopic)
                   .WithPayload(message)
                   .WithAtMostOnceQoS()
                   .Build()
                   );

                #region 逻辑日志记录

                if (message.Split('|').Length > 3)
                {
                    ApplicationLog.BusinessLog(message.Split('|')[2], "SendMsg:" + message);
                }

                #endregion 逻辑日志记录
            }
            catch (Exception ex)
            {
                Console.WriteLine("### MQTT PUBLISH FAILED ###" + ex.Message);
                ApplicationLog.WriteLog("### MQTT PUBLISH FAILED ###" + ex.Message);
            }
        }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return mqttClient.IsConnected;
        }

        /// <summary>
        /// 非必需的，只是为了更符合其他语言的规范，如C++、java
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        ///
        /// </summary>
        ~MQTTClient()
        {
            //必须为false
            Dispose(false);
        }

        /// <summary>
        /// 释放标记
        /// </summary>
        private bool disposed;

        /// <summary>
        /// 是否资源
        /// </summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收器不再调用终结器
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 非密封类可重写的Dispose方法，方便子类继承时可重写
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {
            }
            //清理非托管资源
            // if (NativeResource != IntPtr.Zero) {
            //     Marshal.FreeHGlobal (NativeResource);
            //     NativeResource = IntPtr.Zero;
            // }
            //告诉自己已经被释放
            disposed = true;
        }
    }
}
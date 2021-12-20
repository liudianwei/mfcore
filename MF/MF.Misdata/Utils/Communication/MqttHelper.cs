using Common.Utils;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Communication
{
    public class MQTTHelper
    {
        private static IMqttClient mqttClient;

        /// <summary>
        /// 接收到订阅消息的事件
        /// </summary>
        private static event Action<string> Recive;

        /// <summary>
        /// mqtt Ip
        /// </summary>
        private static readonly string MqttIp = ConfigHelper.GetAppseting("MQTT:Address").Split(':')[0];

        /// <summary>
        /// mqtt Port
        /// </summary>
        private static readonly int MqttPort = Convert.ToInt32(ConfigHelper.GetAppseting("MQTT:Address").Split(':')[1]);

        /// <summary>
        /// mqtt ClientID
        /// </summary>
        private static readonly string ClientID = ConfigHelper.GetAppseting("MQTT:ClientId");

        /// <summary>
        /// mqtt UserName
        /// </summary>
        private static readonly string UserName = ConfigHelper.GetAppseting("MQTT:UserName");

        /// <summary>
        /// mqtt Password
        /// </summary>
        private static readonly string Password = ConfigHelper.GetAppseting("MQTT:Password");

        /// <summary>
        /// mqtt CleanSession
        /// </summary>
        private static readonly bool CleanSession = ConfigHelper.GetAppseting("MQTT:CleanSession") == "1";

        /// <summary>
        /// mqtt发布通道
        /// </summary>
        private static readonly string SentTopic = ConfigHelper.GetAppseting("MQTT:SentTopic");

        /// <summary>
        /// mqtt订阅通道
        /// </summary>
        private static readonly string RecvTopic = ConfigHelper.GetAppseting("MQTT:RecvTopic");

        /// <summary>
        /// Client配置
        /// </summary>
        private static IMqttClientOptions options = null;

        /// <summary>
        /// 初始化MQTT客户端
        /// </summary>
        private MQTTHelper()
        {
            mqttClient = new MqttFactory().CreateMqttClient();
            mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(new Func<MqttClientConnectedEventArgs, Task>(MqttClient_Connected));
            mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(new Func<MqttClientDisconnectedEventArgs, Task>(MqttClient_Disconnected));
            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(new Action<MqttApplicationMessageReceivedEventArgs>(MqttClient_ApplicationMessageReceived));

            options = new MqttClientOptionsBuilder()
                .WithClientId(ClientID)
                .WithTcpServer(MqttIp, MqttPort)
                .WithCredentials(UserName, Password)
                .WithCleanSession(CleanSession)
                .WithCommunicationTimeout(new TimeSpan(0, 0, 30, 0))
                .Build();

            try
            {
                mqttClient.ConnectAsync(options, CancellationToken.None);
                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"### MQTT CREATE FAILED ###{ex.Message}");
            }
        }

        public static void Init()
        {
            _ = new MQTTHelper();
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
                .WithTopic(SentTopic)
                .WithPayload($"{ClientID} StartUp")
                .WithAtMostOnceQoS()
                .Build()
                );
                while (!mqttClient.IsConnected)
                {
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"### MQTT PUBLISH FAILED ###{ex.Message}");
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
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(RecvTopic).WithExactlyOnceQoS().Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"### MQTT RECONNECTING FAILED ###{ex.Message}");
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
        /// <param name="callback"></param>
        /// <returns></returns>
        public static async void Sub(Action<string> callback)
        {
            Recive += callback;
            while (!mqttClient.IsConnected)
            {
                Thread.Sleep(2000);
            }
            await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(RecvTopic).Build());
        }

        /// <summary>
        /// mqtt发布
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async void Pub(String message)
        {
            await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                    .WithTopic(SentTopic)
                    .WithPayload(message)
                    .WithAtMostOnceQoS()
                    .Build());
        }

        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
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

        ~MQTTHelper()
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
            //告诉自己已经被释放
            disposed = true;
        }
    }
}
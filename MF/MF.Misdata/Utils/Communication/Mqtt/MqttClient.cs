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
    /// <summary>
    /// Mqtt操作类
    /// </summary>
    public class MqttClient
    {
        /// <summary>
        ///
        /// </summary>
        private IMqttClient Client { get; set; } = null;

        /// <summary>
        /// Client配置
        /// </summary>
        private IMqttClientOptions options { get; set; } = null;

        /// <summary>
        /// 接收到订阅消息的事件
        /// </summary>
        public event Action<string> Recive;

        /// <summary>
        /// 接收到发布消息的事件
        /// </summary>
        public event Action<string> Send;

        /// <summary>
        /// mqtt Ip
        /// </summary>
        public string _Ip { get; set; } = "127.0.0.1";

        /// <summary>
        /// mqtt Port
        /// </summary>
        public int _port { get; set; } = 1883;

        /// <summary>
        /// mqtt ClientID
        /// </summary>
        public string _clientid { get; set; } = "clientid";

        /// <summary>
        /// mqtt UserName
        /// </summary>
        public string _username { get; set; } = "admin";

        /// <summary>
        /// mqtt Password
        /// </summary>
        public string _password { get; set; } = "";

        /// <summary>
        /// mqtt CleanSession
        /// </summary>
        public bool _cleansession { get; set; } = false;

        /// <summary>
        /// mqtt发布通道
        /// </summary>
        public string _senttopic { get; set; } = "A";

        /// <summary>
        /// mqtt订阅通道
        /// </summary>
        public string _recvtopic { get; set; } = "B";

        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public bool IsConnected => Client.IsConnected;

        /// <summary>
        /// 主动关闭
        /// </summary>
        private bool _close { get; set; } = false;

        /// <summary>
        /// 初始化MQTT客户端
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="clientid"></param>
        /// <param name="senttopic"></param>
        /// <param name="recvtopic"></param>
        /// <param name="cleansession"></param>
        public MqttClient(string ip = "127.0.0.1", int port = 1883, string username = "", string password = "",
            string clientid = "clientid", string senttopic = "A", string recvtopic = "B", bool cleansession = false)
        {
            _Ip = ip;
            _port = port;
            _username = username;
            _password = password;
            _clientid = clientid;
            _senttopic = senttopic;
            _recvtopic = recvtopic;
            _cleansession = cleansession;

            Client = new MqttFactory().CreateMqttClient();
            Client.ConnectedHandler =
                new MqttClientConnectedHandlerDelegate
                (
                    new Func<MqttClientConnectedEventArgs, Task>(MqttClient_Connected)
                );
            Client.DisconnectedHandler =
                new MqttClientDisconnectedHandlerDelegate
                (
                  new Func<MqttClientDisconnectedEventArgs, Task>(MqttClient_Disconnected)
                );
            Client.ApplicationMessageReceivedHandler =
                new MqttApplicationMessageReceivedHandlerDelegate
                (
                    new Action<MqttApplicationMessageReceivedEventArgs>(MqttClient_ApplicationMessageReceived)
                );

            try
            {
                options = new MqttClientOptionsBuilder()
               .WithClientId(_clientid + $"({Guid.NewGuid().ToString()})")
               .WithTcpServer(_Ip, _port)
               .WithCredentials(_username, _password)
               .WithCleanSession(_cleansession)
               .Build();
                Client.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                SystemLog.Exception($"### MQTT CREATE FAILED ###", ex);
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
                await Client.PublishAsync(
                    new MqttApplicationMessageBuilder()
                    .WithTopic(_senttopic)
                    .WithPayload($"{_clientid} StartUp")
                    .WithAtMostOnceQoS()
                    .Build()
                    );
                while (!Client.IsConnected)
                {
                    SystemLog.Error("### MQTT RECONNECTING FAILE,SLEEP 2S,UNTIL CONNECTED ###");
                    Thread.Sleep(2000);
                }
                SystemLog.Info($"Mqtt客户端连接成功 ClientId[{options.ClientId}]");
                await Client.SubscribeAsync(_recvtopic, MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("The return codes are not matching the topic filters [MQTT-3.9.3-1]."))
                {
                    SystemLog.Exception($"### MQTT PUBLISH FAILED ###", ex);
                }
            }
        }

        /// <summary>
        /// mqtt客户端断开连接触发事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task MqttClient_Disconnected(MqttClientDisconnectedEventArgs e)
        {
            if (!_close)
            {
                SystemLog.Error($"### MQTT DISCONNECTED FROM SERVER,SLEEP 5S,UNTIL CONNECTED ###");
                Thread.Sleep(5000);
                try
                {
                    await Client.ConnectAsync(options);
                }
                catch (Exception ex)
                {
                    SystemLog.Error($"### MQTT RECONNECTING FAILED, {ex.Message}###");
                }
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
        public void Sub(Action<string> callback)
        {
            Recive += callback;
        }

        /// <summary>
        /// mqtt发布
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async void Pub(string message, string topic = "")
        {
            try
            {
                if (Client.IsConnected)
                {
                    await Client.PublishAsync(new MqttApplicationMessageBuilder()
                  .WithTopic(topic == "" ? _senttopic : topic)
                  .WithPayload(message)
                  .WithAtLeastOnceQoS()
                  .Build());
                }
                else
                {
                    SystemLog.Error($"### MQTT PUBLISH FAILED,DISCONNECTED FROM SERVER ###");
                }

                Send?.Invoke(message);
            }
            catch (Exception ex)
            {
                SystemLog.Exception($"### MQTT PUBLISH FAILED ###", ex);
            }
        }

        /// <summary>
        /// 非必需的，只是为了更符合其他语言的规范，如C++、java
        /// </summary>
        public void Close()
        {
            _close = true;
            Client.UnsubscribeAsync(_recvtopic);
            Client.DisconnectAsync(new MqttClientDisconnectOptions() { ReasonCode = MqttClientDisconnectReason.NormalDisconnection, ReasonString = "NormalClose" });
            Dispose();
        }

        ~MqttClient()
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
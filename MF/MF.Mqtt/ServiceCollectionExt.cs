using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MQTTnet.Client;
using MQTTnet;
using System;
using System.Threading.Tasks;
using MQTTnet.Client.Options;
using System.Threading;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;

namespace MF.MQTT
{
    public static class ServiceCollectionExt
    {
        private static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

        public static string _clientid { get; set; } = $"webApi_{Guid.NewGuid().ToString()}";
        /// <summary>
        /// Client配置
        /// </summary>
        private static IMqttClientOptions options { get; set; } = null;
        public static IServiceCollection AddMqttClient(this IServiceCollection services, IConfiguration configuration)
        {
            // 这些放配置文件 参考cache
            services.Configure<MqttOptions>(configuration?.GetSection("MqttOption"));
            var config = configuration?.GetSection("MqttOption")?.Get<MqttOptions>();           

            options = new MqttClientOptionsBuilder()
               .WithClientId($"({_clientid})")
               .WithTcpServer(config.HostIp, config.HostPort)
               .WithCredentials(config.UserName, config.Password)
               .WithCleanSession(true)
               .Build();
            mqttClient.ConnectAsync(options).ConfigureAwait(false).GetAwaiter();
            mqttClient.DisconnectedHandler =
               new MqttClientDisconnectedHandlerDelegate
               (
                 new Func<MqttClientDisconnectedEventArgs, Task>(MqttClient_Disconnected)
               );

            mqttClient.ConnectedHandler =
                new MqttClientConnectedHandlerDelegate
                (
                    new Func<MqttClientConnectedEventArgs, Task>(MqttClient_Connected)
                );

            services.AddSingleton(mqttClient);
            return services;
        }

        /// <summary>
        /// mqtt客户端断开连接触发事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static async Task MqttClient_Disconnected(MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"### Mqtt客户端断开连接 ###");
            try
            {
                await mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"### Mqtt客户端断开连接 ###");
            }
        }

        /// <summary>
        /// mqtt客户端连接成功触发事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static async Task MqttClient_Connected(MqttClientConnectedEventArgs e)
        {
            try
            {
                await mqttClient.PublishAsync(
                    new MqttApplicationMessageBuilder()
                    .WithTopic("misdataSent")
                    .WithPayload($"{_clientid} StartUp")
                    .WithAtMostOnceQoS()
                    .Build()
                    );
                while (!mqttClient.IsConnected)
                {
                    Console.WriteLine("### MQTT RECONNECTING FAILE,SLEEP 2S,UNTIL CONNECTED ###");
                    Thread.Sleep(2000);
                }
                Console.WriteLine($"Mqtt客户端连接成功 ClientId[{options.ClientId}]");
                //await mqttClient.SubscribeAsync("misdataRecv", MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("The return codes are not matching the topic filters [MQTT-3.9.3-1]."))
                {
                    Console.WriteLine($"### MQTT PUBLISH FAILED ###{ex}");
                }
            }
        }
        //public static IMqttClient mqttClient { get; set; }
        //private static void ConnectToServer()
        //{
        //    try
        //    {
        //        // 1. 创建 MQTT 客户端
        //        mqttClient = new MqttFactory().CreateMqttClient();


        //        // 2 . 设置 MQTT 客户端选项
        //        MqttClientOptionsBuilder optionsBuilder = new MqttClientOptionsBuilder();

        //        // 设置服务器端地址
        //        optionsBuilder.WithTcpServer("127.0.0.1", 8001);

        //        // 设置鉴权参数
        //        optionsBuilder.WithCredentials("yufei", "123456");

        //        // 设置客户端序列号
        //        optionsBuilder.WithClientId(Guid.NewGuid().ToString());

        //        // 创建选项
        //        IMqttClientOptions options = optionsBuilder.Build();


        //        // 设置消息接收处理程序
        //        mqttClient.UseApplicationMessageReceivedHandler(args => {
        //            Console.WriteLine("### 收到来自服务器端的消息 ###");
        //            // 收到的消息主题
        //            string topic = args.ApplicationMessage.Topic;
        //            // 收到的的消息内容
        //            string payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
        //            // 收到的发送级别(Qos)
        //            var qos = args.ApplicationMessage.QualityOfServiceLevel;
        //            // 收到的消息保持形式
        //            bool retain = args.ApplicationMessage.Retain;

        //            Console.WriteLine($"主题: [{topic}] 内容: [{payload}] Qos: [{qos}] Retain:[{retain}]");
        //        });

        //        // 重连机制
        //        mqttClient.UseDisconnectedHandler(async e =>
        //        {
        //            Console.WriteLine("与服务器之间的连接断开了，正在尝试重新连接");
        //            // 等待 5s 时间
        //            await Task.Delay(TimeSpan.FromSeconds(5));
        //            try
        //            {
        //                // 重新连接
        //                await mqttClient.ConnectAsync(options);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"重新连接服务器失败:{ex}");
        //            }
        //        });

        //        // 连接到服务器
        //        mqttClient.ConnectAsync(options);
        //        Console.WriteLine("连接服务器成功！请输入任意内容并回车进入菜单界面");


        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write($"连接服务器失败: {ex}");
        //    }
        //}

        //private static void SendMessage()
        //{
        //    Console.ReadLine();

        //    // 是否退出
        //    bool isExit = false;

        //    while (!isExit)
        //    {
        //        Console.WriteLine(@"请输入
        //    1. 订阅主题
        //    2. 取消订阅
        //    3. 发送消息
        //    4. 退出");

        //        string input = Console.ReadLine();
        //        string topic = "";

        //        switch (input)
        //        {
        //            case "1":
        //                Console.WriteLine(@"请输入主题名称：");
        //                topic = Console.ReadLine();
        //                ClientSubscribeTopic(topic);
        //                break;
        //            case "2":
        //                Console.WriteLine(@"请输入需要退订的主题名称：");
        //                topic = Console.ReadLine();
        //                ClientUnsubscribeTopic(topic);
        //                break;
        //            case "3":
        //                Console.WriteLine(@"请输入需要发送的主题名称: ");
        //                topic = Console.ReadLine();
        //                Console.WriteLine(@"请输入需要发送的消息内容：");
        //                string message = Console.ReadLine();
        //                ClientPublish(topic, message);
        //                break;
        //            case "4":
        //                isExit = true;
        //                break;
        //            default:
        //                Console.WriteLine("请输入正确的指令");
        //                break;
        //        }
        //    }
        //}

        //private static async void ClientSubscribeTopic(string topic)
        //{
        //    topic = topic.Trim();
        //    if (string.IsNullOrEmpty(topic))
        //    {
        //        Console.Write("订阅主题不能为空！");
        //        return;
        //    }

        //    // 判断客户端是否连接
        //    if (!mqttClient.IsConnected)
        //    {
        //        Console.WriteLine("MQTT 客户端尚未连接!");
        //        return;
        //    }

        //    // 设置订阅参数
        //    var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
        //        .WithTopicFilter(topic)
        //        .Build();

        //    // 订阅
        //    await mqttClient.SubscribeAsync(
        //            subscribeOptions,
        //            System.Threading.CancellationToken.None);
        //}

        //private static async void ClientUnsubscribeTopic(string topic)
        //{
        //    topic = topic.Trim();
        //    if (string.IsNullOrEmpty(topic))
        //    {
        //        Console.Write("退订主题不能为空！");
        //        return;
        //    }

        //    // 判断客户端是否连接
        //    if (!mqttClient.IsConnected)
        //    {
        //        Console.WriteLine("MQTT 客户端尚未连接!");
        //        return;
        //    }

        //    // 设置订阅参数
        //    var subscribeOptions = new MqttClientUnsubscribeOptionsBuilder()
        //        .WithTopicFilter(topic)
        //        .Build();


        //    // 退订
        //    await mqttClient.UnsubscribeAsync(
        //            subscribeOptions,
        //            System.Threading.CancellationToken.None);
        //}


        //private async static void ClientPublish(string topic, string message)
        //{
        //    topic = topic.Trim();
        //    message = message.Trim();

        //    if (string.IsNullOrEmpty(topic))
        //    {
        //        Console.Write("退订主题不能为空！");
        //        return;
        //    }

        //    // 判断客户端是否连接
        //    if (!mqttClient.IsConnected)
        //    {
        //        Console.WriteLine("MQTT 客户端尚未连接!");
        //        return;
        //    }

        //    // 填充消息
        //    var applicationMessage = new MqttApplicationMessageBuilder()
        //        .WithTopic(topic)       // 主题
        //        .WithPayload(message)   // 消息
        //        .WithExactlyOnceQoS()   // qos
        //        .WithRetainFlag()       // retain
        //        .Build();

        //    await mqttClient.PublishAsync(applicationMessage);
        //}
    }
}

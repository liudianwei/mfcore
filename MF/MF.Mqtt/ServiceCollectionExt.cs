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
using NLog;

namespace MF.MQTT
{
    public static class ServiceCollectionExt
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

        public static string _clientid { get; set; } = $"webApi_{Guid.NewGuid().ToString()}";
        /// <summary>
        /// Client配置
        /// </summary>
        private static IMqttClientOptions options { get; set; } = null;
        public static IServiceCollection AddMqttClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MqttOptions>(configuration?.GetSection("MqttOption"));
            var config = configuration?.GetSection("MqttOption")?.Get<MqttOptions>();
            if (config != null && config.Enabled)
            {
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

            }
            else
            {
                Console.WriteLine($"### Mqtt is not enabled or [MqttOption] is missing in appsettings.json ###");
                logger.Error($"### Mqtt is not enabled or [MqttOption] is missing in appsettings.json ###");
            }
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
            Console.WriteLine($"### MQTT客户端断开连接 ###");
            try
            {
                await mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                logger.Error($"### MQTT客户端断开连接 ###");
                Console.WriteLine($"### MQTT客户端断开连接 ###");
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
                Console.WriteLine($"MQTT客户端连接成功 ClientId[{options.ClientId}]");
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("The return codes are not matching the topic filters [MQTT-3.9.3-1]."))
                {
                    Console.WriteLine($"### MQTT PUBLISH FAILED ###{ex}");
                    logger.Error($"### MQTT PUBLISH FAILED ###{ex}");
                }
            }
        }
    }
}

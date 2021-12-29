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
        /// <summary>
        /// Mqtt 客户端
        /// </summary>
        public static MqttClient mqttClient { get; set; }

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
        /// 初始化客户端
        /// </summary>
        /// <returns></returns>
        public static bool InitMqttClient()
        {
            bool flag = false;
            try
            {
                mqttClient = new MqttClient(MqttIp, MqttPort, UserName, Password, ClientID, SentTopic, RecvTopic, CleanSession);
                Thread.Sleep(1000);
                flag = mqttClient.IsConnected;
            }
            catch (Exception ex)
            {
                SystemLog.Fatal("InitMqttClient", ex);
            }

            return flag;
        }
    }
}
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
        public static string MqttIp { get; set; } = "127.0.0.1";

        /// <summary>
        /// mqtt Port
        /// </summary>
        public static int MqttPort { get; set; } = 1883;

        /// <summary>
        /// mqtt ClientID
        /// </summary>
        public static string ClientID { get; set; } = "MisData_Unknown_1";

        /// <summary>
        /// mqtt UserName
        /// </summary>
        public static string UserName { get; set; } = "master";

        /// <summary>
        /// mqtt Password
        /// </summary>
        public static string Password { get; set; } = "";

        /// <summary>
        /// mqtt CleanSession
        /// </summary>
        public static bool CleanSession { get; set; } = false;

        /// <summary>
        /// mqtt发布通道
        /// </summary>
        public static string SentTopic { get; set; } = "misdataSent";

        /// <summary>
        /// mqtt订阅通道
        /// </summary>
        public static string RecvTopic { get; set; } = "misdataRecv";

        /// <summary>
        /// 初始化客户端
        /// </summary>
        /// <returns></returns>
        public static bool InitMqttClient()
        {
            bool flag = false;
            try
            {
                MqttIp = (ConfigHelper.GetAppseting("MQTT:Address") ?? "127.0.0.1:1883").Split(':')[0];
                MqttPort = Convert.ToInt32((ConfigHelper.GetAppseting("MQTT:Address") ?? "127.0.0.1:1883").Split(':')[1]);
                UserName = ConfigHelper.GetAppseting("MQTT:UserName") ?? "master";
                Password = ConfigHelper.GetAppseting("MQTT:Password") ?? "";
                ClientID = ConfigHelper.GetAppseting("MQTT:ClientId") ?? "MisData_Unknown_1";
                SentTopic = ConfigHelper.GetAppseting("MQTT:SentTopic") ?? "misdataSent";
                RecvTopic = ConfigHelper.GetAppseting("MQTT:RecvTopic") ?? "misdataRecv";
                CleanSession = (ConfigHelper.GetAppseting("MQTT:CleanSession") ?? "0") == "1";
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
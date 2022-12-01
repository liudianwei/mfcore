using Common.Communication;
using Common.Utils;
using System;
using System.Configuration;

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
        public static MqttClient mqtt;
    }

    /// <summary>
    /// Mqtt管理
    /// </summary>
    public class MqttManager
    {
        /// <summary>
        /// mqttClient的IP
        /// </summary>
        private static string MqttIp = ConfigurationManager.AppSettings["MQTTAddress"].Split(':')[0];

        /// <summary>
        /// mqttClient的端口号
        /// </summary>
        private static int MqttPort = Convert.ToInt32(ConfigurationManager.AppSettings["MQTTAddress"].Split(':')[1]);

        /// <summary>
        /// mqttClientID
        /// </summary>
        private static string ClientID = "MqttClient_[MES_Code]" + ConfigurationManager.AppSettings["MES_Code"] + "_" + Guid.NewGuid().ToString();

        /// <summary>
        /// mqttClient的用户名
        /// </summary>
        private static string UserName = "master";

        /// <summary>
        /// 订阅通道
        /// </summary>
        private static string RecvTopic = ConfigurationManager.AppSettings["MQTTRecvTopic"];

        /// <summary>
        /// 发布通道
        /// </summary>
        private static string SentTopic = ConfigurationManager.AppSettings["MQTTSentTopic"];

        /// <summary>
        /// 会话重用机制
        /// </summary>
        private static bool CleanSession = Convert.ToBoolean(Convert.ToInt32(ConfigurationManager.AppSettings["CleanSession"]));

        /// <summary>
        /// 初始化客户端
        /// </summary>
        /// <returns></returns>
        public static MqttClient InitMqttClient()
        {
            try
            {
                MqttClient client = new MqttClient(MqttIp, MqttPort, UserName, "", ClientID, SentTopic, RecvTopic, CleanSession);
                client.Send += Client_Send;
                return client;
            }
            catch (Exception ex)
            {
                SystemLog.Fatal("InitMqttClient", ex);
                return null;
            }
        }

        /// <summary>
        /// 收到发布成功数据
        /// </summary>
        /// <param name="message"></param>
        private static void Client_Send(string message)
        {
            #region 逻辑日志记录

            if (message.Split('|').Length > 3)
            {
                if (message.Split('|')[1] != "HeartBeatPLC" && message.Split('|')[1] != "HeartBeatMIS")
                {
                    ApplicationLog.BusinessLog(message.Split('|')[2], "【数据转发】" + message);
                }
                ApplicationLog.SystemLog(message.Split('|')[2], "SendMsg:" + message);
            }

            #endregion 逻辑日志记录
        }
    }
}
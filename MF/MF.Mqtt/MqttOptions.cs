namespace MF.MQTT
{
    public class MqttOptions
    { 
        public string HostIp { get; set; }

        public int HostPort { get; set; }

        public int Timeout { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
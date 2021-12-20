using System;
using System.Collections.Generic;
using System.Text;

namespace MF.Consul
{
    public class ConsulOptions
    {
        public string Address { set; get; }
        public string ServiceName { set; get; }
        public string ServiceIP { set; get; }
        public int ServicePort { set; get; }
        public string ServiceHealthCheck { set; get; }

        public int? Interval { set; get; }
        public int? TimeOut { set; get; }

        public int? DeregisterCriticalServiceAfter { get; set; }
    }
}
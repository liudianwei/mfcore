using System;
using System.Collections.Generic;
using System.Text;

namespace MF.Swagger
{
    public class YLSwaggerOptions
    {
        public string XmlName { get; set; }

        public string[] XmlNames { get; set; }

        public bool Enabled { get; set; }

        public string RoutePrefix { get; set; } = string.Empty;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Models
{
    public class MqttMessage
    {
        public string Topic { get; set; }
        public string Event { get; set; }
        public Record Record { get; set; }
    }
}
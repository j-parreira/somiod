using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Models
{
    public class MqttMessage
    {
        public string topic { get; set; }
        public string event_type { get; set; }
        public string message { get; set; }
        public DateTime received_time { get; set; }
    }
}
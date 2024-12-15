using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExternalAPI.Models
{
    public class Message
    {
        public string topic { get; set; }
        public string content { get; set; }
        public string event_type { get; set; }
    }
}
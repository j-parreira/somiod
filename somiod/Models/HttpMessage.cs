using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Models
{
    public class Message
    {
        public string topic { get; set; }
        public string content { get; set; }
        public string event_type { get; set; }
    }
}
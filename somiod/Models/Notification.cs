using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Models
{
    public class Notification : Container
    {
        public string Event { get; set; } // 0 = creation & deletion, 1 = creation, 2 = deletion
        public string Endpoint { get; set; }
        public bool Enabled { get; set; }
    }
}
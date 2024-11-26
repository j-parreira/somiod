using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Models
{
    public class Notification : RecordOrNotification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int Parent { get; set; }
        public int Event { get; set; }
        public string Endpoint { get; set; }
        public bool Enabled { get; set; }
    }
}
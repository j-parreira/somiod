﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int Parent { get; set; }
        public int Event { get; set; } // 0 = creation & deletion, 1 = creation, 2 = deletion
        public string Endpoint { get; set; }
        public bool Enabled { get; set; }
    }
}
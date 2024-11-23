using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Creation_DateTime { get; set; }
        public int Parent { get; set; }
    }
}
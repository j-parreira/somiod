/**
* @brief Somiod - Projeto de Integração de Sistemas
* @date 2024-12-18
* @authors Diogo Abegão, João Parreira, Marcelo Oliveira, Pedro Barbeiro
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace somiod.Models
{
    public class Notification : Container
    {
        public string event_type { get; set; } // 0 = creation & deletion, 1 = creation, 2 = deletion
        public string endpoint { get; set; }
        public bool enabled { get; set; }
    }
}
/**
* @brief Somiod - Projeto de Integração de Sistemas
* @date 2024-12-18
* @authors Diogo Abegão, João Parreira, Marcelo Oliveira, Pedro Barbeiro
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace somiod
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var xmlFormatter = config.Formatters.XmlFormatter;
            xmlFormatter.UseXmlSerializer = true;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

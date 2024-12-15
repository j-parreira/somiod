using ExternalAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Xml.Linq;

namespace ExternalAPI.Controllers
{
    [RoutePrefix("api/message")]
    public class ExternalAPIController : ApiController
    {
        // List of Messages received
        private static List<Message> messages = new List<Message>
        {
            new Message { topic = "http_servers/http_server_mail", content = "ON", event_type = "creation" },
            new Message { topic = "http_computers/http_desktop", content = "OFF", event_type = "deletion" },
            new Message { topic = "http_sensors/http_temperature_sensor", content = "ON", event_type = "creation" }
        };

        // ---------------------------- XML Formatter ---------------------------
        XmlMediaTypeFormatter xmlFormatter = new XmlMediaTypeFormatter
        {
            Indent = true,
            UseXmlSerializer = true
        };

        // POST: api/external receives a record post request by the somiod server and returns the same record
        [Route("")]
        [HttpPost]
        public IHttpActionResult PostRecord([FromBody] Message message)
        {
            if (message == null)
            {
                return BadRequest("message must be provided.");
            }
            try
            {
                messages.Add(message);
                return Content(HttpStatusCode.OK, message, xmlFormatter);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/record returns all records
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetMessages()
        {
            try
            {
                return Content(HttpStatusCode.OK, messages);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}

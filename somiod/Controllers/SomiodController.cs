using somiod.Handlers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using Application = somiod.Models.Application;

namespace somiod.Controllers
{
    [RoutePrefix("api/somiod")]
    public class SomiodController : ApiController
    {
        // ---Application---

        // GET: api/somiod
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetApplications()
        {
            var locateHeader = Request.Headers.FirstOrDefault(h => h.Key.Equals("somiod-locate", StringComparison.OrdinalIgnoreCase));
            if (locateHeader.Key != null)
            {
                string locateType = locateHeader.Value.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(locateType))
                {
                    return BadRequest("Invalid somiod-locate header value.");
                }

                try
                {
                    switch (locateType.ToLower())
                    {
                        case "application":
                            var applications = ApplicationHandler.FindApplicationsInDatabase();
                            if (applications == null || !applications.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), new XmlMediaTypeFormatter());
                            }
                            return Content(HttpStatusCode.OK, applications.Select(a => a.Name), new XmlMediaTypeFormatter());

                        default:
                            return BadRequest($"Unsupported locate type: {locateType}");
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            List<Application> apps = new List<Application>();

            try
            {
                apps = ApplicationHandler.FindApplicationsInDatabase();

                if (apps == null || !apps.Any())
                {
                    return Ok(new List<Application>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, apps, new XmlMediaTypeFormatter());
        }

        // GET: api/somiod/{application}
        [Route("{application}")]
        [HttpGet]
        public IHttpActionResult GetApplication(string application)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }

            var locateHeader = Request.Headers.FirstOrDefault(h => h.Key.Equals("somiod-locate", StringComparison.OrdinalIgnoreCase));
            if (locateHeader.Key != null)
            {
                string locateType = locateHeader.Value.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(locateType))
                {
                    return BadRequest("Invalid somiod-locate header value.");
                }

                try
                {
                    switch (locateType.ToLower())
                    {
                        case "container":
                            var containers = ContainerHandler.FindContainersByApplication(application);
                            if (containers == null || !containers.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), new XmlMediaTypeFormatter());
                            }
                            return Content(HttpStatusCode.OK, containers.Select(c => c.Name), new XmlMediaTypeFormatter());

                        case "record":
                            var records = RecordHandler.FindRecordsByApplication(application);
                            if (records == null || !records.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), new XmlMediaTypeFormatter());
                            }
                            return Content(HttpStatusCode.OK, records.Select(r => r.Name), new XmlMediaTypeFormatter());

                        case "notification":
                            var notifications = NotificationHandler.FindNotificationsByApplication(application);
                            if (notifications == null || !notifications.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), new XmlMediaTypeFormatter());
                            }
                            return Content(HttpStatusCode.OK, notifications.Select(n => n.Name), new XmlMediaTypeFormatter());

                        default:
                            return BadRequest($"Unsupported locate type: {locateType}");
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            Application app;

            try
            {
                app = ApplicationHandler.FindApplicationInDatabase(application);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            if (app == null)
            {
                return NotFound();
            }

            return Content(HttpStatusCode.OK, app, new XmlMediaTypeFormatter());
        }


        // POST: api/somiod
        [Route("")]
        [HttpPost]
        public IHttpActionResult PostApplication([FromBody] Application application)
        {
            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Application.xsd");

            if (!XMLHandler.ValidateWithXSD(application, xsdPath, out string validationError))
            {
                return BadRequest(validationError);
            }

            if (application.Name == "application")
            {
                return BadRequest("The application name 'application' is reserved and cannot be used.");
            }

            Application createdApp;

            try
            {
                createdApp = ApplicationHandler.AddApplicationToDatabase(application);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.Created, createdApp, new XmlMediaTypeFormatter());
        }

        // DELETE: api/somiod/{application}
        [Route("{application}")]
        [HttpDelete]
        public IHttpActionResult DeleteApplication(string application)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }

            try
            {
                ApplicationHandler.DeleteApplicationFromDatabase(application);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/somiod/{application}
        [Route("{application}")]
        [HttpPut]
        public IHttpActionResult PutApplication(string application, [FromBody] Application newApp)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }

            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Application.xsd");

            if (!XMLHandler.ValidateWithXSD(newApp, xsdPath, out string validationError))
            {
                return BadRequest(validationError);
            }

            if (!string.Equals(application, newApp.Name, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("The application name in the URL does not match the name in the request body.");
            }

            if (newApp.Name == "application")
            {
                return BadRequest("The application name 'application' is reserved and cannot be used.");
            }

            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }

            Application updatedApp;
            try
            {
                updatedApp = ApplicationHandler.UpdateApplicationInDatabase(application, newApp);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, updatedApp, new XmlMediaTypeFormatter());
        }

        // ---End of Application---

        // ---Container---

        // GET: api/somiod/{application}/container
        [Route("{application}/container")]
        [HttpGet]
        public IHttpActionResult GetContainers(string application)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }

            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }

            List<Container> conts;

            try
            {
                conts = ContainerHandler.FindContainersByApplication(application);

                if (conts == null || !conts.Any())
                {
                    return Ok(new List<Container>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, conts, new XmlMediaTypeFormatter());
        }

        // GET: api/somiod/{application}/{container}
        [Route("{application}/{container}")]
        [HttpGet]
        public IHttpActionResult GetContainer(string application, string container)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }
            if (!ContainerHandler.ContainerExists(application, container))
            {
                return NotFound();
            }

            var locateHeader = Request.Headers.FirstOrDefault(h => h.Key.Equals("somiod-locate", StringComparison.OrdinalIgnoreCase));
            if (locateHeader.Key != null)
            {
                string locateType = locateHeader.Value.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(locateType))
                {
                    return BadRequest("Invalid somiod-locate header value.");
                }

                try
                {
                    switch (locateType.ToLower())
                    {
                        case "record":
                            var records = RecordHandler.FindRecordsByContainer(application, container);
                            if (records == null || !records.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), new XmlMediaTypeFormatter());
                            }
                            return Content(HttpStatusCode.OK, records.Select(r => r.Name), new XmlMediaTypeFormatter());

                        case "notification":
                            var notifications = NotificationHandler.FindNotificationsByContainer(application, container);
                            if (notifications == null || !notifications.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), new XmlMediaTypeFormatter());
                            }
                            return Content(HttpStatusCode.OK, notifications.Select(n => n.Name), new XmlMediaTypeFormatter());

                        default:
                            return BadRequest($"Unsupported locate type: {locateType}");
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            Container cont;
            try
            {
                cont = ContainerHandler.FindContainerInDatabase(application, container);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Content(HttpStatusCode.OK, cont, new XmlMediaTypeFormatter());
        }

        // POST: api/somiod/{application}
        [Route("{application}")]
        [HttpPost]
        public IHttpActionResult PostContainer(string application, [FromBody] Container container)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }

            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }

            if (container.Name == "container")
            {
                return BadRequest("The container name 'container' is reserved and cannot be used.");
            }

            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Container.xsd");

            if (!XMLHandler.ValidateWithXSD(container, xsdPath, out string validationError))
            {
                return BadRequest(validationError);
            }

            Container createdCont;

            try
            {
                createdCont = ContainerHandler.AddContainerToDatabase(application, container);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.Created, createdCont, new XmlMediaTypeFormatter());
        }

        // DELETE: api/somiod/{application}/{container}
        [Route("{application}/{container}")]
        [HttpDelete]
        public IHttpActionResult DeleteContainer(string application, string container)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }
            if (!ContainerHandler.ContainerExists(application, container))
            {
                return NotFound();
            }
            try
            {
                ContainerHandler.DeleteContainerFromDatabase(application, container);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/somiod/{application}/{container}
        [Route("{application}/{container}")]
        [HttpPut]
        public IHttpActionResult PutContainer(string application, string container, [FromBody] Container newContainer)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }
            if (!ContainerHandler.ContainerExists(application, container))
            {
                return NotFound();
            }

            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Container.xsd");

            if (!XMLHandler.ValidateWithXSD(newContainer, xsdPath, out string validationError))
            {
                return BadRequest(validationError);
            }
            if (!string.Equals(container, newContainer.Name, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("The container name in the URL does not match the name in the request body.");
            }
            Container updatedCont;
            try
            {
                updatedCont = ContainerHandler.UpdateContainerInDatabase(container, newContainer);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Content(HttpStatusCode.OK, updatedCont, new XmlMediaTypeFormatter());
        }

        // ---End of Container---

        // ---Records and Notifications---

        // GET: api/somiod/{application}/{container}/record
        [Route("{application}/{container}/record")]
        [HttpGet]
        public IHttpActionResult GetRecords(string application, string container)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }
            if (!ContainerHandler.ContainerExists(application, container))
            {
                return NotFound();
            }
            List<Record> records;
            try
            {
                records = RecordHandler.FindRecordsByContainer(application, container);
                if (records == null || !records.Any())
                {
                    return Ok(new List<Record>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Content(HttpStatusCode.OK, records, new XmlMediaTypeFormatter());
        }

        // GET: api/somiod/{application}/{container}/notification
        [Route("{application}/{container}/notification")]
        [HttpGet]
        public IHttpActionResult GetNotifications(string application, string container)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }
            if (!ContainerHandler.ContainerExists(application, container))
            {
                return NotFound();
            }
            List<Notification> notifications;
            try
            {
                notifications = NotificationHandler.FindNotificationsByContainer(application, container);
                if (notifications == null || !notifications.Any())
                {
                    return Ok(new List<Notification>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Content(HttpStatusCode.OK, notifications, new XmlMediaTypeFormatter());
        }

        // GET: api/somiod/{application}/{container}/{record}
        [Route("{application}/{container}/{record}")]
        [HttpGet]
        public IHttpActionResult GetRecord(string application, string container, string record)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(record))
            {
                return BadRequest("Record name must be provided.");
            }
            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }
            if (!ContainerHandler.ContainerExists(application, container))
            {
                return NotFound();
            }
            if (!RecordHandler.RecordExists(application, container, record))
            {
                return NotFound();
            }
            Record rec;
            try
            {
                rec = RecordHandler.FindRecordInDatabase(application, container, record);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Content(HttpStatusCode.OK, rec, new XmlMediaTypeFormatter());
        }

        // GET: api/somiod/{application}/{container}/{notification}
        [Route("{application}/{container}/{notification}")]
        [HttpGet]
        public IHttpActionResult GetNotification(string application, string container, string notification)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(notification))
            {
                return BadRequest("Notification name must be provided.");
            }
            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }
            if (!ContainerHandler.ContainerExists(application, container))
            {
                return NotFound();
            }
            if (!NotificationHandler.NotificationExists(application, container, notification))
            {
                return NotFound();
            }
            Notification not;
            try
            {
                not = NotificationHandler.FindNotificationInDatabase(application, container, notification);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Content(HttpStatusCode.OK, not, new XmlMediaTypeFormatter());
        }

        // POST: api/somiod/{application}/{container}
        [Route("{application}/{container}")]
        [HttpPost]
        public IHttpActionResult PostRecordOrNotification(string application, string container, [FromBody] Object recordOrNotification)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (recordOrNotification == null)
            {
                return BadRequest("Record or notification must be provided.");
            }
            if (!ApplicationHandler.ApplicationExists(application))
            {
                return NotFound();
            }
            if (!ContainerHandler.ContainerExists(application, container))
            {
                return NotFound();
            }

            var resourceHeader = Request.Headers.FirstOrDefault(h => h.Key.Equals("res_type", StringComparison.OrdinalIgnoreCase));
            if (resourceHeader.Key != null)
            {
                string resourceType = resourceHeader.Value.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(resourceType))
                {
                    return BadRequest("Invalid res_type header value.");
                }

                try
                {
                    switch (resourceType.ToLower())
                    {
                        case "record":
                            Record record = recordOrNotification as Record;
                            if (record.Name == "record")
                            {
                                return BadRequest("The record name 'record' is reserved and cannot be used.");
                            }
                            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Record.xsd");
                            if (!XMLHandler.ValidateWithXSD(record, xsdPath, out string validationError))
                            {
                                return BadRequest(validationError);
                            }
                            try
                            {
                                RecordHandler.AddRecordToDatabase(application, container, record);
                            }
                            catch (Exception ex)
                            {
                                return InternalServerError(ex);
                            }
                            return Content(HttpStatusCode.Created, record, new XmlMediaTypeFormatter());

                        case "notification":
                            Notification notification = recordOrNotification as Notification;
                            if (notification.Name == "notification")
                            {
                                return BadRequest("The notification name 'notification' is reserved and cannot be used.");
                            }
                            xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Notification.xsd");
                            if (!XMLHandler.ValidateWithXSD(notification, xsdPath, out validationError))
                            {
                                return BadRequest(validationError);
                            }
                            try
                            {
                                NotificationHandler.AddNotificationToDatabase(application, container, notification);
                            }
                            catch (Exception ex)
                            {
                                return InternalServerError(ex);
                            }
                            return Content(HttpStatusCode.Created, notification, new XmlMediaTypeFormatter());

                        default:
                            return BadRequest($"Unsupported res_type type: {resourceType}");
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest("Must specify res_type value in header.");
        }

        // DELETE: api/somiod/{application}/{container}/{record}
        //[Route("{application}/{container}/{record}")]
    }
}

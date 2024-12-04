using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using Container = somiod.Models.Container;

namespace somiod.Controllers
{
    [RoutePrefix("api/somiod")]
    public class SomiodController : ApiController
    {
        // ---------------------------- XML Formatter ---------------------------

        XmlMediaTypeFormatter xmlFormatter = new XmlMediaTypeFormatter
        {
            Indent = true,
            UseXmlSerializer = true
        };

        // ---------------------------- Applications ----------------------------

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
                            var applications = ApplicationHelper.FindApplicationsInDatabase();

                            if (applications == null || !applications.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }

                            return Content(HttpStatusCode.OK, applications.Select(a => a.Name), xmlFormatter);

                        case "container":
                            var containers = ContainerHelper.FindContainersInDatabase();
                            if (containers == null || !containers.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }
                            return Content(HttpStatusCode.OK, containers.Select(c => c.Name), xmlFormatter);

                        case "record":
                            var records = RecordHelper.FindRecordsInDatabase();
                            if (records == null || !records.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }
                            return Content(HttpStatusCode.OK, records.Select(r => r.Name), xmlFormatter);

                        case "notification":
                            var notifications = NotificationHelper.FindNotificationsInDatabase();
                            if (notifications == null || !notifications.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }
                            return Content(HttpStatusCode.OK, notifications.Select(n => n.Name), xmlFormatter);

                        default:
                            return BadRequest($"Unsupported locate type: {locateType}");
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            var apps = new List<Application>();

            try
            {
                apps = ApplicationHelper.FindApplicationsInDatabase();

                if (apps == null || !apps.Any())
                {
                    return Ok(new List<Application>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, apps, xmlFormatter);
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

            if (!ApplicationHelper.ApplicationExists(application))
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
                        case "container":
                            var containers = ContainerHelper.FindContainersByApplication(application);

                            if (containers == null || !containers.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }

                            return Content(HttpStatusCode.OK, containers.Select(c => c.Name), xmlFormatter);

                        case "record":
                            var records = RecordHelper.FindRecordsByApplication(application);

                            if (records == null || !records.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }

                            return Content(HttpStatusCode.OK, records.Select(r => r.Name), xmlFormatter);

                        case "notification":
                            var notifications = NotificationHelper.FindNotificationsByApplication(application);

                            if (notifications == null || !notifications.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }

                            return Content(HttpStatusCode.OK, notifications.Select(n => n.Name), xmlFormatter);

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
                app = ApplicationHelper.FindApplicationInDatabase(application);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            if (app == null)
            {
                return NotFound();
            }

            return Content(HttpStatusCode.OK, app, xmlFormatter);
        }


        // POST: api/somiod
        [Route("")]
        [HttpPost]
        public IHttpActionResult PostApplication([FromBody] Application application)
        {
            string xmlApp = XMLHelper.SerializeXml(application);
            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Application.xsd");

            if (!XMLHelper.ValidateXml(xmlApp, xsdPath, out string validationError))
            {
                return BadRequest($"XML validation failed: {validationError}");
            }

            if (application == null || string.IsNullOrWhiteSpace(application.Name))
            {
                return BadRequest("Application must be provided.");
            }

            if (application.Name.ToLower() == "application")
            {
                return BadRequest("The application name 'application' is reserved and cannot be used.");
            }

            Application createdApp;

            try
            {
                createdApp = ApplicationHelper.AddApplicationToDatabase(application);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.Created, createdApp, xmlFormatter);
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

            if (!ApplicationHelper.ApplicationExists(application))
            {
                return NotFound();
            }

            try
            {
                ApplicationHelper.DeleteApplicationFromDatabase(application);
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

            if (!ApplicationHelper.ApplicationExists(application))
            {
                return NotFound();
            }

            string xmlApp = XMLHelper.SerializeXml(newApp);
            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Application.xsd");

            if (!XMLHelper.ValidateXml(xmlApp, xsdPath, out string validationError))
            {
                return BadRequest($"XML validation failed: {validationError}");
            }

            if (newApp == null)
            {
                return BadRequest("Application must be provided.");
            }

            if (string.IsNullOrWhiteSpace(newApp.Name))
            {
                return BadRequest("New Application name must be provided.");
            }

            if (newApp.Name.ToLower() == "application")
            {
                return BadRequest("The application name 'application' is reserved and cannot be used.");
            }

            Application updatedApp;

            try
            {
                updatedApp = ApplicationHelper.UpdateApplicationInDatabase(application, newApp);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, updatedApp, xmlFormatter);
        }

        // ---------------------------- Containers ----------------------------

        // GET: api/somiod/{application}/container
        [Route("{application}/container")]
        [HttpGet]
        public IHttpActionResult GetContainers(string application)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }

            if (!ApplicationHelper.ApplicationExists(application))
            {
                return NotFound();
            }

            List<Container> conts;

            try
            {
                conts = ContainerHelper.FindContainersByApplication(application);

                if (conts == null || !conts.Any())
                {
                    return Ok(new List<Container>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, conts, xmlFormatter);
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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container))
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
                            var records = RecordHelper.FindRecordsByContainer(application, container);

                            if (records == null || !records.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }

                            return Content(HttpStatusCode.OK, records.Select(r => r.Name), xmlFormatter);

                        case "notification":
                            var notifications = NotificationHelper.FindNotificationsByContainer(application, container);

                            if (notifications == null || !notifications.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), xmlFormatter);
                            }

                            return Content(HttpStatusCode.OK, notifications.Select(n => n.Name), xmlFormatter);

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
                cont = ContainerHelper.FindContainerInDatabase(application, container);

                if (cont == null)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, cont, xmlFormatter);
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

            if (!ApplicationHelper.ApplicationExists(application))
            {
                return NotFound();
            }

            string xmlCont = XMLHelper.SerializeXml(container);
            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Container.xsd");

            if (!XMLHelper.ValidateXml(xmlCont, xsdPath, out string validationError))
            {
                return BadRequest($"XML validation failed: {validationError}");
            }

            if (container == null)
            {
                return BadRequest("Container must be provided.");
            }

            if (string.IsNullOrWhiteSpace(container.Name))
            {
                return BadRequest("New Container name must be provided.");
            }

            if (container.Name.ToLower() == "container")
            {
                return BadRequest("The container name 'container' is reserved and cannot be used.");
            }

            Container createdCont;

            try
            {
                createdCont = ContainerHelper.AddContainerToDatabase(application, container);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.Created, createdCont, xmlFormatter);
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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container))
            {
                return NotFound();
            }

            try
            {
                ContainerHelper.DeleteContainerFromDatabase(application, container);
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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container))
            {
                return NotFound();
            }

            string xmlCont = XMLHelper.SerializeXml(newContainer);
            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Container.xsd");

            if (!XMLHelper.ValidateXml(xmlCont, xsdPath, out string validationError))
            {
                return BadRequest($"XML validation failed: {validationError}");
            }

            if (container == null)
            {
                return BadRequest("Container must be provided.");
            }

            if (string.IsNullOrWhiteSpace(newContainer.Name))
            {
                return BadRequest("New Container name must be provided.");
            }

            if (newContainer.Name.ToLower() == "container")
            {
                return BadRequest("The container name 'container' is reserved and cannot be used.");
            }

            Container updatedCont;

            try
            {
                updatedCont = ContainerHelper.UpdateContainerInDatabase(application, container, newContainer);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, updatedCont, xmlFormatter);
        }

        // ---------------------------- Records and Notifications ----------------------------

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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container))
            {
                return NotFound();
            }

            List<Record> records;

            try
            {
                records = RecordHelper.FindRecordsByContainer(application, container);

                if (records == null || !records.Any())
                {
                    return Ok(new List<Record>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, records, xmlFormatter);
        }

        // GET: api/somiod/{application}/{container}/notif
        [Route("{application}/{container}/notif")]
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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container))
            {
                return NotFound();
            }

            List<Notification> notifications;

            try
            {
                notifications = NotificationHelper.FindNotificationsByContainer(application, container);

                if (notifications == null || !notifications.Any())
                {
                    return Ok(new List<Notification>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, notifications, xmlFormatter);
        }

        // GET: api/somiod/{application}/{container}/record/{record}
        [Route("{application}/{container}/record/{record}")]
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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container) ||
                !RecordHelper.RecordExists(application, container, record))
            {
                return NotFound();
            }

            Record rec;

            try
            {
                rec = RecordHelper.FindRecordInDatabase(application, container, record);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, rec, xmlFormatter);
        }

        // GET: api/somiod/{application}/{container}/notif/{notification}
        [Route("{application}/{container}/notif/{notification}")]
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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container) ||
                !NotificationHelper.NotificationExists(application, container, notification))
            {
                return NotFound();
            }

            Notification not;

            try
            {
                not = NotificationHelper.FindNotificationInDatabase(application, container, notification);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, not, xmlFormatter);
        }

        // POST: api/somiod/{application}/{container}
        [Route("{application}/{container}")]
        [HttpPost]
        public IHttpActionResult PostRecordOrNotification(string application, string container, [FromBody] XElement recordOrNotification)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }

            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container))
            {
                return NotFound();
            }

            if (recordOrNotification == null)
            {
                return BadRequest("Record or notification must be provided.");
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

                            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Record.xsd");

                            if (!XMLHelper.ValidateXml(recordOrNotification.ToString(), xsdPath, out string validationError))
                            {
                                return BadRequest($"XML validation failed: {validationError}");
                            }

                            Record record = XMLHelper.DeserializeXml<Record>(recordOrNotification.ToString());


                            if (string.IsNullOrWhiteSpace(record.Name))
                            {
                                return BadRequest("New Record name must be provided.");
                            }

                            if (record.Name.ToLower() == "record")
                            {
                                return BadRequest("The record name 'record' is reserved and cannot be used.");
                            }

                            Record createdRecord;

                            try
                            {
                                createdRecord = RecordHelper.AddRecordToDatabase(application, container, record);
                            }
                            catch (Exception ex)
                            {
                                return InternalServerError(ex);
                            }

                            return Content(HttpStatusCode.Created, createdRecord, xmlFormatter);

                        case "notification":

                            xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Notification.xsd");

                            if (!XMLHelper.ValidateXml(recordOrNotification.ToString(), xsdPath, out validationError))
                            {
                                return BadRequest($"XML validation failed: {validationError}");
                            }

                            Notification notification = XMLHelper.DeserializeXml<Notification>(recordOrNotification.ToString());


                            if (string.IsNullOrWhiteSpace(notification.Name))
                            {
                                return BadRequest("New Notification name must be provided.");
                            }

                            if (notification.Name.ToLower() == "notification")
                            {
                                return BadRequest("The notification name 'notification' is reserved and cannot be used.");
                            }

                            Notification createdNotification;

                            try
                            {
                                createdNotification = NotificationHelper.AddNotificationToDatabase(application, container, notification);
                            }
                            catch (Exception ex)
                            {
                                return InternalServerError(ex);
                            }

                            return Content(HttpStatusCode.Created, createdNotification, xmlFormatter);

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

        // DELETE: api/somiod/{application}/{container}/record/{record}
        [Route("{application}/{container}/record/{record}")]
        [HttpDelete]
        public IHttpActionResult DeleteRecord(string application, string container, string record)
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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container) ||
                !RecordHelper.RecordExists(application, container, record))
            {
                return NotFound();
            }

            try
            {
                RecordHelper.DeleteRecordFromDatabase(application, container, record);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/somiod/{application}/{container}/notif/{notification}
        [Route("{application}/{container}/notif/{notification}")]
        [HttpDelete]
        public IHttpActionResult DeleteNotification(string application, string container, string notification)
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

            if (!ApplicationHelper.ApplicationExists(application) ||
                !ContainerHelper.ContainerExists(application, container) ||
                !NotificationHelper.NotificationExists(application, container, notification))
            {
                return NotFound();
            }

            try
            {
                NotificationHelper.DeleteNotificationFromDatabase(application, container, notification);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

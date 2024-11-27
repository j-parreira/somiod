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

            try
            {
                bool isDeleted = ApplicationHandler.DeleteApplicationFromDatabase(application);

                if (!isDeleted)
                {
                    return NotFound();
                }
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
        public IHttpActionResult PutApplication(string application, [FromBody] Application app)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }

            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Application.xsd");

            if (!XMLHandler.ValidateWithXSD(application, xsdPath, out string validationError))
            {
                return BadRequest(validationError);
            }

            if (!string.Equals(application, app.Name, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("The application name in the URL does not match the name in the request body.");
            }

            try
            {
                bool isUpdated = ApplicationHandler.UpdateApplicationInDatabase(application, app);
                if (!isUpdated)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.OK, application, new XmlMediaTypeFormatter());
        }

        // ---End of Application---

        // ---Container---

        // GET: api/somiod/{application}/containers
        //[Route("{application}/containers")]
        //[HttpGet]
        //public IHttpActionResult GetContainers(string application)
        //{
        //    if (string.IsNullOrWhiteSpace(application))
        //    {
        //        return BadRequest("Application name must be provided.");
        //    }

        //    List<Container> containers;

        //    try
        //    {
        //        bool applicationExists = ApplicationHandler.ApplicationExists(application);
        //        if (!applicationExists)
        //        {
        //            return NotFound();
        //        }

        //        containers = ContainerHandler.FindContainersByApplication(application);

        //        if (containers == null || !containers.Any())
        //        {
        //            return Ok(new List<Container>());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }

        //    return Content(HttpStatusCode.OK, containers, new XmlMediaTypeFormatter());
        //}

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
                            var records = RecordHandler.FindRecordsByContainer(application);
                            if (records == null || !records.Any())
                            {
                                return Content(HttpStatusCode.OK, new List<string>(), new XmlMediaTypeFormatter());
                            }
                            return Content(HttpStatusCode.OK, records.Select(r => r.Name), new XmlMediaTypeFormatter());

                        case "notification":
                            var notifications = NotificationHandler.FindNotificationsByContainer(application);
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
                bool applicationExists = ApplicationHandler.ApplicationExists(application);
                if (!applicationExists)
                {
                    return NotFound();
                }
                cont = ContainerHandler.FindContainerInDatabase(application, container);
                if (cont == null)
                {
                    return NotFound();
                }
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

            string xsdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XSD", "Container.xsd");

            if (!XMLHandler.ValidateWithXSD(container, xsdPath, out string validationError))
            {
                return BadRequest(validationError);
            }

            Container createdCont;

            try
            {
                bool applicationExists = ApplicationHandler.ApplicationExists(application);
                if (!applicationExists)
                {
                    return NotFound();
                }
                createdCont = ContainerHandler.AddContainerToDatabase(application, container);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.Created, createdCont, new XmlMediaTypeFormatter());
        }

        /// <summary>
        /// // Estamos aqui
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>

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
            try
            {
                bool applicationExists = ApplicationHandler.ApplicationExists(application);
                if (!applicationExists)
                {
                    return NotFound();
                }
                bool isDeleted = ContainerHandler.DeleteContainerFromDatabase(application, container);
                if (!isDeleted)
                {
                    return NotFound();
                }
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
        public IHttpActionResult PutContainer(string application, string container, [FromBody] Container cont)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }
            if (string.IsNullOrWhiteSpace(container))
            {
                return BadRequest("Container name must be provided.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!string.Equals(container, cont.Name, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("The container name in the URL does not match the name in the request body.");
            }
            try
            {
                bool applicationExists = ApplicationHandler.ApplicationExists(application);
                if (!applicationExists)
                {
                    return NotFound();
                }
                bool isUpdated = ContainerHandler.UpdateContainerInDatabase(application, container, cont);
                if (!isUpdated)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Content(HttpStatusCode.OK, cont, new XmlMediaTypeFormatter());
        }

        // ---End of Container---

        // ---Records and Notifications---

        // POST: api/somiod/{application}/{container}
        [Route("{application}/{container}")]
        [HttpPost]
        public IHttpActionResult PostRecordOrNotification(string application, string container, [FromBody] RecordOrNotification recordOrNotification)
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

            try
            {
                if (recordOrNotification is Record)
                {
                    RecordHandler.AddRecordToDatabase(application, container, recordOrNotification as Record);
                    return Content(HttpStatusCode.Created, recordOrNotification, new XmlMediaTypeFormatter());
                }
                else if (recordOrNotification is Notification)
                {
                    NotificationHandler.AddNotificationToDatabase(application, container, recordOrNotification as Notification);
                    return Content(HttpStatusCode.Created, recordOrNotification, new XmlMediaTypeFormatter());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest("Invalid record or notification.");
        }

        // DELETE: api/somiod/{application}/{container}/{record}
        [Route("{application}/{container}/{record}")]
    }
}

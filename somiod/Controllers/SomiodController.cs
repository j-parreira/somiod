using somiod.Handlers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

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
            List<Application> apps = new List<Application>();

            try
            {
                apps = ApplicationHandler.FindApplicationsInDatabase();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(apps);
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

            return Ok(app);
        }

        // POST: api/somiod
        [Route("")]
        [HttpPost]
        public IHttpActionResult PostApplication([FromBody] Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            return CreatedAtRoute("GetApplication", new { application = createdApp.Name }, createdApp);
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            return Ok(app);
        }

        // ---End of Application---

        // ---Container---

        // GET: api/somiod/{application}/containers
        [Route("{application}/containers")]
        [HttpGet]
        public IHttpActionResult GetContainers(string application)
        {
            if (string.IsNullOrWhiteSpace(application))
            {
                return BadRequest("Application name must be provided.");
            }

            List<Container> containers;

            try
            {
                bool applicationExists = ApplicationHandler.ApplicationExists(application);
                if (!applicationExists)
                {
                    return NotFound();
                }

                containers = ContainerHandler.FindContainersInDatabase(application);

                if (containers == null || !containers.Any())
                {
                    return Ok(new List<Container>());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(containers);
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
            return Ok(cont);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            return CreatedAtRoute("GetContainer", new { application, container = createdCont.Name }, createdCont);
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
            return Ok(cont);
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
                    return Created(new Uri($"{Request.RequestUri}/{recordOrNotification.Name}"), recordOrNotification);
                }
                else if (recordOrNotification is Notification)
                {
                    NotificationHandler.AddNotificationToDatabase(application, container, recordOrNotification as Notification);
                    return Created(new Uri($"{Request.RequestUri}/{recordOrNotification.Name}"), recordOrNotification);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}

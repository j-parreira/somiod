using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace somiod.Controllers
{
    [RoutePrefix("api/applications")]
    public class ApplicationsController : ApiController
    {
        string connectionString = Properties.Settings.Default.ConnStr;
    }
}

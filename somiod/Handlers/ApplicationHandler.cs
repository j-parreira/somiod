using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace somiod.Handlers
{
    public class ApplicationHandler
    {
        protected SqlConnection sqlConnection;
        protected String connectionString = Properties.Settings.Default.ConnStr;
        protected string sqlCommand = "";
    }
}
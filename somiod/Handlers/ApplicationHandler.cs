using somiod.Models;
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

        internal static List<Application> FindApplicationsInDatabase()
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static Application FindApplicationInDatabase(string application)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static Application AddApplicationToDatabase(Application app)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static void DeleteApplicationFromDatabase(string application)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static Application UpdateApplicationInDatabase(string application, Application newApp)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static bool ApplicationExists(string application)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }
    }
}
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace somiod.Handlers
{
    public class ContainerHandler
    {
        protected SqlConnection sqlConnection;
        protected String connectionString = Properties.Settings.Default.ConnStr;
        protected string sqlCommand = "";

        internal static List<Container> FindContainersByApplication(string application)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static Container FindContainerInDatabase(string application, string container)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static Container AddContainerToDatabase(string application, Container container)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static void DeleteContainerFromDatabase(string application, string container)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }
        internal static Container UpdateContainerInDatabase(string container, Container newContainer)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }
        internal static bool ContainerExists(string application, string container)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }
    }
}
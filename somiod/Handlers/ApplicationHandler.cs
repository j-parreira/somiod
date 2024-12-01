using somiod.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            List<Application> applications = new List<Application>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Applications", sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                applications.Add(new Application
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2)
                                });
                            }
                            reader.Close();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error finding applications in database", e);
            }
            return applications;
        }

        internal static Application FindApplicationInDatabase(string application)
        {
            Application app = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Applications WHERE Name = @ApplicationName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationName", application);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                app = new Application
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2)
                                };
                            }
                            reader.Close();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error finding application in database", e);
            }
            return app;
        }

        internal static Application AddApplicationToDatabase(Application app)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static void DeleteApplicationFromDatabase(string application)
        {
            Application app = FindApplicationInDatabase(application);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Applications WHERE Id = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", app.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting application from database", e);
            }
        }

        internal static Application UpdateApplicationInDatabase(string application, Application newApp)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }

        internal static bool ApplicationExists(string application)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Applications WHERE Name = @ApplicationName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationName", application);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        int count = (int)sqlCommand.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if container exists", ex);
            }
        }
    }
}

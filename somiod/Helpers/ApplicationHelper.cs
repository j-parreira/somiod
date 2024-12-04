using somiod.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace somiod.Helpers
{
    public class ApplicationHelper
    {
        internal static bool ApplicationExists(string application)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Applications " +
                        "WHERE Name = @ApplicationName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationName", application.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        int count = (int)sqlCommand.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if application exists", ex);
            }
        }

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
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Applications " +
                        "WHERE Name = @ApplicationName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationName", application.ToLower());
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

        internal static Application AddApplicationToDatabase(Application application)
        {
            
            if (ApplicationExists(application.Name))
            {
                int i = 1;
                while (ApplicationExists(application.Name))
                {
                    application.Name += i.ToString();
                    i++;
                }
            }

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Applications " +
                        "(Name, CreationDateTime) VALUES (@ApplicationName, @CreationDateTime)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationName", application.Name.ToLower());
                        sqlCommand.Parameters.AddWithValue("@CreationDateTime", DateTime.Now);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error adding application to database", e);
            }
            application = FindApplicationInDatabase(application.Name);
            return application;
        }

        internal static void DeleteApplicationFromDatabase(string application)
        {
            Application app = FindApplicationInDatabase(application);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Applications " +
                        "WHERE Id = @ApplicationId", sqlConnection))
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
            var app = FindApplicationInDatabase(application);

            if (ApplicationExists(newApp.Name))
            {
                int i = 1;
                while (ApplicationExists(newApp.Name))
                {
                    newApp.Name += i.ToString();
                    i++;
                }
            }

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("UPDATE Applications " +
                        "SET Name = @NewName WHERE Id = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NewName", newApp.Name.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", app.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error updating application in database", e);
            }
            newApp = FindApplicationInDatabase(newApp.Name);
            return newApp;
        }
    }
}

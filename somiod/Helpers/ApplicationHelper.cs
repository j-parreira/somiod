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
        // Check if application exists in database
        internal static bool ApplicationExists(string application)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM applications " +
                        "WHERE name = @ApplicationName", sqlConnection))
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

        // Find all applications in database
        internal static List<Application> FindApplicationsInDatabase()
        {
            var apps = new List<Application>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM applications", sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                apps.Add(new Application
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2)
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
                throw new Exception("Error finding apps in database", e);
            }
            return apps;
        }

        // Find application in database
        public static Application FindApplicationInDatabase(string application)
        {
            Application app = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM applications " +
                        "WHERE name = @ApplicationName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationName", application.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                app = new Application
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2)
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

        // Add application to database
        internal static Application AddApplicationToDatabase(Application application)
        {
            if (ApplicationExists(application.name))
            {
                int i = 1;
                string newName = application.name;
                while (ApplicationExists(newName))
                {
                    newName = application.name + i.ToString();
                    i++;
                }
                application.name = newName;
            }
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO applications " +
                        "(name, creation_datetime) VALUES (@ApplicationName, @CreationDateTime)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationName", application.name.ToLower());
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
            application = FindApplicationInDatabase(application.name);
            return application;
        }

        // Delete application from database
        internal static void DeleteApplicationFromDatabase(string application)
        {
            var app = FindApplicationInDatabase(application);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM applications " +
                        "WHERE id = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", app.id);
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

        // Update application in database
        internal static Application UpdateApplicationInDatabase(string application, Application newApp)
        {
            var app = FindApplicationInDatabase(application);

            if (ApplicationExists(newApp.name))
            {
                int i = 1;
                while (ApplicationExists(newApp.name))
                {
                    newApp.name += i.ToString();
                    i++;
                }
            }
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("UPDATE applications " +
                        "SET name = @NewName WHERE id = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NewName", newApp.name.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", app.id);
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
            newApp = FindApplicationInDatabase(newApp.name);
            return newApp;
        }
    }
}

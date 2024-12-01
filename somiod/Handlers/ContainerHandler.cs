using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace somiod.Handlers
{
    public class ContainerHandler
    {
        internal static List<Container> FindContainersByApplication(string application)
        {
            List<Container> containers = new List<Container>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Containers WHERE Parent = @ApplicationId", sqlConnection))
                    {
                        int appId = ApplicationHandler.FindApplicationInDatabase(application).Id;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", appId);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                containers.Add(new Container
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3)
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
                throw new Exception("Error finding containers by application", e);
            }
            return containers;
        }

        internal static Container FindContainerInDatabase(string application, string container)
        {
            Container cont = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Containers WHERE Name = @ContainerName AND Parent = @ApplicationId", sqlConnection))
                    {
                        int appId = ApplicationHandler.FindApplicationInDatabase(application).Id;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", appId);
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cont = new Container
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3)
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
                throw new Exception("Error finding container by application", e);
            }
            return cont;
        }

        internal static Container AddContainerToDatabase(string application, Container container)
        {
            var app = ApplicationHandler.FindApplicationInDatabase(application);
            var cont = FindContainerInDatabase(application, container.Name);
            if (cont != null)
            {
                int i = 1;
                string newName = "";
                while (cont != null)
                {
                    newName = container.Name + "_" + i.ToString();
                    cont = FindContainerInDatabase(application, newName);
                    i++;
                }
                container.Name = newName;
            }
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Containers (Name, CreationDateTime, Parent) VALUES (@Name, @DateTime, @Parent)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Name", container.Name);
                        sqlCommand.Parameters.AddWithValue("@DateTime", container.CreationDateTime);
                        sqlCommand.Parameters.AddWithValue("@Parent", app.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error adding container to database", e);
            }
            container = FindContainerInDatabase(application, container.Name);
            return container;
        }

        internal static void DeleteContainerFromDatabase(string application, string container)
        {
            var app = ApplicationHandler.FindApplicationInDatabase(application);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Containers WHERE Name = @ContainerName AND Parent = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container);
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", app.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting container from database", e);
            }
        }
        internal static Container UpdateContainerInDatabase(string application, string container, Container newContainer)
        {
            throw new NotImplementedException(); //TODO: Implement this method
        }
        internal static bool ContainerExists(string application, string container)
        {
            try
            {
                var app = ApplicationHandler.FindApplicationInDatabase(application);
                if (app == null)
                {
                    return false;
                }

                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Containers WHERE Name = @ContainerName AND Parent = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container);
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", app.Id);
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
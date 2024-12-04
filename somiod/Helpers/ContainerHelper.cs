using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace somiod.Helpers
{
    public class ContainerHelper
    {
        internal static bool ContainerExists(string application, string container)
        {
            Application app = ApplicationHelper.FindApplicationInDatabase(application); 
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Containers WHERE Name = @ContainerName AND Parent = @ParentId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ParentId", app.Id);
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
                        int appId = ApplicationHelper.FindApplicationInDatabase(application).Id;
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
                        int appId = ApplicationHelper.FindApplicationInDatabase(application).Id;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", appId);
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.ToLower());
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
            if (ContainerExists(application, container.Name))
            {
                int i = 1;
                while (ContainerExists(application, container.Name))
                {
                    container.Name += i.ToString();
                    i++;
                }
            }

            var app = ApplicationHelper.FindApplicationInDatabase(application);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Containers (Name, CreationDateTime, Parent) VALUES (@ContainerName, @CreationDateTime, @ParentId)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.Name.ToLower());
                        sqlCommand.Parameters.AddWithValue("@CreationDateTime", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@ParentId", app.Id);
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
            var app = ApplicationHelper.FindApplicationInDatabase(application);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Containers WHERE Name = @ContainerName AND Parent = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.ToLower());
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
            if (ContainerExists(application, newContainer.Name))
            {
                int i = 1;
                while (ContainerExists(application, newContainer.Name))
                {
                    newContainer.Name += i.ToString();
                    i++;
                }
            }

            var app = ApplicationHelper.FindApplicationInDatabase(application);
            
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("UPDATE Containers SET Name = @NewContainerName WHERE Name = @ContainerName AND Parent = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NewContainerName", newContainer.Name.ToLower());
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
                throw new Exception("Error updating container in database", e);
            }
            var NewCont = FindContainerInDatabase(application, newContainer.Name);
            return NewCont;
        }
    }
}
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace somiod.Helpers
{
    public class ContainerHelper
    {
        internal static bool ContainerNameExists(string container)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM containers " +
                        "WHERE name = @ContainerName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        int count = (int)sqlCommand.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if container name exists", ex);
            }
        }

        internal static bool ContainerExists(string application, string container)
        {
            var app = ApplicationHelper.FindApplicationInDatabase(application); 
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) " +
                        "FROM containers WHERE name = @ContainerName AND parent = @ParentId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ParentId", app.id);
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

        internal static List<Container> FindContainersInDatabase()
        {
            var containers = new List<Container>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM containers", sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                containers.Add(new Container
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3)
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

        internal static List<Container> FindContainersByApplication(string application)
        {
            var containers = new List<Container>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM containers " +
                        "WHERE parent = @ApplicationId", sqlConnection))
                    {
                        int appId = ApplicationHelper.FindApplicationInDatabase(application).id;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", appId);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                containers.Add(new Container
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3)
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
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM containers " +
                        "WHERE name = @ContainerName AND parent = @ApplicationId", sqlConnection))
                    {
                        int appId = ApplicationHelper.FindApplicationInDatabase(application).id;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", appId);
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cont = new Container
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3)
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
            if (ContainerNameExists(container.name))
            {
                int i = 1;
                while (ContainerNameExists(container.name))
                {
                    container.name += i.ToString();
                    i++;
                }
            }
            var app = ApplicationHelper.FindApplicationInDatabase(application);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO containers " +
                        "(name, creation_datetime, parent) " +
                        "VALUES (@ContainerName, @CreationDateTime, @ParentId)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.name.ToLower());
                        sqlCommand.Parameters.AddWithValue("@CreationDateTime", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@ParentId", app.id);
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
            container = FindContainerInDatabase(application, container.name);
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
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM containers " +
                        "WHERE name = @ContainerName AND parent = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", app.id);
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
            if (ContainerNameExists(newContainer.name))
            {
                int i = 1;
                while (ContainerNameExists(newContainer.name))
                {
                    newContainer.name += i.ToString();
                    i++;
                }
            }
            var app = ApplicationHelper.FindApplicationInDatabase(application);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("UPDATE containers " +
                        "SET name = @NewContainerName " +
                        "WHERE name = @ContainerName AND parent = @ApplicationId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NewContainerName", newContainer.name.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ContainerName", container);
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", app.id);
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
            var NewCont = FindContainerInDatabase(application, newContainer.name);
            return NewCont;
        }
    }
}
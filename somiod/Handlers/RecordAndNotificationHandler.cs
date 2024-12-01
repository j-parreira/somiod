using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace somiod.Handlers
{
    public class RecordAndNotificationHandler
    {
        // ---------------------------- Records ----------------------------
        internal static bool RecordExists(string application, string container, string record)
        {
            try
            {
                var cont = ContainerHandler.FindContainerInDatabase(application, container);
                if (cont == null)
                {
                    return false;
                }

                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Records WHERE Name = @RecordName AND Parent = @ParentId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", record);
                        sqlCommand.Parameters.AddWithValue("@ParentId", cont.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        int count = (int)sqlCommand.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error checking if record exists", e);
            }
        }

        internal static List<Record> FindRecordsByApplication(string application)
        {
            Application app = ApplicationHandler.FindApplicationInDatabase(application);
            List<Record> records = new List<Record>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Records WHERE Parent IN (SELECT Id FROM Containers WHERE Parent = @ParentId)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ParentId", app.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                records.Add(new Record
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Content = reader.GetString(2),
                                    CreationDateTime = reader.GetDateTime(3),
                                    Parent = reader.GetInt32(4)
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
                throw new Exception("Error finding records by application", e);
            }
            return records;
        }

        internal static List<Record> FindRecordsByContainer(string application, string container)
        {
            Container cont = ContainerHandler.FindContainerInDatabase(application, container);
            List<Record> records = new List<Record>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Records WHERE Parent = @ParentId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ParentId", cont.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                records.Add(new Record
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Content = reader.GetString(2),
                                    CreationDateTime = reader.GetDateTime(3),
                                    Parent = reader.GetInt32(4)
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
                throw new Exception("Error finding records by container", e);
            }
            return records;
        }

        internal static Record FindRecordInDatabase(string application, string container, string record)
        {
            Record rec = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Records WHERE Name = @RecordName AND Parent = @ContainerId", sqlConnection))
                    {
                        int contId = ContainerHandler.FindContainerInDatabase(application, container).Id;
                        sqlCommand.Parameters.AddWithValue("@ContainerId", contId);
                        sqlCommand.Parameters.AddWithValue("@RecordName", record);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rec = new Record
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Content = reader.GetString(2),
                                    CreationDateTime = reader.GetDateTime(3),
                                    Parent = reader.GetInt32(4)
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
            return rec;
        }

        internal static void AddRecordToDatabase(string application, string container, Record record)
        {
            throw new NotImplementedException();
        }

        internal static void DeleteRecordFromDatabase(string application, string container, string record)
        {
            throw new NotImplementedException();
        }

        // ---------------------------- Notifications ----------------------------
        internal static bool NotificationExists(string application, string container, string notification)
        {
            try
            {
                var cont = ContainerHandler.FindContainerInDatabase(application, container);
                if (cont == null)
                {
                    return false;
                }
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Notifications WHERE Name = @NotificationName AND Parent = @ParentId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification);
                        sqlCommand.Parameters.AddWithValue("@ParentId", cont.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        int count = (int)sqlCommand.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error checking if notification exists", e);
            }
        }

        internal static List<Notification> FindNotificationsByApplication(string application)
        {
            Application app = ApplicationHandler.FindApplicationInDatabase(application);
            List<Notification> notifications = new List<Notification>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Notifications WHERE Parent IN (SELECT Id FROM Containers WHERE Parent = @ParentId)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ParentId", app.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifications.Add(new Notification
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3),
                                    Event = reader.GetInt32(4),
                                    Endpoint = reader.GetString(5),
                                    Enabled = reader.GetBoolean(6)
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
                throw new Exception("Error finding notifications by application", e);
            }
            return notifications;
        }

        internal static List<Notification> FindNotificationsByContainer(string application, string container)
        {
            Container cont = ContainerHandler.FindContainerInDatabase(application, container);
            List<Notification> notifications = new List<Notification>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Notifications WHERE Parent = @ParentId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ParentId", cont.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifications.Add(new Notification
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3),
                                    Event = reader.GetInt32(4),
                                    Endpoint = reader.GetString(5),
                                    Enabled = reader.GetBoolean(6)
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
                throw new Exception("Error finding notifications by container", e);
            }
            return notifications;
        }

        internal static Notification FindNotificationInDatabase(string application, string container, string notification)
        {
            Notification not = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Notifications WHERE Name = @NotificationName AND Parent = @ContainerId", sqlConnection))
                    {
                        int contId = ContainerHandler.FindContainerInDatabase(application, container).Id;
                        sqlCommand.Parameters.AddWithValue("@ContainerId", contId);
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                not = new Notification
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3),
                                    Event = reader.GetInt32(4),
                                    Endpoint = reader.GetString(5),
                                    Enabled = reader.GetBoolean(6)
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
            return not;
        }

        internal static void AddNotificationToDatabase(string application, string container, Notification notification)
        {
            throw new NotImplementedException();
        }

        internal static void DeleteNotificationFromDatabase(string application, string container, string notification)
        {
            throw new NotImplementedException();
        }
    }
}
using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace somiod.Helpers
{
    public class NotificationHelper
    {
        internal static bool NotificationExists(string notification)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Notifications WHERE Name = @NotificationName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.ToLower());
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
            Application app = ApplicationHelper.FindApplicationInDatabase(application);
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
                                    Event = reader.GetString(4),
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
            Container cont = ContainerHelper.FindContainerInDatabase(application, container);
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
                                    Event = reader.GetString(4),
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
                        int contId = ContainerHelper.FindContainerInDatabase(application, container).Id;
                        sqlCommand.Parameters.AddWithValue("@ContainerId", contId);
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.ToLower());
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
                                    Event = reader.GetString(4),
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

        internal static Notification AddNotificationToDatabase(string application, string container, Notification notification)
        {
            if (notification.Event != "0" && notification.Event != "1" && notification.Event != "2")
            {
                throw new Exception("Invalid event type");
            }

            if (NotificationExists(notification.Name))
            {
                int i = 1;
                while (NotificationExists(notification.Name))
                {
                    notification.Name += i.ToString();
                    i++;
                }
            }

            var cont = ContainerHelper.FindContainerInDatabase(application, container);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Notifications (Name, CreationDateTime, Parent, Event, Endpoint, Enabled) VALUES (@NotificationName, @NotificationCreationDateTime, @NotificationParent, @NotificationEvent, @NotificationEndpoint, @NotificationEnabled)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.Name.ToLower());
                        sqlCommand.Parameters.AddWithValue("@NotificationCreationDateTime", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@NotificationParent", cont.Id);
                        sqlCommand.Parameters.AddWithValue("@NotificationEvent", notification.Event);
                        sqlCommand.Parameters.AddWithValue("@NotificationEndpoint", notification.Endpoint);
                        sqlCommand.Parameters.AddWithValue("@NotificationEnabled", notification.Enabled);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error adding notification to database", e);
            }
            notification = FindNotificationInDatabase(application, container, notification.Name);
            return notification;
        }

        internal static void DeleteNotificationFromDatabase(string application, string container, string notification)
        {
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Notifications WHERE Name = @NotificationName AND Parent = @ContainerId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ContainerId", cont.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting notification from database", e);
            }
        }
    }
}
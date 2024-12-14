using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace somiod.Helpers
{
    public class NotificationHelper
    {
        // Check if notification name exists in database
        internal static bool NotificationNameExists(string notificationName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM notifications " +
                        "WHERE name = @NotificationName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notificationName.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        int count = (int)sqlCommand.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error checking if notification name exists", e);
            }
        }

        // Check if notification exists in database under a container of an application
        internal static bool NotificationExists(string application, string container, string notification)
        {   
            var app = ApplicationHelper.FindApplicationInDatabase(application);
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1)" +
                        "FROM notifications n " +
                        "INNER JOIN containers c ON n.parent = c.id " +
                        "WHERE n.name = @NotificationName AND n.parent = @NotificationParent " +
                        "AND c.parent = @ContainerParent", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.ToLower());
                        sqlCommand.Parameters.AddWithValue("@NotificationParent", cont.id);
                        sqlCommand.Parameters.AddWithValue("@ContainerParent", app.id);
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

        // Find all notifications in database
        internal static List<Notification> FindNotificationsInDatabase()
        {
            var notifications = new List<Notification>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM notifications", sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifications.Add(new Notification
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3),
                                    event_type = reader.GetString(4),
                                    endpoint = reader.GetString(5),
                                    enabled = reader.GetBoolean(6)
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

        // Find notifications by application
        internal static List<Notification> FindNotificationsByApplication(string application)
        {
            var app = ApplicationHelper.FindApplicationInDatabase(application);
            var notifications = new List<Notification>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * " +
                        "FROM notifications WHERE parent IN " +
                        "(SELECT id FROM containers WHERE parent = @ParentId)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ParentId", app.id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifications.Add(new Notification
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3),
                                    event_type = reader.GetString(4),
                                    endpoint = reader.GetString(5),
                                    enabled = reader.GetBoolean(6)
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

        // Find notifications by container
        internal static List<Notification> FindNotificationsByContainer(string application, string container)
        {
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            var notifications = new List<Notification>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM notifications " +
                        "WHERE parent = @ParentId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ParentId", cont.id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifications.Add(new Notification
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3),
                                    event_type = reader.GetString(4),
                                    endpoint = reader.GetString(5),
                                    enabled = reader.GetBoolean(6)
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

        // Find notification in database under a container of an application
        internal static Notification FindNotificationInDatabase(string application, string container, string notification)
        {
            Notification not = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM notifications " +
                        "WHERE name = @NotificationName AND parent = @ContainerId", sqlConnection))
                    {
                        int contId = ContainerHelper.FindContainerInDatabase(application, container).id;
                        sqlCommand.Parameters.AddWithValue("@ContainerId", contId);
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                not = new Notification
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3),
                                    event_type = reader.GetString(4),
                                    endpoint = reader.GetString(5),
                                    enabled = reader.GetBoolean(6)
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

        // Add notification to database under a container of an application
        internal static Notification AddNotificationToDatabase(string application, string container, Notification notification)
        {
            if (notification.event_type != "0" && notification.event_type != "1" && notification.event_type != "2")
            {
                throw new Exception("Invalid event type");
            }
            if (NotificationNameExists(notification.name))
            {
                int i = 1;
                string newName = notification.name;
                while (NotificationNameExists(newName))
                {
                    newName = notification.name + i.ToString();
                    i++;
                }
                notification.name = newName;
            }
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO notifications " +
                        "(name, creation_datetime, parent, event_type, endpoint, enabled) " +
                        "VALUES (@NotificationName, @NotificationCreationDateTime, @NotificationParent, " +
                        "@NotificationEvent, @NotificationEndpoint, @NotificationEnabled)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.name.ToLower());
                        sqlCommand.Parameters.AddWithValue("@NotificationCreationDateTime", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@NotificationParent", cont.id);
                        sqlCommand.Parameters.AddWithValue("@NotificationEvent", notification.event_type);
                        sqlCommand.Parameters.AddWithValue("@NotificationEndpoint", notification.endpoint);
                        sqlCommand.Parameters.AddWithValue("@NotificationEnabled", notification.enabled);
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
            notification = FindNotificationInDatabase(application, container, notification.name);
            return notification;
        }

        // Update notification in database under a container of an application
        internal static void DeleteNotificationFromDatabase(string application, string container, string notification)
        {
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM notifications " +
                        "WHERE name = @NotificationName AND parent = @ContainerId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ContainerId", cont.id);
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
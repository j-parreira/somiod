using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using uPLibrary.Networking.M2Mqtt;

namespace somiod.Handlers
{
    public class RecordAndNotificationHandler
    {
        // ---------------------------- Records ----------------------------
        internal static bool RecordExists(string record)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Records WHERE Name = @RecordName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", record);
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

        internal static Record AddRecordToDatabase(string application, string container, Record record)
        {
            if (RecordExists(record.Name))
            {
                int i = 1;
                while (RecordExists(record.Name))
                {
                    record.Name += i.ToString();
                    i++;
                }
            }

            var cont = ContainerHandler.FindContainerInDatabase(application, container);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Records (Name, Content, CreationDateTime, Parent) VALUES (@RecordName, @RecordContent, @RecordCreationDateTime, @RecordParent)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", record.Name);
                        sqlCommand.Parameters.AddWithValue("@RecordContent", record.Content);
                        sqlCommand.Parameters.AddWithValue("@RecordCreationDateTime", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@RecordParent", cont.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error adding record to database", e);
            }

            record = FindRecordInDatabase(application, container, record.Name);

            var channel = ContainerHandler.FindContainerInDatabase(application, container).Name;
            var content = FindRecordInDatabase(application, container, record.Name).Content;
            List<Notification> notificationsToSend = FindNotificationsToSend(application, container, 1);
            PublishNotifications(notificationsToSend, channel, record, "creation");

            return record;
        }

        internal static void DeleteRecordFromDatabase(string application, string container, string record)
        {
            var channel = ContainerHandler.FindContainerInDatabase(application, container).Name;
            var rec = FindRecordInDatabase(application, container, record);
            List<Notification> notificationsToSend = FindNotificationsToSend(application, container, 2);
            PublishNotifications(notificationsToSend, channel, rec, "deletion");

            var cont = ContainerHandler.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Records WHERE Name = @RecordName AND Parent = @ContainerId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", record);
                        sqlCommand.Parameters.AddWithValue("@ContainerId", cont.Id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting record from database", e);
            }
        }

        // ---------------------------- Notifications ----------------------------
        internal static bool NotificationExists(string notification)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Notifications WHERE Name = @NotificationName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification);
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

        internal static Notification AddNotificationToDatabase(string application, string container, Notification notification)
        {
            if (NotificationExists(notification.Name))
            {
                int i = 1;
                while (NotificationExists(notification.Name))
                {
                    notification.Name += i.ToString();
                    i++;
                }
            }

            var cont = ContainerHandler.FindContainerInDatabase(application, container);

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Notifications (Name, CreationDateTime, Parent, Event, Endpoint, Enabled) VALUES (@NotificationName, @NotificationCreationDateTime, @NotificationParent, @NotificationEvent, @NotificationEndpoint, @NotificationEnabled)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification.Name);
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
            var cont = ContainerHandler.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Notifications WHERE Name = @NotificationName AND Parent = @ContainerId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@NotificationName", notification);
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

        // ---------------------------- MQTT & HTTP ----------------------------

        private static List<Notification> FindNotificationsToSend(string application, string container, int fireEvent)
        {
            var containerId = ContainerHandler.FindContainerInDatabase(application, container).Id;
            List<Notification> notifications = new List<Notification>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Notifications WHERE Parent = @ContainerId AND Enabled = 1 AND (Event = 0 OR EVENT = @fireEvent", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerId", containerId);
                        sqlCommand.Parameters.AddWithValue("@fireEvent", fireEvent);
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
                throw new Exception("Error finding notifications to send", e);
            }
            return notifications;
        }

        private static void PublishNotifications(List<Notification> notificationsToSend, string channel, Record record, string fireEvent)
        {
            MqttClient mClient;
            string message = XMLHandler.SerializeXml(fireEvent) + XMLHandler.SerializeXml(record);
            byte qos = uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE;

            try
            {
                var endpoints = notificationsToSend.Select(n => n.Endpoint).ToList();
                foreach (var endpoint in endpoints)
                {
                    mClient = new MqttClient(endpoint);
                    mClient.Connect(Guid.NewGuid().ToString());
                    if (!mClient.IsConnected)
                    {
                        throw new Exception("Error connecting to message broker...");
                    }
                    mClient.Publish(channel, System.Text.Encoding.UTF8.GetBytes(message), qos, false);
                    mClient.Disconnect();
                }

            }
            catch (Exception e)
            {
                throw new Exception("Error publishing notifications", e);
            }
        }
    }
}
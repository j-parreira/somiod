using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace somiod.Helpers
{
    public class MqttHelper
    {
        internal static List<Notification> FindNotificationsToSend(string application, string container, string evento)
        {
            var containerId = ContainerHelper.FindContainerInDatabase(application, container).Id;
            var notifications = new List<Notification>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Notifications " +
                        "WHERE Parent = @ContainerId AND Enabled = 1 " +
                        "AND (Event = 0 OR Event = @Event)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerId", containerId);
                        sqlCommand.Parameters.AddWithValue("@Event", evento);
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
                throw new Exception("Error finding notifications to send", e);
            }
            return notifications;
        }

        internal static void PublishNotifications(List<Notification> notificationsToSend, MqttMessage mqttMessage)
        {
            MqttClient mClient;
            string message = XMLHelper.SerializeXml(mqttMessage);
            byte qos = MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE;

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
                    mClient.Publish(mqttMessage.Topic, System.Text.Encoding.UTF8.GetBytes(message), qos, false);
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
using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace somiod.Helpers
{
    public class MqttHelper
    {
        internal static List<String> FindEndpointsToSend(string application, string container, string NotifEvent)
        {
            var containerId = ContainerHelper.FindContainerInDatabase(application, container).Id;
            var notifications = new List<Notification>();
            List<string> endpoints = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Notifications " +
                        "WHERE Parent = @ContainerId AND Enabled = 1 " +
                        "AND (Event = @EventAll OR Event = @Event)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerId", containerId);
                        sqlCommand.Parameters.AddWithValue("@EventAll", "0");
                        sqlCommand.Parameters.AddWithValue("@Event", NotifEvent);
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
                endpoints = notifications.Select(n => n.Endpoint).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error finding notifications to send", e);
            }
            Console.WriteLine("Endpoints to send: " + string.Join(", ", endpoints));
            return endpoints;
        }

        internal static void PublishRecord(List<String> endpointsToSend, MqttMessage mqttMessage)
        {
            MqttClient mClient;
            string message = XMLHelper.SerializeXml(mqttMessage);
            byte qos = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;

            try
            {
                foreach (var endpoint in endpointsToSend)
                {
                    Console.WriteLine("Sending to endpoint: " + endpoint);  
                    mClient = new MqttClient(IPAddress.Parse(endpoint));
                    mClient.Connect(Guid.NewGuid().ToString());
                    if (!mClient.IsConnected)
                    {
                        throw new Exception("Error connecting to message broker...");
                    }
                    mClient.Publish(mqttMessage.Topic, Encoding.UTF8.GetBytes(message), qos, false);
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
using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace somiod.Helpers
{
    public class MqttHelper
    {
        internal static List<string> FindEndpointsToSend(string application, string container, string notifEvent)
        {
            var containerId = ContainerHelper.FindContainerInDatabase(application, container).Id;
            List<string> endpoints = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT Endpoint FROM Notifications " +
                        "WHERE Parent = @ContainerId AND Enabled = 1 " +
                        "AND (Event = @EventAll OR Event = @Event)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerId", containerId);
                        sqlCommand.Parameters.AddWithValue("@EventAll", "0");
                        sqlCommand.Parameters.AddWithValue("@Event", notifEvent);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                endpoints.Add(reader.GetString(0));
                            }
                            reader.Close();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error finding endpoints to send", e);
            }
            if (endpoints.Count == 0)
            {
                throw new Exception("No endpoints found to send notifications");
            }
            return endpoints;
        }

        internal static void PublishMqttMessages(string topic, string message, List<string> endpointsToSend)
        {
            MqttClient mClient;
            try
            {
                foreach (var endpoint in endpointsToSend)
                {
                    mClient = new MqttClient(IPAddress.Parse(endpoint));
                    mClient.Connect(Guid.NewGuid().ToString());
                    Thread.Sleep(250);
                    if (!mClient.IsConnected)
                    {
                        throw new Exception("Error connecting to message broker...");
                    }
                    mClient.Publish(topic, Encoding.UTF8.GetBytes(message));
                    Thread.Sleep(250);
                    mClient.Disconnect();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error publishing notifications", e);
            }
        }

        internal static void BasicPublish(string topic, string message, string endpoint)
        {
            MqttClient mClient;
            try
            {
                mClient = new MqttClient(IPAddress.Parse(endpoint));
                mClient.Connect(Guid.NewGuid().ToString());
                Thread.Sleep(1000);
                if (!mClient.IsConnected)
                {
                    throw new Exception("Error connecting to message broker...");
                }
                mClient.Publish(topic, Encoding.UTF8.GetBytes(message));
                Thread.Sleep(1000);
                mClient.Disconnect();
            }
            catch (Exception e)
            {
                throw new Exception("Error publishing notifications", e);
            }
        }
    }
}
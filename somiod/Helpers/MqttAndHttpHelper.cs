/**
* @brief Somiod - Projeto de Integração de Sistemas
* @date 2024-12-18
* @authors Diogo Abegão, João Parreira, Marcelo Oliveira, Pedro Barbeiro
*/
using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Xml;

namespace somiod.Helpers
{
    public class MqttAndHttpHelper
    {
        // Method to find endpoints to send notifications
        internal static List<string> FindEndpointsToSend(string application, string container, string eventType)
        {
            var containerId = ContainerHelper.FindContainerInDatabase(application, container).id;
            List<string> endpoints = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT endpoint FROM notifications " +
                        "WHERE parent = @ContainerId AND enabled = 1 " +
                        "AND (event_type = @EventAll OR event_type = @Event)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ContainerId", containerId);
                        sqlCommand.Parameters.AddWithValue("@EventAll", "0");
                        sqlCommand.Parameters.AddWithValue("@Event", eventType);
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

        // Method to publish MQTT messages
        internal static void PublishMqttMessage(string topic, string message, string endpoint)
        {
            try
            {
                MqttClient mClient;
                Uri uri = new Uri(endpoint);
                mClient = new MqttClient(IPAddress.Parse(uri.Host));
                mClient.Connect(Guid.NewGuid().ToString());
                Thread.Sleep(50);
                if (!mClient.IsConnected)
                {
                    throw new Exception("Error connecting to message broker...");
                }
                mClient.Publish(topic, Encoding.UTF8.GetBytes(message));
                Thread.Sleep(50);
                mClient.Disconnect();
            }
            catch (Exception e)
            {
                throw new Exception("Error publishing mqtt message", e);
            }
        }

        // Method to send HTTP POST requests
        internal static void SendHttpPostRequest(string topic, string content,string eventType, string endpoint)
        {
            Message message = new Message
            {
                topic = topic,
                content = content,
                event_type = eventType
            };

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string requestBody = XMLHelper.SerializeXmlUtf8<Message>(message);
                    HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = client.PostAsync(endpoint, httpContent).Result;
                    int statusCode = (int)response.StatusCode;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    throw new Exception("Error sending http post request", e);
                }
            }
        }
    }
}
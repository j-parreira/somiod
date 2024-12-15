using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services.Description;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static System.Net.Mime.MediaTypeNames;

namespace somiod.Helpers
{
    public class RecordHelper
    {
        // Method to check if record name exists
        internal static bool RecordNameExists(string recordName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM records " +
                        "WHERE name = @RecordName", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", recordName.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        int count = (int)sqlCommand.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error checking if record name exists", e);
            }
        }

        // Method to check if record exists under a container in an application
        internal static bool RecordExists(string application, string container, string record)
        {
            var app = ApplicationHelper.FindApplicationInDatabase(application);
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1)" +
                        "FROM records r " +
                        "INNER JOIN containers c ON r.parent = c.id " +
                        "WHERE r.name = @RecordName AND r.parent = @RecordParent " +
                        "AND c.parent = @ContainerParent", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", record.ToLower());
                        sqlCommand.Parameters.AddWithValue("@RecordParent", cont.id);
                        sqlCommand.Parameters.AddWithValue("@ContainerParent", app.id);
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

        // Method to find all records in database
        internal static List<Record> FindRecordsInDatabase()
        {
            var records = new List<Record>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM records", sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                records.Add(new Record
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3),
                                    content = reader.GetString(4)
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

        // Method to find records by application
        internal static List<Record> FindRecordsByApplication(string application)
        {
            var app = ApplicationHelper.FindApplicationInDatabase(application);
            var records = new List<Record>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM records " +
                        "WHERE parent IN (SELECT id FROM containers WHERE parent = @ParentId)", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ParentId", app.id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                records.Add(new Record
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3),
                                    content = reader.GetString(4)
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

        // Method to find records by container
        internal static List<Record> FindRecordsByContainer(string application, string container)
        {
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            var records = new List<Record>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * " +
                        "FROM records WHERE parent = @ParentId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ParentId", cont.id);
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                records.Add(new Record
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3),
                                    content = reader.GetString(4)
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

        // Method to find record in database under a container in an application
        internal static Record FindRecordInDatabase(string application, string container, string record)
        {
            Record rec = null;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * " +
                        "FROM records WHERE name = @RecordName " +
                        "AND parent = @ContainerId", sqlConnection))
                    {
                        int contId = ContainerHelper.FindContainerInDatabase(application, container).id;
                        sqlCommand.Parameters.AddWithValue("@ContainerId", contId);
                        sqlCommand.Parameters.AddWithValue("@RecordName", record.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rec = new Record
                                {
                                    id = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    creation_datetime = reader.GetDateTime(2),
                                    parent = reader.GetInt32(3),
                                    content = reader.GetString(4)
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

        // Method to add record to database under a container in an application and publish MQTT messages or send HTTP post
        internal static Record AddRecordToDatabase(string application, string container, Record record)
        {
            if (RecordNameExists(record.name))
            {
                int i = 1;
                string newName = record.name;
                while (RecordNameExists(newName))
                {
                    newName = record.name + i.ToString();
                    i++;
                }
                record.name = newName;
            }
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO records " +
                        "(name, content, creation_datetime, parent) " +
                        "VALUES (@RecordName, @RecordContent, @RecordCreationDateTime, @RecordParent)", sqlConnection))
                    {
                        record.name = record.name.ToLower();
                        sqlCommand.Parameters.AddWithValue("@RecordName", record.name);
                        sqlCommand.Parameters.AddWithValue("@RecordContent", record.content);
                        sqlCommand.Parameters.AddWithValue("@RecordCreationDateTime", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@RecordParent", cont.id);
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
            string topic = application + "/" + container;
            string content = record.content;
            string event_type = "creation";
            string message = event_type + ";" + content;
            List<string> endpoints = MqttAndHttpHelper.FindEndpointsToSend(application, container, "1");
            foreach (var endpoint in endpoints)
            {
                if (endpoint.StartsWith("mqtt://"))
                {
                    MqttAndHttpHelper.PublishMqttMessage(topic, message, endpoint);
                }
                else if (endpoint.StartsWith("http://"))
                {
                    MqttAndHttpHelper.SendHttpPostRequest(topic, content, event_type, endpoint);
                }
                else
                {
                    throw new Exception("Invalid endpoint...");
                }
            }
            return record;
        }

        // Method to delete record in database under a container in an application and publish MQTT messages or send HTTP post
        internal static void DeleteRecordFromDatabase(string application, string container, string record)
        {
            string content = FindRecordInDatabase(application, container, record).content;
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM records " +
                        "WHERE name = @RecordName AND parent = @ContainerId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", record.ToLower());
                        sqlCommand.Parameters.AddWithValue("@ContainerId", cont.id);
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
            string topic = application + "/" + container;
            string event_type = "deletion";
            string message = event_type + ";" + content;
            List<string> endpoints = MqttAndHttpHelper.FindEndpointsToSend(application, container, "2");
            foreach (var endpoint in endpoints)
            {
                if (endpoint.StartsWith("mqtt://"))
                {
                    MqttAndHttpHelper.PublishMqttMessage(topic, message, endpoint);
                }
                else if (endpoint.StartsWith("http://"))
                {
                    MqttAndHttpHelper.SendHttpPostRequest(topic, content, event_type, endpoint);
                }
                else
                {
                    throw new Exception("Invalid endpoint...");
                }
            }
        }
    }
}
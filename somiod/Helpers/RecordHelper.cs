using somiod.Helpers;
using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services.Description;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static System.Net.Mime.MediaTypeNames;

namespace somiod.Helpers
{
    public class RecordHelper
    {
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

        internal static Record AddRecordToDatabase(string application, string container, Record record)
        {
            record.name = "rec_0";
            if (RecordNameExists(record.name))
            {
                int i = 1;
                while (RecordNameExists(record.name))
                {
                    record.name = "rec_" + i.ToString();
                    i++;
                }
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
            string message = "creation;" + record.content;
            List<string> endpoints = MqttHelper.FindEndpointsToSend(application, container, "1");
            MqttHelper.PublishMqttMessages(topic, message, endpoints);
            return record;
        }

        internal static void DeleteRecordFromDatabase(string application, string container, string record)
        {
            string message = FindRecordInDatabase(application, container, record).content;
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
            message = "deletion;" + message;
            List<string> endpoints = MqttHelper.FindEndpointsToSend(application, container, "1");
            MqttHelper.PublishMqttMessages(topic, message, endpoints);
        }
    }
}
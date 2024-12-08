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
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(1) FROM Records " +
                        "WHERE Name = @RecordName", sqlConnection))
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
                        "FROM Records r " +
                        "INNER JOIN Containers c ON r.Parent = c.Id " +
                        "WHERE r.Name = @RecordName AND r.Parent = @RecordParent " +
                        "AND c.Parent = @ContainerParent", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", record.ToLower());
                        sqlCommand.Parameters.AddWithValue("@RecordParent", cont.Id);
                        sqlCommand.Parameters.AddWithValue("@ContainerParent", app.Id);
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
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Records", sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                records.Add(new Record
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3),
                                    Content = reader.GetString(4)
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
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Records " +
                        "WHERE Parent IN (SELECT Id FROM Containers WHERE Parent = @ParentId)", sqlConnection))
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
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3),
                                    Content = reader.GetString(4)
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
                        "FROM Records WHERE Parent = @ParentId", sqlConnection))
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
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3),
                                    Content = reader.GetString(4)
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
                        "FROM Records WHERE Name = @RecordName " +
                        "AND Parent = @ContainerId", sqlConnection))
                    {
                        int contId = ContainerHelper.FindContainerInDatabase(application, container).Id;
                        sqlCommand.Parameters.AddWithValue("@ContainerId", contId);
                        sqlCommand.Parameters.AddWithValue("@RecordName", record.ToLower());
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rec = new Record
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreationDateTime = reader.GetDateTime(2),
                                    Parent = reader.GetInt32(3),
                                    Content = reader.GetString(4)
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
            record.Name = "rec_0";
            if (RecordNameExists(record.Name))
            {
                int i = 1;
                while (RecordNameExists(record.Name))
                {
                    record.Name = "rec_" + i.ToString();
                    i++;
                }
            }
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO Records " +
                        "(Name, Content, CreationDateTime, Parent) " +
                        "VALUES (@RecordName, @RecordContent, @RecordCreationDateTime, @RecordParent)", sqlConnection))
                    {
                        record.Name = record.Name.ToLower();
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
            string topic = application + "/" + container;
            string message = "deletion;" + record.Content;
            List<string> endpoints = MqttHelper.FindEndpointsToSend(application, container, "1");
            MqttHelper.PublishMqttMessages(topic, message, endpoints);
            return record;
        }

        internal static void DeleteRecordFromDatabase(string application, string container, string record)
        {
            string message = FindRecordInDatabase(application, container, record).Content;
            var cont = ContainerHelper.FindContainerInDatabase(application, container);
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.ConnStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM Records " +
                        "WHERE Name = @RecordName AND Parent = @ContainerId", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@RecordName", record.ToLower());
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
            string topic = application + "/" + container;
            message = "deletion;" + message;
            List<string> endpoints = MqttHelper.FindEndpointsToSend(application, container, "1");
            MqttHelper.PublishMqttMessages(topic, message, endpoints);
        }
    }
}
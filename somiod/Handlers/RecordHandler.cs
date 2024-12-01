using somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace somiod.Handlers
{
    public class RecordHandler
    {
        internal static void AddRecordToDatabase(string application, string container, Record record)
        {
            throw new NotImplementedException();
        }

        internal static void DeleteRecordFromDatabase(string application, string container, string record)
        {
            throw new NotImplementedException();
        }

        internal static Record FindRecordInDatabase(string application, string container, string record)
        {
            throw new NotImplementedException();
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

        internal static bool RecordExists(string application, string container, string record)
        {
            throw new NotImplementedException();
        }
    }
}
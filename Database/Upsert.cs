using MySql.Data.MySqlClient;
using SPTC_APPLICATION.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPTC_APPLICATION.Database
{
    //USAGE: 
    /*
     new Create() for every new object with connection to database
    Insert(fieldname, value) for inserting new values, use the Field static variables
    Save() always to save the progress to database
     */
    public class Upsert
    {
        private readonly string tableName;
        public int id;
        private Dictionary<string, object> fieldValues;

        public Upsert(string tableName, int id)
        {
            this.tableName = tableName;
            this.id = id;

            if (id == -1)
            {
                fieldValues = new Dictionary<string, object>();
                //EventLogger.Post($"DTB :: Attempt to create new Information at ({tableName})");
            }
            else
            {
                fieldValues = LoadDataFromDatabase();
                //EventLogger.Post($"DTB :: Attempt to access ({tableName}) pk: {id}");
            }

        }

        public void Insert(string fieldName, object value)
        {
            fieldValues[fieldName] = value;
        }

        public object Access(string fieldName)
        {
            if (fieldValues.ContainsKey(fieldName))
            {
                return fieldValues[fieldName];
            }

            return null;
        }

        public void Save()
        {
            if (id == -1)
            {
                InsertDataToDatabase();
                EventLogger.Post($"DTB :: Insert into ({tableName})");
            }
            else
            {
                UpdateDataInDatabase();
                EventLogger.Post($"DTB :: Update ({tableName}) pk: {id}");
            }
        }

        private Dictionary<string, object> LoadDataFromDatabase()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                using (MySqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = $"SELECT * FROM {tableName} WHERE id = @id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string fieldName = reader.GetName(i);
                                object fieldValue = reader.GetValue(i);
                                result[fieldName] = fieldValue;
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                EventLogger.Post($"DTB :: Exception : {ex.Message}");
            }

            return result;
        }

        private void InsertDataToDatabase()
        {
            try
            {
                using (MySqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string columns = string.Join(", ", fieldValues.Keys);
                    string values = string.Join(", ", fieldValues.Keys.Select(key => "@" + key));

                    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    foreach (var entry in fieldValues)
                    {
                        command.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    try
                    {
                        command.ExecuteNonQuery();
                        id = (int)command.LastInsertedId;
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Number == 1062)
                        {
                            id = GetExistingRecordId();
                            EventLogger.Post($"DTB :: duplicate entry. id={GetExistingRecordId()}");
                            this.Save();
                        }
                        else
                        {
                            EventLogger.Post($"Exception: {ex.Message}");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                EventLogger.Post($"DTB :: Exception : {ex.Message}");
            }
        }

        private void UpdateDataInDatabase()
        {
            try
            {
                using (MySqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string setValues = string.Join(", ", fieldValues.Keys.Select(key => $"{key} = @{key}"));

                    string query = $"UPDATE {tableName} SET {setValues} WHERE id=@idkey";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idkey", id);

                    foreach (var entry in fieldValues)
                    {
                        command.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                EventLogger.Post($"DTB :: Exception : {ex.Message}");
            }
        }

        private int GetExistingRecordId()
        {
            try
            {
                using (MySqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = $"SELECT id FROM {tableName} WHERE {GetUniqueAttributeName()} = @{GetUniqueAttributeName()}";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue($"@{GetUniqueAttributeName()}", fieldValues[GetUniqueAttributeName()]);

                    object result = command.ExecuteScalar();

                    return (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : -1;
                }
            }
            catch (MySqlException ex)
            {
                EventLogger.Post($"DTB :: Exception : {ex.Message}");
            }
            return -1;
        }

        private string GetUniqueAttributeName()
        {
            return fieldValues.Keys.FirstOrDefault();
        }

    }
}

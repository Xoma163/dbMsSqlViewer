using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace bd_lab1.db
{
    class dbWorker
    {
        private string connectionString;

        private string dbName;
        private string tableName;
        private List<string> fields;

        public dbWorker(string dbName)
        {
            this.dbName = dbName;
            connectionString = "Data Source=(local);Initial Catalog=" + dbName + "; Integrated Security=true";
        }

        //Селект всей таблицы
        public List<List<string>> Select(string tableName)
        {
            this.tableName = tableName;
            string query = "Select * from " + tableName;
            List<List<string>> table = new List<List<string>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader[i]+"");
                    }
                    table.Add(row);
                }
                fields = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                reader.Close();
            }
            return table;
        }
        //Селект со своим запросом
        public List<List<string>> Select(string tableName, string query)
        {
            this.tableName = tableName;
            List<List<string>> table = new List<List<string>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader[i] + "");
                    }
                    table.Add(row);
                }
                fields = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                reader.Close();
            }
            return table;
        }

        //Получаем список всех таблиц в базе или всех баз (пользовательских)
        public List<string> getTables()
        {
            string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '"+ dbName+"'" ;

            List<string> list = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[0]+"");
                }
                reader.Close();
            }
            return list;
        }
        //Получаем список всех БД (пользовательских)
        public List<string> getDatabases()
        {
            string query = "SELECT name FROM sys.databases WHERE name NOT IN ('master','tempdb','model','msdb') AND is_distributor = 0 AND source_database_id IS NULL";

            List<string> list = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[0] + "");
                }
                reader.Close();
            }
            return list;
        }
        //Выполняем любой запрос
        public string doQuery(string query)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    var cmd = new SqlCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            return "ok";
        }
        
        public List<string> getFields()
        {
            return fields;
        }
    }
}

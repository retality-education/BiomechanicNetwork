using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using BiomechanicNetwork.Utilities;

namespace BiomechanicNetwork.Database
{
    internal class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            _connectionString = SecretsManager.GetConnectionString();
        }

        public DataTable ExecuteQuery(string query, NpgsqlParameter[] parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var dataTable = new DataTable();
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    return dataTable;
                }
            }
        }

        public int ExecuteNonQuery(string query, NpgsqlParameter[] parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }
        public object ExecuteScalar(string query, NpgsqlParameter[] parameters = null)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteScalar();
                }
            }
        }
        public T ExecuteScalar<T>(string query, NpgsqlParameter[] parameters = null)
        {
            var result = ExecuteScalar(query, parameters);
            return result == null || result == DBNull.Value
                ? default(T)
                : (T)Convert.ChangeType(result, typeof(T));
        }
    }
}

using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ChatModularMicroservice.Shared.Configs
{
    public class ConnectionFactory : IConnectionFactory, IDisposable
    {
        private readonly string _connectionString;

        public ConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string must not be null or empty", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }

        public void Dispose()
        {
            // No-op: connections are disposed by Dapper per call; factory holds no persistent resources
        }
    }
}
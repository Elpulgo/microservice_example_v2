using System;
using Npgsql;

namespace Shared.Infrastructure
{
    public class PostgreContext
    {
        private readonly string m_ConnectionString;

        public NpgsqlConnection Instance => ConnectToPostgres();

        public PostgreContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or empty", nameof(connectionString));

            m_ConnectionString = connectionString;
        }

        private NpgsqlConnection ConnectToPostgres()
            => new NpgsqlConnection(m_ConnectionString);
    }
}
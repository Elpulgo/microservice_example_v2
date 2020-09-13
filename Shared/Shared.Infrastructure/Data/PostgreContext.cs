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
            m_ConnectionString = connectionString;
        }

        private NpgsqlConnection ConnectToPostgres()
        {
            if (string.IsNullOrEmpty(m_ConnectionString))
                throw new ArgumentNullException(nameof(m_ConnectionString));

            return new NpgsqlConnection(m_ConnectionString);
        }
    }
}
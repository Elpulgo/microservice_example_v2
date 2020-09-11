using System;
using Npgsql;

namespace Shared.Infrastructure
{
    public class PostgreContext
    {
        private readonly string m_ConnectionName;

        public NpgsqlConnection Instance => ConnectToPostgres();

        public string TableName { get; }

        public PostgreContext(string connectionName, string tableName)
        {
            m_ConnectionName = connectionName;
            TableName = tableName;
        }

        private NpgsqlConnection ConnectToPostgres()
        {
            if (string.IsNullOrEmpty(m_ConnectionName))
                throw new ArgumentNullException(nameof(m_ConnectionName));

            return new NpgsqlConnection(m_ConnectionName);
        }
    }
}
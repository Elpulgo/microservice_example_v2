using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using Shared.Core.Models;
using Dommel;

namespace Shared.Infrastructure
{
    public class ReadRepository<T> : IReadRepository<T> where T : Entity
    {
        private readonly PostgreContext m_Context;
        private readonly string m_TableName;

        public ReadRepository(PostgreContext context, string tableName)
        {
            m_TableName = tableName;
            m_Context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            using var connection = m_Context.Instance;
            return (await connection.GetAllAsync<T>()).AsList();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            EnsureNotNullOrEmpty(id);
            using var connection = m_Context.Instance;
            var result = await connection.QueryFirstOrDefaultAsync<T>($@"SELECT * FROM {m_TableName} WHERE id=@Id", new { Id = id });
            return result ?? throw new KeyNotFoundException($"{m_TableName} with id [{id}] could not be found.");
        }

        private void EnsureNotNullOrEmpty(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException(id.GetType().Name);

            if (id == Guid.Empty)
                throw new ArgumentException($"{id.GetType().Name} can't be empty.");
        }
    }
}
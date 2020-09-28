using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;
using Shared.Core.Models;
using Dommel;
using System.Linq.Expressions;

namespace Shared.Infrastructure
{
    public class ReadRepository<T> : IReadRepository<T> where T : Entity
    {
        private readonly PostgreContext m_Context;
        private readonly string m_TableName;

        public ReadRepository(PostgreContext context, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            m_TableName = tableName;
            m_Context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            using var connection = m_Context.Instance;
            return (await connection.GetAllAsync<T>()).AsList();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            EnsureNotNullOrEmpty(id);
            using var connection = m_Context.Instance;
            var result = await connection.ExecuteScalarAsync<int>($@"SELECT 1 FROM {m_TableName} WHERE id=@Id", new { Id = id });
            Console.WriteLine($"Result if exists is: '{result}', id is: '{id}'");
            return result == 1;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            EnsureNotNullOrEmpty(id);
            using var connection = m_Context.Instance;
            var result = await connection.QueryFirstOrDefaultAsync<T>($@"SELECT * FROM {m_TableName} WHERE id=@Id", new { Id = id });
            return result ?? throw new KeyNotFoundException($"{m_TableName} with id [{id}] could not be found.");
        }

        public async Task<IReadOnlyList<T>> SelectAsync(Expression<Func<T, bool>> predicate)
        {
            using var connection = m_Context.Instance;
            var result = await connection.SelectAsync(predicate);
            return result.AsList();
        }

        private void EnsureNotNullOrEmpty(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(id)} can't be empty.");
        }
    }
}
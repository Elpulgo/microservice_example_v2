using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Core;
using Dapper;
using Dapper.Contrib.Extensions;
using System;

namespace Shared.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PostgreContext m_Context;

        public Repository(PostgreContext context)
        {
            m_Context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            using var connection = m_Context.Instance;
            return (await connection.GetAllAsync<T>()).AsList();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            using var connection = m_Context.Instance;
            var result = await connection.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {m_Context.TableName} WHERE Id=@Id", new { Id = id });
            return result ?? throw new KeyNotFoundException($"{m_Context.TableName} with id [{id}] could not be found.");
        }

        public async Task DeleteAsync(T entity)
        {
            using var connection = m_Context.Instance;
            var result = await connection.DeleteAsync<T>(entity);
            if (!result)
            {
                throw new InvalidOperationException($"{m_Context.TableName} failed to delete entity [{entity}].");
            }
        }

        public async Task DeleteByIdAsync(TId id)
        {
            using var connection = m_Context.Instance;
            var result = await connection.ExecuteAsync($"DELETE FROM {m_Context.TableName} WHERE Id=@Id", new { Id = id });
            if (result == 0)
            {
                throw new InvalidOperationException($"{m_Context.TableName} failed to delete entity with id [{id}], don't exist.");
            }
        }

        public async Task<T> InsertAsync(T entity)
        {
            using var connection = m_Context.Instance;
            await connection.InsertAsync<T>(entity);
            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            using var connection = m_Context.Instance;
            return await connection.UpdateAsync<T>(entity);
        }
    }
}
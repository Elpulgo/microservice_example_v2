using System.Threading.Tasks;
using Dapper;
using System;
using Shared.Core.Models;
using Dommel;
using Shared.Infrastructure.Extensions;

namespace Shared.Infrastructure
{
    public class WriteRepository<T> : IWriteRepository<T> where T : Entity
    {
        private readonly PostgreContext m_Context;
        private readonly string m_TableName;

        public WriteRepository(PostgreContext context, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            m_TableName = tableName;
            m_Context = context;
        }

        public async Task DeleteAsync(T entity)
        {
            EnsureNotNull(entity);
            await DeleteByIdAsync(entity.Id);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            EnsureNotNullOrEmpty(id);
            using var connection = m_Context.Instance;
            var result = await connection.ExecuteAsync($@"DELETE FROM {m_TableName} WHERE id=@Id", new { Id = id });
            if (result == 0)
            {
                throw new InvalidOperationException($"{m_TableName} failed to delete entity with id [{id}], don't exist.");
            }
        }

        public async Task<T> InsertAsync(T entity)
        {
            EnsureNotNull(entity);
            using var connection = m_Context.Instance;
            await connection.InsertAsync<T>(entity);
            return entity;
        }

        // Special handling of update, since not working with Dapper.Dommel for some reason.. probably some id issue
        // since I use UUID as Id in my solution, and Dapper.Dommel don't properly track it.
        // Get some issue in PostgreSql where it says the syntax is wrong when using 'connection.UpdateAsync<T>(entity)'
        // and there is not much documentation regarding Dapper.Dommel... 
        public async Task<bool> UpdateAsync(T entity)
        {
            EnsureNotNull(entity);
            using var connection = m_Context.Instance;

            var command = GetUpdateCommand(entity);
            using var transaction = await connection.BeginTransactionAsync();
            var result = await transaction.Connection.ExecuteAsync(command, entity);
            if (result > 1)
            {
                Console.WriteLine("Command updated 2 rows, there is something wrong in the database, this should not happen! Will rollback transaction.");
                await transaction.RollbackAsync();
            }
            else
            {
                await transaction.CommitAsync();
            }
            return result == 1;
        }

        private void EnsureNotNull(T param)
        {
            if (param == null)
                throw new ArgumentNullException(param.GetType().Name);
        }

        private void EnsureNotNullOrEmpty(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException(id.GetType().Name);

            if (id == Guid.Empty)
                throw new ArgumentException($"{id.GetType().Name} can't be empty.");
        }

        private string GetUpdateCommand(T entity)
        {
            var command = $"UPDATE {m_TableName} SET ";

            foreach (var property in entity.GetType().GetProperties())
            {
                if (property.Name == "Id")
                    continue;

                command += $"{property.Name.ToSnakeCase()} = @{property.Name},";
            }

            command = command.Remove(command.Length - 1);
            command += $" WHERE id = @Id";

            return command;
        }
    }
}
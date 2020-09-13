using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Infrastructure
{
    public interface IRepository<T>
    {
        Task DeleteAsync(T entity);
        Task DeleteByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
    }
}
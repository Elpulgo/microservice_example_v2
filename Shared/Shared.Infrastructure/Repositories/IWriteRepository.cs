using System;
using System.Threading.Tasks;

namespace Shared.Infrastructure
{
    public interface IWriteRepository<T>
    {
        Task DeleteAsync(T entity);
        Task DeleteByIdAsync(Guid id);
        Task<T> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
    }
}
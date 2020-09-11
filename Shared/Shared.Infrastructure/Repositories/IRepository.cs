using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Core;

namespace Shared.Infrastructure
{
    public interface IRepository<T>
    {
        Task DeleteAsync(T entity);
        Task DeleteByIdAsync(TId id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(TId id);
        Task<T> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
    }
}
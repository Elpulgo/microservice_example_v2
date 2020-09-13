using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Infrastructure
{
    public interface IReadRepository<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
    }
}
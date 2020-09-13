using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Infrastructure
{
    public interface IReadRepository<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);

        Task<IReadOnlyList<T>> SelectAsync(Expression<Func<T, bool>> predicate);
    }
}
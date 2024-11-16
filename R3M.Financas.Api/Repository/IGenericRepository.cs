

using System.Linq.Expressions;

namespace R3M.Financas.Api.Repository;

public interface IGenericRepository<T>
{
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
    IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>> predicate);
    IAsyncEnumerable<T> ListAsync();
    Task UpdateAsync(T entity);
}
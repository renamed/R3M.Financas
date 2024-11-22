

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace R3M.Financas.Api.Repository;

public interface IGenericRepository<TEntity, TContext> 
    where TEntity : class 
    where TContext : DbContext
{
    Task AddAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> ListAsync();
    Task UpdateAsync(TEntity entity);
}
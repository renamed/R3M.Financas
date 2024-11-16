using Microsoft.EntityFrameworkCore;
using R3M.Financas.Api.Domain;
using System;
using System.Linq.Expressions;

namespace R3M.Financas.Api.Repository;

public class GenericRepository<T> : IGenericRepository<T>
    where T : Registry
{
    private readonly DbContext _context;

    public GenericRepository(DbContext context)
    {
        _context = context;
    }

    public Task AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return _context.SaveChangesAsync();
    }

    public Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return _context.SaveChangesAsync();
    }

    public IAsyncEnumerable<T> ListAsync()
    {
        return _context.Set<T>().AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>> predicate)
    {        
        return _context.Set<T>()
            .Where(predicate)
            .AsAsyncEnumerable();
    }

    public ValueTask<T?> GetAsync(int id)
    {
        return _context.Set<T>().FindAsync(id);
    }
}

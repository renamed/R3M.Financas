﻿using Microsoft.EntityFrameworkCore;
using R3M.Financas.Api.Domain;
using System.Linq.Expressions;

namespace R3M.Financas.Api.Repository;

public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity, TContext>
    where TEntity : Register
    where TContext : DbContext
{
    protected readonly TContext Context;

    public GenericRepository(TContext context)
    {
        Context = context;
    }

    public virtual Task<int> CountAsync()
    {
        return Context.Set<TEntity>().CountAsync();
    }

    public virtual Task AddAsync(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
        return Context.SaveChangesAsync();
    }

    public virtual Task UpdateAsync(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
        return Context.SaveChangesAsync();
    }

    public virtual Task DeleteAsync(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
        return Context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> ListAsync()
    {
        return await Context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {        
        return await Context.Set<TEntity>()
            .Where(predicate)
            .ToListAsync();
    }

    public virtual ValueTask<TEntity?> GetAsync(Guid id)
    {
        return Context.Set<TEntity>().FindAsync(id);
    }
}

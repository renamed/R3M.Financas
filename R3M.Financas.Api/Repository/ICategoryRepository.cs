﻿
using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository;

public interface ICategoryRepository
{
    Task AddAsync(Category entity);
    Task DeleteAsync(Category entity);
    ValueTask<Category?> GetAsync(int id);
    IAsyncEnumerable<Category> ListAsync();
    Task UpdateAsync(Category entity);
}
﻿using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository;

public interface IMovimentationRepository
{
    Task AddAsync(Movimentation entity);
    Task DeleteAsync(Movimentation entity);
    ValueTask<Movimentation?> GetAsync(Guid id);
    Task<int> GetCategoryCountAsync(Guid id);
    Task<IEnumerable<Movimentation>> ListAsync(Guid periodId);
    Task UpdateAsync(Movimentation entity);
}
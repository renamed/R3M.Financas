﻿using Microsoft.EntityFrameworkCore;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;

namespace R3M.Financas.Api.Repository;

public class MovimentationRepository : GenericRepository<Movimentation, FinancasContext>, IMovimentationRepository
{
    public MovimentationRepository(FinancasContext context) : base(context)
    {
    }

    public Task<int> GetCategoryCountAsync(Guid id)
    {
        return Context.Movimentations.CountAsync(x => x.CategoryId == id);
    }

    public async Task<IEnumerable<Movimentation>> ListAsync(Guid periodId)
    {
        return await Context.Movimentations.Where(x => x.PeriodId == periodId).ToListAsync();
    }
}

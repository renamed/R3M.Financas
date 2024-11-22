using Microsoft.EntityFrameworkCore;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;
using System.Xml.Linq;

namespace R3M.Financas.Api.Repository;

public class PeriodRepository : GenericRepository<Period, FinancasContext>, IPeriodRepository
{
    public PeriodRepository(FinancasContext context) : base(context)
    {
    }

    public IAsyncEnumerable<Period> ListAsync(int page, int count)
    {
        int skipCount = (page - 1) * count;
        return Context
            .Periods
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip(skipCount)
            .Take(count)
            .AsAsyncEnumerable();
    }

    public IAsyncEnumerable<Period> ListAsync(DateOnly startDate, DateOnly endDate, int page, int count)
    {
        int skipCount = (page - 1) * count;
        return Context
            .Periods
            .AsNoTracking()
            .Where(x => x.Start >= startDate && x.End <= endDate)
            .OrderBy(p => p.Id)
            .Skip(skipCount)
            .Take(count)
            .AsAsyncEnumerable();
    }

    public Task<Period?> GetAsync(DateOnly startDate, DateOnly endDate)
    {
        return Context
            .Periods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Start >= startDate && x.End <= endDate);
    }

    public Task<Period?> GetAsync(string description)
    {
        return Context
            .Periods
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Description == description);
    }
}

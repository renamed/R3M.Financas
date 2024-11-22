using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository;

public interface IPeriodRepository
{
    Task AddAsync(Period entity);
    Task DeleteAsync(Period entity);
    ValueTask<Period?> GetAsync(Guid id);
    IAsyncEnumerable<Period> ListAsync(int page, int count);
    IAsyncEnumerable<Period> ListAsync(DateOnly startDate, DateOnly endDate, int page, int count);
    Task UpdateAsync(Period entity);
    Task<int> CountAsync();
    Task<Period?> GetAsync(string description);
    Task<Period?> GetAsync(DateOnly startDate, DateOnly endDate);
}
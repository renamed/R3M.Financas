using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository;

public interface IPeriodRepository
{
    Task AddAsync(Period entity);
    Task DeleteAsync(Period entity);
    ValueTask<Period?> GetAsync(int id);
    IAsyncEnumerable<Period> ListAsync();
    Task UpdateAsync(Period entity);
}
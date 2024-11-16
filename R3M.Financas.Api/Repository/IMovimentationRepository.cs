using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository;

public interface IMovimentationRepository
{
    Task AddAsync(Movimentation entity);
    Task DeleteAsync(Movimentation entity);
    ValueTask<Movimentation?> GetAsync(int id);
    IAsyncEnumerable<Movimentation> ListAsync();
    Task UpdateAsync(Movimentation entity);
}
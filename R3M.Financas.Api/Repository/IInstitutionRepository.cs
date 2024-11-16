using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository;

public interface IInstitutionRepository
{
    Task AddAsync(Institution entity);
    Task DeleteAsync(Institution entity);
    ValueTask<Institution?> GetAsync(int id);
    IAsyncEnumerable<Institution> ListAsync();
    Task UpdateAsync(Institution entity);
}
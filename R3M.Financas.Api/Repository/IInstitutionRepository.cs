using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository;

public interface IInstitutionRepository
{
    Task AddAsync(Institution entity);
    Task DeleteAsync(Institution entity);
    Task<bool> ExistsAsync(string name);
    ValueTask<Institution?> GetAsync(Guid id);
    IAsyncEnumerable<Institution> ListAsync();
    Task UpdateAsync(Institution entity);
}
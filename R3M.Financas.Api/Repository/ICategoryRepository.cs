using R3M.Financas.Api.Domain;

namespace R3M.Financas.Api.Repository;
 
public interface ICategoryRepository
{
    Task AddAsync(Category entity);
    Task DeleteAsync(Category entity);
    ValueTask<Category?> GetAsync(Guid id);
    ValueTask<Category?> GetAsync(string name);
    Task<int> GetChildrenCountAsync(Guid id);
    Task<IEnumerable<Category>> ListAsync(Guid? parentId);
    Task UpdateAsync(Category entity);
}
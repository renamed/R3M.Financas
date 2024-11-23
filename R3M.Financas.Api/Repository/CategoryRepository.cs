using Microsoft.EntityFrameworkCore;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;
using System.Xml.Linq;

namespace R3M.Financas.Api.Repository;

public class CategoryRepository : GenericRepository<Category, FinancasContext>, ICategoryRepository
{
    public CategoryRepository(FinancasContext context) : base(context)
    {
    }

    public async ValueTask<Category?> GetAsync(string name)
    {
        return await Context.Categories.FirstOrDefaultAsync(x => EF.Functions.ILike(x.Name, name));
    }

    public Task<int> GetChildrenCountAsync(Guid id)
    {
        return Context.Categories.CountAsync(x => x.ParentId == id);
    }

    public async Task<IEnumerable<Category>> ListAsync(Guid? parentId)
    {
        return await Context.Categories.Where(x => x.ParentId == parentId).ToListAsync();
    }
}

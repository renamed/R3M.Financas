using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;

namespace R3M.Financas.Api.Repository;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FinancasContext context) : base(context)
    {
    }
}

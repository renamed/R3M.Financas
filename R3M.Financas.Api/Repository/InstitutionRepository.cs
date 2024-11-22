using Microsoft.EntityFrameworkCore;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;

namespace R3M.Financas.Api.Repository;

public class InstitutionRepository : GenericRepository<Institution, FinancasContext>, IInstitutionRepository
{
    private readonly FinancasContext context;

    public InstitutionRepository(FinancasContext context) : base(context)
    {
        this.context = context;
    }

    public Task<bool> ExistsAsync(string name)
    {
        return context.Institutions.AnyAsync(x => EF.Functions.Like(x.Name, $"%{name}%"));
    }
}

using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;

namespace R3M.Financas.Api.Repository;

public class InstitutionRepository : GenericRepository<Institution>, IInstitutionRepository
{
    public InstitutionRepository(FinancasContext context) : base(context)
    {
    }
}

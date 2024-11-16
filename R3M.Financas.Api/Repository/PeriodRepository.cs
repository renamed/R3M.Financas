using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;

namespace R3M.Financas.Api.Repository;

public class PeriodRepository : GenericRepository<Period>, IPeriodRepository
{
    public PeriodRepository(FinancasContext context) : base(context)
    {
    }
}

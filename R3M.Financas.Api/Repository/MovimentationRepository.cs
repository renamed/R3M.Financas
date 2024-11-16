using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;

namespace R3M.Financas.Api.Repository;

public class MovimentationRepository : GenericRepository<Movimentation>, IMovimentationRepository
{
    public MovimentationRepository(FinancasContext context) : base(context)
    {
    }
}

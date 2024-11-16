using Microsoft.EntityFrameworkCore;
using R3M.Financas.Api.Repository.Context;

namespace R3M.Financas.Api.Repository;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                    .AddScoped<ICategoryRepository, CategoryRepository>()
                    .AddScoped<IInstitutionRepository, InstitutionRepository>()
                    .AddScoped<IMovimentationRepository, MovimentationRepository>()
                    .AddScoped<IPeriodRepository, PeriodRepository>()

                    .AddDbContext<FinancasContext>(opt =>
                        opt.UseNpgsql(configuration.GetConnectionString("financas"))
                            .UseSnakeCaseNamingConvention());

}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository.Context;

namespace R3M.Financas.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {   
            // Criação do banco de dados para testes
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<FinancasContext>();
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            
            dbContext.Periods.ExecuteDelete();
            dbContext.Periods.AddRange([            
                new Period
                {
                    Id = Guid.Parse("BFF69F57-4CDC-4C04-B72D-63F38EE502C2"),
                    Description = "202001",
                    Start = new DateOnly(2020, 01, 01),
                    End = new DateOnly(2020, 01, 31)
                },
                new Period
                {
                    Id = Guid.Parse("BFF69F57-4CDC-4C04-B72D-63F38EE502C3"),
                    Description = "202002",
                    Start = new DateOnly(2020, 02, 01),
                    End = new DateOnly(2020, 02, 29)
                }
            ]);
        });
    }
}
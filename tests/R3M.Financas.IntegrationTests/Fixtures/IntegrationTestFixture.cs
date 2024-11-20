
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository;
using R3M.Financas.Api.Repository.Context;
using System.Text.Json;
using Testcontainers.PostgreSql;

namespace R3M.Financas.IntegrationTests.Fixtures;

public class IntegrationTestFixture : IAsyncLifetime
{
    private INetwork network;
    private IContainer postgreSql;
    private IContainer api;
    private string connectionString;
    private string connectionStringContainer;

    public HttpClient Client { get; private set; }

    public async Task DisposeAsync()
    {        
        await DisposeAsync(api);
        await DisposeAsync(postgreSql);
        await DisposeAsync(network);

        Client?.Dispose();
    }

    private async Task DisposeAsync<T>(T obj)
        where T : IAsyncDisposable
    {
        if (obj != null)
        {
            await obj.DisposeAsync();
        }
    }

    public async Task InitializeAsync()
    {
        await CreateNetworkAsync();
        await CreateDbAsync();
        await InitDbAsync();
        await CreateApiAsync();

        Client = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:8080")
        };        
    }

    private async Task  InitDbAsync()
    {
        using FinancasContext context = GetFinancasContext();
        await context.Database.MigrateAsync();

        context.Institutions.AddRange(JsonSerializer.Deserialize<Institution[]>(await File.ReadAllTextAsync(Path.Combine("Data", "Institution.json"))));
        await context.SaveChangesAsync();
    }

    private async Task CreateNetworkAsync()
    {
        network = new NetworkBuilder()
            .WithName($"test-network-{Guid.NewGuid()}")
            .Build();
        
        await network.CreateAsync();
    }

    private async Task CreateDbAsync()
    {
        postgreSql = new PostgreSqlBuilder()
            .WithName("test-postgres")
            .WithHostname("test-postgres")
            .WithNetwork(network)
            .WithDatabase("financas")
            .WithPassword("password123")
            .WithUsername("financas")
            .WithPortBinding(5432, 5432)
            .WithExposedPort(5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("database system is ready to accept connections"))
            .Build();
        
        await postgreSql.StartAsync();

        connectionString = ((PostgreSqlContainer)postgreSql)
            .GetConnectionString();

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Host = "test-postgres"
        };

        connectionStringContainer = connectionStringBuilder.ToString();
    }

    private async Task CreateApiAsync()
    {
        var image = new ImageFromDockerfileBuilder()
            .WithName("r3m-financas")
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("R3M.Financas.Api/Dockerfile")
            .Build();

        await image.CreateAsync();

        api = new ContainerBuilder()
            .WithImage(image)
            .WithName("financas")
            .WithNetwork(network)
            .WithPortBinding(8080, 8080) // Mapeando a porta 8080 do container para a 8080 da máquina
            .WithPortBinding(8081, 8081) // Mapeando a porta 8081 do container para a 8081 da máquina
            .WithExposedPort(8080) // Expondo a porta 8080 para comunicação
            .WithExposedPort(8081) // Expondo a porta 8081
            .WithEnvironment("ConnectionStrings__financas", connectionStringContainer)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080))
            .Build();

        await api.StartAsync();
    }

    private FinancasContext GetFinancasContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<FinancasContext>();
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        var context = new FinancasContext(optionsBuilder.Options);
        return context;
    }

    public IInstitutionRepository GetInstitutionRepository()
    {
        return new InstitutionRepository(GetFinancasContext());
    }
}

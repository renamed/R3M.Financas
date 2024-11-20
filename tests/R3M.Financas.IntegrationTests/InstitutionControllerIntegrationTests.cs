using FluentAssertions;
using R3M.Financas.IntegrationTests.Fixtures;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json.Nodes;

namespace R3M.Financas.IntegrationTests;

public class InstitutionControllerIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture fixture;

    private const string CreateEndpoint = "api/institution";
    private const string ListEndpoint = "api/institution";
    private const string GetEndpoint = "api/institution/{0}";
    private const string DeleteEndpoint = "api/institution/{0}";
    private const string EditEndpoint = "api/institution/{0}";


    public InstitutionControllerIntegrationTests(IntegrationTestFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task ListAsync()
    {
        var response = await fixture.Client.GetAsync(ListEndpoint);
        var responseBody = await JsonObject.ParseAsync(response.Content.ReadAsStream());

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseBody["result"].AsArray().Should()
            .HaveCountGreaterThanOrEqualTo(4)
            .And
            .HaveCountLessThanOrEqualTo(5);
    }

    [Fact]
    public async Task CreateAsync()
    {
        const string InstitutionName = "A good name";
        JsonObject json = new()
        {
            ["Name"] = InstitutionName
        };

        var response = await fixture.Client.PostAsync(CreateEndpoint, new StringContent(json.ToString(), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json));
        var responseBody = await JsonObject.ParseAsync(response.Content.ReadAsStream());

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        responseBody["result"].AsObject().Should().NotBeNull();

        var id = responseBody["result"]["id"].AsValue().GetValue<Guid>();
        id.Should().NotBeEmpty();

        var repo = fixture.GetInstitutionRepository();
        var institution = await repo.GetAsync(id);
        institution.Should().NotBeNull();
        institution.Name.Should().Be(InstitutionName);
    }

    [Fact]
    public async Task GetAsync()
    {
        var id = "024FB6BE-73C6-4B77-81C0-4CA4E84BB594";
        var url = string.Format(GetEndpoint, id);

        var response = await fixture.Client.GetAsync(url);
        var responseBody = await JsonObject.ParseAsync(response.Content.ReadAsStream());

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseBody["result"].AsObject().Should().NotBeNull();
        responseBody["result"]["id"].AsValue().GetValue<Guid>().Should().Be(id);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var id = Guid.Parse("024FB6BE-73C6-4B77-81C0-4CA4E84BB595");
        var url = string.Format(DeleteEndpoint, id);

        var repo = fixture.GetInstitutionRepository();
        var institution = await repo.GetAsync(id);

        var response = await fixture.Client.DeleteAsync(url);
        var responseBody = await JsonObject.ParseAsync(response.Content.ReadAsStream());

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        institution.Should().NotBeNull();
        responseBody["result"].Should().BeNull();        
    }

    [Fact]
    public async Task EditAsync()
    {
        JsonObject json = new()
        {
            ["name"] = "Inst 666"
        };

        var id = "024FB6BE-73C6-4B77-81C0-4CA4E84BB597";
        var url = string.Format(EditEndpoint, id);

        var response = await fixture.Client.PutAsync(url, new StringContent(json.ToString(), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json));
        var responseBody = await JsonObject.ParseAsync(response.Content.ReadAsStream());

        responseBody["result"].AsObject().Should().NotBeNull();
        responseBody["result"]["id"].AsValue().GetValue<Guid>().Should().Be(id);
        responseBody["result"]["name"].AsValue().GetValue<string>().Should().Be("Inst 666");
    }
}

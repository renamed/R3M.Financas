using FluentAssertions;
using R3M.Financas.IntegrationTests.Fixtures;
using System.Net.Mime;
using System.Text.Json.Nodes;

namespace R3M.Financas.IntegrationTests;

[Collection("IntegrationTests")]
public class PeriodControllerIntegrationTests
{
    private readonly IntegrationTestFixture fixture;

    private const string ListEndpoint = "api/period";
    private const string ListByDateRangeEndpoint = "api/period/{0}/{1}";
    private const string CreateEndpoint = "api/period";

    public PeriodControllerIntegrationTests(IntegrationTestFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData(1, 12, 12)]
    [InlineData(2, 12, 1)]
    public async Task ListAsync_ShouldReturnPeriods_WhenPageAndCountAreValid(int page, int count, int resultCount)
    {
        // Arrange
        var url = $"{ListEndpoint}?page={page}&count={count}";

        // Act
        var response = await fixture.Client.GetAsync(url);
        var responseBody = await JsonObject.ParseAsync(response.Content.ReadAsStream());

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseBody["result"].AsArray().Should()
            .HaveCount(resultCount);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnPeriods_WhenDateRangeIsValid()
    {
        // Arrange
        var startDate = "2024-01-01";
        var endDate = "2024-12-31";
        var url = string.Format(ListByDateRangeEndpoint, startDate, endDate);

        // Act
        var response = await fixture.Client.GetAsync(url);
        var responseBody = await JsonObject.ParseAsync(response.Content.ReadAsStream());

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseBody["result"].AsArray().Should()
            .HaveCount(12);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePeriod_WhenValidDataIsProvided()
    {
        // Arrange
        var periodRequest = new JsonObject
        {
            ["Description"] = "202502",
            ["InitialDate"] = "2025-02-01",
            ["FinalDate"] = "2025-02-28"
        };

        // Act
        var response = await fixture.Client.PostAsync(CreateEndpoint,
            new StringContent(periodRequest.ToString(), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json));

        var responseBody = await JsonObject.ParseAsync(response.Content.ReadAsStream());

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        responseBody["result"].AsObject().Should().NotBeNull();

        var id = responseBody["result"]["id"].AsValue().GetValue<Guid>();
        id.Should().NotBeEmpty();

        // Verificar se o período foi de fato criado (opcional)
        var repo = fixture.GetPeriodRepository();
        var period = await repo.GetAsync(id);
        period.Should().NotBeNull();
        period.Description.Should().Be("202502");
        await repo.DeleteAsync(period);
    }
}

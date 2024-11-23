using FluentAssertions;
using R3M.Financas.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json;

namespace R3M.Financas.IntegrationTests;

[Category("IntegrationTest")]
public class PeriodControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public PeriodControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_Periods_Returns_Ok()
    {
        // Arrange
        var response = await _client.GetAsync("api/period");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_Periods_ByDateRange_Returns_Ok()
    {
        // Arrange
        var startDate = "2023-01-01";
        var endDate = "2023-12-31";
        var response = await _client.GetAsync($"api/period/{startDate}/{endDate}?page=1&count=10");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_Period_Returns_Created()
    {
        // Arrange
        var periodRequest = new PeriodRequest
        {
            Description = "New Period",
            InitialDate = new DateOnly(2023, 01,01),
            FinalDate = new DateOnly(2023, 12, 31)
        };

        var content = new StringContent(JsonSerializer.Serialize(periodRequest), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/period", content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var responseContent = await response.Content.ReadAsStringAsync();
        var serverResponse = JsonSerializer.Deserialize<ServerResponse<PeriodResponse>>(responseContent);

        serverResponse.Should().NotBeNull();
        serverResponse.Result.Should().NotBeNull();
        serverResponse.Result.Description.Should().Be(periodRequest.Description);
    }
}

using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using R3M.Financas.Api.Controllers;
using R3M.Financas.Api.Domain;
using R3M.Financas.Api.Repository;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.UnitTests.Controllers;

[Trait("Category", "UnitTest")]
public class PeriodControllerTests
{
    private readonly IPeriodRepository _periodRepository;
    private readonly IMovimentationRepository _movimentationRepository;
    private readonly IValidator<PeriodRequest> _validator;
    private readonly PeriodController _controller;

    public PeriodControllerTests()
    {
        _periodRepository = A.Fake<IPeriodRepository>();
        _movimentationRepository = A.Fake<IMovimentationRepository>();
        _validator = A.Fake<IValidator<PeriodRequest>>();
        _controller = new PeriodController(_periodRepository, _movimentationRepository, _validator);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnBadRequest_WhenPageOrCountAreZeroOrNegative()
    {
        // Arrange
        int page = -1;
        int count = 12;

        // Act
        var result = await _controller.ListAsync(page, count);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Be("Page and count must be positive greater than zero");
    }

    [Fact]
    public async Task ListAsync_ShouldReturnBadRequest_WhenStartDateIsAfterEndDate()
    {
        // Arrange
        var startDate = new DateOnly(2024, 11, 22);
        var endDate = new DateOnly(2024, 11, 21);

        // Act
        var result = await _controller.ListAsync(startDate, endDate);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Be("Start date may be after end date");
    }

    [Fact]
    public async Task ListAsync_ShouldReturnOk_WhenDatesAreValid()
    {
        // Arrange
        var startDate = new DateOnly(2024, 11, 20);
        var endDate = new DateOnly(2024, 11, 22);
        var periods = new List<Period>
        {
            new Period { Id = Guid.NewGuid(), Description = "Period 1", Start = new DateOnly(2024, 11, 20), End = new DateOnly(2024, 11, 21) },
            new Period { Id = Guid.NewGuid(), Description = "Period 2", Start = new DateOnly(2024, 11, 21), End = new DateOnly(2024, 11, 22) }
        };

        A.CallTo(() => _periodRepository.ListAsync(startDate, endDate, 1, 12)).Returns(periods);
        A.CallTo(() => _periodRepository.CountAsync()).Returns(2);

        // Act
        var result = await _controller.ListAsync(startDate, endDate);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<PagingServerResponse<IEnumerable<PeriodResponse>>>().Subject;
        response.Result.Should().HaveCount(2);
        response.Result.First().Description.Should().Be("Period 1");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var request = new PeriodRequest
        {
            InitialDate = new DateOnly(2024, 11, 22),
            FinalDate = new DateOnly(2024, 11, 21),
            Description = "Test Period"
        };

        var validationResult = new ValidationResult(new[] { new ValidationFailure("InitialDate", "Start date cannot be after end date") });
        A.CallTo(() => _validator.Validate(request)).Returns(validationResult);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Contain("Start date cannot be after end date");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenPeriodWithSameDatesExists()
    {
        // Arrange
        var request = new PeriodRequest
        {
            InitialDate = new DateOnly(2024, 11, 22),
            FinalDate = new DateOnly(2024, 11, 23),
            Description = "Test Period"
        };

        A.CallTo(() => _periodRepository.GetAsync(request.InitialDate, request.FinalDate)).Returns(new Period());
        A.CallTo(() => _validator.Validate(request)).Returns(new ValidationResult());

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Contain("There is already a period between");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenPeriodWithSameDescriptionExists()
    {
        // Arrange
        var request = new PeriodRequest
        {
            InitialDate = new DateOnly(2024, 11, 22),
            FinalDate = new DateOnly(2024, 11, 23),
            Description = "Existing Period"
        };

        A.CallTo(() => _periodRepository.GetAsync(request.InitialDate, request.FinalDate)).Returns((Period?)null);
        A.CallTo(() => _periodRepository.GetAsync(request.Description)).Returns(new Period());
        A.CallTo(() => _validator.Validate(request)).Returns(new ValidationResult());

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Contain("There is already a period with such description");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenPeriodIsValid()
    {
        // Arrange
        var request = new PeriodRequest
        {
            InitialDate = new DateOnly(2024, 11, 22),
            FinalDate = new DateOnly(2024, 11, 23),
            Description = "New Period"
        };

        var newPeriod = new Period
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Start = request.InitialDate,
            End = request.FinalDate
        };

        A.CallTo(() => _periodRepository.GetAsync(request.InitialDate, request.FinalDate)).Returns((Period?)null);
        A.CallTo(() => _periodRepository.GetAsync(request.Description)).Returns((Period?)null);
        A.CallTo(() => _periodRepository.AddAsync(A<Period>.Ignored)).Returns(Task.CompletedTask);
        A.CallTo(() => _validator.Validate(request)).Returns(new ValidationResult());

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<ServerResponse<PeriodResponse>>().Subject;
        response.Result.Description.Should().Be(request.Description);
        response.Result.InitialDate.Should().Be(request.InitialDate);
        response.Result.FinalDate.Should().Be(request.FinalDate);
    }
}

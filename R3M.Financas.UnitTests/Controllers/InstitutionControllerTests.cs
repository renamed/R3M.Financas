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

public class InstitutionControllerTests
{
    private readonly IInstitutionRepository _institutionRepository;
    private readonly IMovimentationRepository _movimentationRepository;
    private readonly IValidator<InstitutionRequest> _validator;
    private readonly IValidator<InstitutionUpdateRequest> _updateValidator;
    private readonly InstitutionController _controller;

    public InstitutionControllerTests()
    {
        _institutionRepository = A.Fake<IInstitutionRepository>();
        _movimentationRepository = A.Fake<IMovimentationRepository>();
        _validator = A.Fake<IValidator<InstitutionRequest>>();
        _updateValidator = A.Fake<IValidator<InstitutionUpdateRequest>>();
        _controller = new InstitutionController(_institutionRepository, _validator, _updateValidator, _movimentationRepository);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnOk_WithInstitutions()
    {
        // Arrange
        var institutions = new List<Institution>
        {
            new Institution { Id = Guid.NewGuid(), Name = "Institution 1", Balance = 100 },
            new Institution { Id = Guid.NewGuid(), Name = "Institution 2", Balance = 200 }
        };

        A.CallTo(() => _institutionRepository.ListAsync()).Returns(institutions);


        // Act
        var result = await _controller.ListAsync();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ServerResponse<IEnumerable<InstitutionResponse>>>().Subject;
        response.Result.Should().HaveCount(2);
        response.Result.First().Name.Should().Be("Institution 1");
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenInstitutionNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        A.CallTo(() => _institutionRepository.GetAsync(id)).Returns((Institution?)null);

        // Act
        var result = await _controller.GetAsync(id);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.ErrorMessage.Should().Be($"{id} not found");
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOk_WhenInstitutionExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var institution = new Institution { Id = id, Name = "Test Institution", Balance = 100 };
        A.CallTo(() => _institutionRepository.GetAsync(id)).Returns(institution);

        // Act
        var result = await _controller.GetAsync(id);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.Result.Name.Should().Be("Test Institution");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenInvalidRequest()
    {
        // Arrange
        var request = new InstitutionRequest { Name = "" };
        var validationResult = new ValidationResult([new ValidationFailure("Name", "Name cannot be empty") ]);
        A.CallTo(() => _validator.Validate(request)).Returns(validationResult);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.ErrorMessage.Should().Contain("Name cannot be empty");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenNameAlreadyExists()
    {
        // Arrange
        var request = new InstitutionRequest { Name = "Existing Name" };
        A.CallTo(() => _institutionRepository.ExistsAsync(request.Name)).Returns(true);
        var validationResult = new ValidationResult([new ValidationFailure("Name", "Name already taken")]);
        A.CallTo(() => _validator.Validate(request)).Returns(validationResult);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.ErrorMessage.Should().Be("Name already taken");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenInstitutionDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        A.CallTo(() => _institutionRepository.GetAsync(id)).Returns((Institution?)null);

        // Act
        var result = await _controller.DeleteAsync(id);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.ErrorMessage.Should().Be($"{id} not found");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnBadRequest_WhenMovementsExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var institution = new Institution { Id = id };
        A.CallTo(() => _institutionRepository.GetAsync(id)).Returns(institution);
        A.CallTo(() => _movimentationRepository.ListAsync(id)).Returns([new Movimentation()]);

        // Act
        var result = await _controller.DeleteAsync(id);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.ErrorMessage.Should().Be("Period has movimentations attached to it");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenInstitutionNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new InstitutionUpdateRequest { Name = "New Name" };
        A.CallTo(() => _institutionRepository.GetAsync(id)).Returns((Institution?)null);

        // Act
        var result = await _controller.UpdateAsync(id, request);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.ErrorMessage.Should().Be($"{id} not found");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnBadRequest_WhenNameAlreadyExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new InstitutionUpdateRequest { Name = "Existing Name" };
        var institution = new Institution { Id = id };
        A.CallTo(() => _institutionRepository.GetAsync(id)).Returns(institution);
        A.CallTo(() => _institutionRepository.ExistsAsync(request.Name)).Returns(true);
        var validationResult = new ValidationResult([new ValidationFailure("Name", "Name already taken")]);
        A.CallTo(() => _updateValidator.Validate(request)).Returns(validationResult);

        // Act
        var result = await _controller.UpdateAsync(id, request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.ErrorMessage.Should().Be("Name already taken");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnOk_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new InstitutionUpdateRequest { Name = "Updated Name" };
        var institution = new Institution { Id = id, Name = "Old Name", Balance = 100 };
        A.CallTo(() => _institutionRepository.GetAsync(id)).Returns(institution);
        A.CallTo(() => _institutionRepository.ExistsAsync(request.Name)).Returns(false);
        A.CallTo(() => _institutionRepository.UpdateAsync(institution)).Returns(Task.CompletedTask);
        A.CallTo(() => _updateValidator.Validate(request)).Returns(new ValidationResult());

        // Act
        var result = await _controller.UpdateAsync(id, request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ServerResponse<InstitutionResponse>>().Subject;
        response.Result.Name.Should().Be("Updated Name");
    }
}

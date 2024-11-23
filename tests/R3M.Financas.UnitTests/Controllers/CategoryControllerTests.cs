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
public class CategoryControllerTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMovimentationRepository _movimentationRepository;
    private readonly IValidator<CategoryRequest> _validator;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _categoryRepository = A.Fake<ICategoryRepository>();
        _movimentationRepository = A.Fake<IMovimentationRepository>();
        _validator = A.Fake<IValidator<CategoryRequest>>();
        _controller = new CategoryController(_categoryRepository, _validator, _movimentationRepository);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenInvalidRequest()
    {
        var request = new CategoryRequest { Name = "" };
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Name", "Name cannot be empty") });
        A.CallTo(() => _validator.Validate(request)).Returns(validationResult);

        var result = await _controller.CreateAsync(request);

        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Contain("Name cannot be empty");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenCategoryNameExists()
    {
        var request = new CategoryRequest { Name = "Existing Name" };
        var validationResult = new ValidationResult();
        A.CallTo(() => _validator.Validate(request)).Returns(validationResult);
        A.CallTo(() => _categoryRepository.GetAsync(request.Name)).Returns(new Category { Name = "Existing Name" });

        var result = await _controller.CreateAsync(request);

        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Be("Category with the same name already exists");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenValidRequest()
    {
        var request = new CategoryRequest { Name = "New Category" };
        var validationResult = new ValidationResult();
        A.CallTo(() => _validator.Validate(request)).Returns(validationResult);
        A.CallTo(() => _categoryRepository.GetAsync(request.Name)).Returns((Category?)null);

        var result = await _controller.CreateAsync(request);

        var createdResult = result.Should().BeOfType<CreatedResult>().Subject;
        var response = createdResult.Value.Should().BeOfType<ServerResponse<CategoryResponse>>().Subject;
        response.Result.Name.Should().Be("New Category");
    }


    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenCategoryNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new CategoryRequest { Name = "Test Category" };
        var validationResult = new ValidationResult();
        A.CallTo(() => _validator.Validate(request)).Returns(validationResult);
        A.CallTo(() => _categoryRepository.GetAsync(id)).Returns((Category?)null);
        A.CallTo(() => _categoryRepository.GetAsync(A<string>.Ignored)).Returns((Category?)null);

        // Act
        var result = await _controller.UpdateAsync(id, request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>()
              .Which.Value.Should().BeOfType<ServerResponse>()
              .Which.ErrorMessage.Should().Be("Category not found");
    }


    [Fact]
    public async Task UpdateAsync_ShouldReturnOk_WhenValidRequest()
    {
        var id = Guid.NewGuid();
        var category = new Category { Id = id, Name = "Old Name" };
        var request = new CategoryRequest { Name = "Updated Name" };
        var validationResult = new ValidationResult();
        A.CallTo(() => _validator.Validate(request)).Returns(validationResult);
        A.CallTo(() => _categoryRepository.GetAsync(id)).Returns(category);
        A.CallTo(() => _categoryRepository.GetAsync(A<string>.Ignored)).Returns((Category?)null);

        var result = await _controller.UpdateAsync(id, request);

        result.Should().BeOfType<OkObjectResult>()
              .Which.Value.Should().BeOfType<ServerResponse<CategoryResponse>>()
              .Which.Result.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenCategoryNotExist()
    {
        var id = Guid.NewGuid();
        A.CallTo(() => _categoryRepository.GetAsync(id)).Returns((Category?)null);

        var result = await _controller.DeleteAsync(id);

        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Be("Category not found");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnBadRequest_WhenCategoryHasChildren()
    {
        var id = Guid.NewGuid();
        var category = new Category { Id = id };
        A.CallTo(() => _categoryRepository.GetAsync(id)).Returns(category);
        A.CallTo(() => _categoryRepository.GetChildrenCountAsync(id)).Returns(1);

        var result = await _controller.DeleteAsync(id);

        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Be("Cannot delete category with children");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnOk_WhenValidRequest()
    {
        var id = Guid.NewGuid();
        var category = new Category { Id = id };
        A.CallTo(() => _categoryRepository.GetAsync(id)).Returns(category);
        A.CallTo(() => _categoryRepository.GetChildrenCountAsync(id)).Returns(0);
        A.CallTo(() => _movimentationRepository.GetCategoryCountAsync(id)).Returns(0);

        var result = await _controller.DeleteAsync(id);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeOfType<ServerResponse>();
    }

    [Fact]
    public async Task ListAsync_ShouldReturnOk_WithCategories()
    {
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Category 1" },
            new Category { Id = Guid.NewGuid(), Name = "Category 2" }
        };
        A.CallTo(() => _categoryRepository.ListAsync(null)).Returns(categories);

        var result = await _controller.ListAsync();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ServerResponse<IEnumerable<CategoryResponse>>>().Subject;
        response.Result.Should().HaveCount(2);
        response.Result.First().Name.Should().Be("Category 1");
    }

    [Fact]
    public async Task ListChildrenAsync_ShouldReturnNotFound_WhenParentCategoryNotExist()
    {
        var parentId = Guid.NewGuid();
        A.CallTo(() => _categoryRepository.GetAsync(parentId)).Returns((Category?)null);

        var result = await _controller.ListChildrenAsync(parentId);

        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ServerResponse>().Subject;
        response.ErrorMessage.Should().Be("Category not found");
    }

    [Fact]
    public async Task ListChildrenAsync_ShouldReturnOk_WithChildrenCategories()
    {
        var parentId = Guid.NewGuid();
        var parentCategory = new Category { Id = parentId, Name = "Parent Category" };
        var children = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Child 1", ParentId = parentId },
            new Category { Id = Guid.NewGuid(), Name = "Child 2", ParentId = parentId }
        };
        A.CallTo(() => _categoryRepository.GetAsync(parentId)).Returns(parentCategory);
        A.CallTo(() => _categoryRepository.ListAsync(parentId)).Returns(children);

        var result = await _controller.ListChildrenAsync(parentId);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ServerResponse<IEnumerable<CategoryResponse>>>().Subject;
        response.Result.Should().HaveCount(2);
        response.Result.First().Name.Should().Be("Child 1");
    }
}

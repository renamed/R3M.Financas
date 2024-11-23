using FluentAssertions;
using R3M.Financas.Shared.Dtos;
using R3M.Financas.Shared.Validators;
using System.ComponentModel;

namespace R3M.Financas.Shared.UnitTests.Validators;

[Category("UnitTest")]
public class CategoryRequestValidatorUnitTest
{

    private readonly CategoryRequestValidator _validator = new();

    [Fact]
    public void Name_ShouldNotBeNull()
    {
        var category = new CategoryRequest
        {
            Name = null
        };

        var validationResult = _validator.Validate(category);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name is required");
    }

    [Fact]
    public void Name_ShouldNotBeEmpty()
    {
        var category = new CategoryRequest
        {
            Name = string.Empty
        };

        var validationResult = _validator.Validate(category);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name is required");
    }

    [Fact]
    public void Name_ShouldNotBeEmptySpace()
    {
        var category = new CategoryRequest
        {
            Name = "         "
        };

        var validationResult = _validator.Validate(category);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name is required");
    }

    [Fact]
    public void Name_ShouldHaveLengthMoreThan3()
    {
        var category = new CategoryRequest
        {
            Name = "ab"
        };

        var validationResult = _validator.Validate(category);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name length must be at least 3");
    }

    [Fact]
    public void Name_ShouldHaveLengthLessThan20()
    {
        var category = new CategoryRequest
        {
            Name = new string('a', 21)
        };

        var validationResult = _validator.Validate(category);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name length must not exceed 20");
    }
}

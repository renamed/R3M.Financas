using FluentAssertions;
using R3M.Financas.Shared.Dtos;
using R3M.Financas.Shared.Validators;

namespace R3M.Financas.Shared.UnitTests.Validators;

[Trait("Category", "UnitTest")]
public class InstitutionRequestValidatorUnitTest
{
    private readonly InstitutionRequestValidator _validator = new();

    [Fact]
    public void Name_ShouldNotBeNull()
    {
        var institution = new InstitutionRequest
        {
            Name = null
        };

        var validationResult = _validator.Validate(institution);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name is required");
    }

    [Fact]
    public void Name_ShouldNotBeEmpty()
    {
        var institution = new InstitutionRequest
        {
            Name = string.Empty
        };

        var validationResult = _validator.Validate(institution);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name is required");
    }

    [Fact]
    public void Name_ShouldNotBeEmptySpace()
    {
        var institution = new InstitutionRequest
        {
            Name = "         "
        };

        var validationResult = _validator.Validate(institution);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name is required");
    }

    [Fact]
    public void Name_ShouldHaveLengthMoreThan2()
    {
        var institution = new InstitutionRequest
        {
            Name = "a"
        };

        var validationResult = _validator.Validate(institution);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name length must be at least 2");
    }

    [Fact]
    public void Name_ShouldHaveLengthLessThan20()
    {
        var institution = new InstitutionRequest
        {
            Name = new string('a', 21)
        };

        var validationResult = _validator.Validate(institution);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name length must not exceed 20");
    }
}

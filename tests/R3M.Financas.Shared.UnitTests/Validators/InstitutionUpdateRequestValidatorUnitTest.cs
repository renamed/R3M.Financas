using FluentAssertions;
using R3M.Financas.Shared.Dtos;
using System.ComponentModel;

namespace R3M.Financas.Shared.Validators;

[Trait("Category", "UnitTest")]
public class InstitutionUpdateRequestValidatorUnitTest
{
    private readonly InstitutionUpdateRequestValidator _validator = new();

    [Fact]
    public void Name_ShouldNotBeNull()
    {
        var institution = new InstitutionUpdateRequest
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
        var institution = new InstitutionUpdateRequest
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
        var institution = new InstitutionUpdateRequest
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
        var institution = new InstitutionUpdateRequest
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
        var institution = new InstitutionUpdateRequest
        {
            Name = new string('a', 21)
        };

        var validationResult = _validator.Validate(institution);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Name length must not exceed 20");
    }
}

using FluentAssertions;
using R3M.Financas.Shared.Dtos;
using R3M.Financas.Shared.Validators;

namespace R3M.Financas.Shared.UnitTests.Validators;

public class PeriodRequestValidatorUnitTest
{
    private readonly PeriodRequestValidator _validator = new();

    [Fact]
    public void Name_ShouldNotBeNull()
    {
        var period = new PeriodRequest
        {
            Description = null
        };

        var validationResult = _validator.Validate(period);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Description is required");
    }

    [Fact]
    public void Name_ShouldNotBeEmpty()
    {
        var period = new PeriodRequest
        {
            Description = string.Empty
        };

        var validationResult = _validator.Validate(period);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Description is required");
    }

    [Fact]
    public void Name_ShouldNotBeEmptySpace()
    {
        var period = new PeriodRequest
        {
            Description = "         "
        };

        var validationResult = _validator.Validate(period);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Description is required");
    }

    [Fact]
    public void Name_ShouldHaveLengthLessThan6()
    {
        var period = new PeriodRequest
        {
            Description = new string('a', 7)
        };

        var validationResult = _validator.Validate(period);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Description length must not exceed 6");
    }

    [Fact]
    public void InitialDate_ShouldHaveValue()
    {
        var period = new PeriodRequest
        {
            InitialDate = default,
            Description = "123456"
        };

        var validationResult = _validator.Validate(period);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Initial Date is required");
    }

    [Fact]
    public void FinalDate_ShouldHaveValue()
    {
        var period = new PeriodRequest
        {
            FinalDate = default,
            Description = "123456"
        };

        var validationResult = _validator.Validate(period);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Final Date is required");
    }

    [Fact]
    public void FinalDate_ShouldBeBeforeInitialDate()
    {
        var period = new PeriodRequest
        {
            FinalDate = new DateOnly(2024,11,22),
            InitialDate = new DateOnly(2024, 11, 23),
            Description = "123456"
        };

        var validationResult = _validator.Validate(period);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Select(x => x.ErrorMessage).Should().Contain("Final date cannot be greater than the initial date.");
    }

    [Fact]
    public void ShouldHabeNoErrors()
    {
        var period = new PeriodRequest
        {
            FinalDate = new DateOnly(2024, 11, 22),
            InitialDate = new DateOnly(2024, 11, 20),
            Description = "123456"
        };

        var validationResult = _validator.Validate(period);

        validationResult.IsValid.Should().BeTrue();
    }
}

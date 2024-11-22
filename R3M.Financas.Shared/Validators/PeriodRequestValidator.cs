using FluentValidation;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Shared.Validators;

public class PeriodRequestValidator : AbstractValidator<PeriodRequest>
{
    public PeriodRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(6).WithMessage("{PropertyName} length must not exceed {MaxLength}");

        RuleFor(x => x.InitialDate)
            .NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(x => x.FinalDate)
            .NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(x => x)
            .Must(x => x.FinalDate > x.InitialDate)
            .WithMessage("Final date cannot be greater than the initial date.");

    }
}

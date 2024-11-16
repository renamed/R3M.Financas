using FluentValidation;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Shared.Validators;

public class PeriodRequestValidator : AbstractValidator<PeriodRequest>
{
    public PeriodRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is mandatory.")
            .Length(5).WithMessage("Description length must not exceed 5.");

        RuleFor(x => x.InitialDate)
            .NotEmpty().WithMessage("Initial date is mandatory");

        RuleFor(x => x.FinalDate)
            .NotEmpty().WithMessage("Final date is mandatory");

        RuleFor(x => x)
            .Must(x => x.FinalDate > x.InitialDate)
            .WithMessage("Final date cannot be later than the initial date.");

    }
}

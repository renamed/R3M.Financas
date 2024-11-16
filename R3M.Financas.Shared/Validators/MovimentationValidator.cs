using FluentValidation;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Shared.Validators;

public class MovimentationValidator : AbstractValidator<MovimentationRequest>
{
    public MovimentationValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MinimumLength(3).WithMessage("{PropertyName} length must be at least {MinLength}")
            .MaximumLength(30).WithMessage("{PropertyName} length must not exceed {MaxLength}");
    }
}

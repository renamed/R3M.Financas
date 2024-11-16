using FluentValidation;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Shared.Validators;

public class InstitutionUpdateRequestValidator : AbstractValidator<InstitutionUpdateRequest>
{
    public InstitutionUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("{PropertyName} is required")
        .MinimumLength(2).WithMessage("{PropertyName} length must be at least {MinLength}")
        .MaximumLength(20).WithMessage("{PropertyName} length must not exceed {MaxLength}");
    }
}

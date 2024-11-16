using FluentValidation;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Shared.Validators;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is mandatory")
            .MaximumLength(20).WithMessage("Name length must not exceed 20.")
            .MinimumLength(3).WithMessage("Name length must be at least 3.");
    }
}

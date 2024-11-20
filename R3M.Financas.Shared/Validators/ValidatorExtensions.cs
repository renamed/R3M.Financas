using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Shared.Validators;

public static class ValidatorExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection sc)
        => sc
        .AddSingleton<IValidator<CategoryRequest>, CategoryRequestValidator>()
        .AddSingleton<IValidator<InstitutionRequest>, InstitutionRequestValidator>()
        .AddSingleton<IValidator<InstitutionUpdateRequest>, InstitutionUpdateRequestValidator>()
        .AddSingleton<IValidator<MovimentationRequest>, MovimentationRequestValidator>()
        .AddSingleton<IValidator<PeriodRequest>, PeriodRequestValidator>();
}

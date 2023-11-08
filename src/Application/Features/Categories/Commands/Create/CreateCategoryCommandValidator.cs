using Application.Features.Categories.Constants;
using FluentValidation;

namespace Application.Features.Categories.Commands.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage(CategoryValidationErrorMesages.CategoryNameMustNotBeEmpty);
        RuleFor(c => c.Name).MinimumLength(2).WithMessage(CategoryValidationErrorMesages.CategoryNameMustBeBetween2And100Characters);
        RuleFor(c => c.Name).MaximumLength(100).WithMessage(CategoryValidationErrorMesages.CategoryNameMustBeBetween2And100Characters);

    }
}
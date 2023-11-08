using Application.Features.Categories.Constants;
using FluentValidation;

namespace Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage(CategoryValidationErrorMesages.CategoryNameMustNotBeEmpty);
        RuleFor(c => c.Id).GreaterThan(0).WithMessage(CategoryValidationErrorMesages.CategoryIdMustBeGreaterThanZero);
    }
}
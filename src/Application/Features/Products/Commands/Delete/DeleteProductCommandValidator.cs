using Application.Features.Products.Constants;
using FluentValidation;

namespace Application.Features.Products.Commands.Delete;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage(ProductValidationErrorMessages.ProductIdCanNotBeEmpty);
        RuleFor(p => p.Id).GreaterThan(0).WithMessage(ProductValidationErrorMessages.ProductIdMustBeGreaterThan0);
    }
}
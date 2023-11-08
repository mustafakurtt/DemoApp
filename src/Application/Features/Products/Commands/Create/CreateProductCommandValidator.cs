using Application.Features.Products.Constants;
using FluentValidation;

namespace Application.Features.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage(ProductValidationErrorMessages.ProductNameCanNotBeEmpty);
        RuleFor(p => p.Name).MinimumLength(2).WithMessage(ProductValidationErrorMessages.ProductNameMustBeAtLeast2Characters);

        RuleFor(p => p.Price).NotEmpty().WithMessage(ProductValidationErrorMessages.ProductPriceCanNotBeEmpty);
        RuleFor(p => p.Price).GreaterThan(0).WithMessage(ProductValidationErrorMessages.ProductPriceMustBeGreaterThan0);

        RuleFor(p => p.CategoryId).NotEmpty().WithMessage(ProductValidationErrorMessages.ProductCategoryIdCanNotBeEmpty);
        RuleFor(p => p.CategoryId).GreaterThan(0).WithMessage(ProductValidationErrorMessages.ProductCategoryIdMustBeGreaterThan0);

        RuleFor(p => p.SupplierId).NotEmpty().WithMessage(ProductValidationErrorMessages.ProductSupplierIdCanNotBeEmpty);
        RuleFor(p => p.SupplierId).GreaterThan(0).WithMessage(ProductValidationErrorMessages.ProductSupplierIdMustBeGreaterThan0);

    }
}
using Application.Features.Suppliers.Constants;
using FluentValidation;

namespace Application.Features.Suppliers.Commands.Create;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage(SupplierValidationErrorMessages.NameIsRequired);
        RuleFor(p => p.Name).MinimumLength(3).WithMessage(SupplierValidationErrorMessages.NameLengthMustBeGreaterThan3);
    }
}
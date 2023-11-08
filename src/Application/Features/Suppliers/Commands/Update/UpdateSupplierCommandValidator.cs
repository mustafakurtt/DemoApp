using Application.Features.Suppliers.Constants;
using FluentValidation;

namespace Application.Features.Suppliers.Commands.Update;

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage(SupplierValidationErrorMessages.IdIsRequired);
        RuleFor(c => c.Id).GreaterThan(0).WithMessage(SupplierValidationErrorMessages.IdMustBeGreaterThan0);

        RuleFor(c => c.Name).NotEmpty().WithMessage(SupplierValidationErrorMessages.NameIsRequired);
        RuleFor(c => c.Name).MinimumLength(3).WithMessage(SupplierValidationErrorMessages.NameLengthMustBeGreaterThan3);

    }
}
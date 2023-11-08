using Application.Features.Suppliers.Constants;
using FluentValidation;

namespace Application.Features.Suppliers.Commands.Delete;

public class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
{
    public DeleteSupplierCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage(SupplierValidationErrorMessages.IdIsRequired);
    }
}
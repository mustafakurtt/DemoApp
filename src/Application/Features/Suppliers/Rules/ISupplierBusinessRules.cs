using Domain.Entities;

namespace Application.Features.Suppliers.Rules;

public interface ISupplierBusinessRules
{
    public Task SupplierShouldExistsWhenSelected(Supplier? supplier);
    public Task SupplierNameCanNotBeDuplicatedWhenInserted(string name);
}
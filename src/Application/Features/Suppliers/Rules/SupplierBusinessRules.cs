using Application.Features.Suppliers.Constants;
using Application.Repositories;
using Core.Application.Rules;
using Domain.Entities;

namespace Application.Features.Suppliers.Rules;

public class SupplierBusinessRules : BaseBusinessRules, ISupplierBusinessRules
{
    private readonly ISupplierRepository _supplierRepository;
    public SupplierBusinessRules(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Task SupplierShouldExistsWhenSelected(Supplier? supplier)
    {
        if (supplier == null)
            throw new Exception(SupplierBusinessErrorMesages.SupplierCanNotBeFound);
        return Task.CompletedTask;
    }

    public async Task SupplierNameCanNotBeDuplicatedWhenInserted(string name)
    {
        Supplier? supplier = await _supplierRepository.GetAsync(p => p.Name == name);
        if (supplier != null)
            throw new Exception(SupplierBusinessErrorMesages.SupplierNameCanNotBeDuplicated);
        return;
    }
}
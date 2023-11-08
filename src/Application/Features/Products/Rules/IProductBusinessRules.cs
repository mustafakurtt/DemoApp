using Domain.Entities;

namespace Application.Features.Products.Rules;

public interface IProductBusinessRules
{
    public Task ProductShouldExistsWhenSelected(Product? product);
    public Task CategoryShouldExistsWhenSelected(Product product);
    public Task ProductNameCanNotBeDuplicatedWhenInserted(Product product);
    public Task SupplierShouldExistsWhenSelected(Product product);
    public Task ProductNameCanNotBeDuplicatedWhenUpdated(Product product);
}
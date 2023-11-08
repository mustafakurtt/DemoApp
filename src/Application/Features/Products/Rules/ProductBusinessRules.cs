using Application.Features.Products.Constants;
using Application.Repositories;
using Core.Application.Rules;
using Domain.Entities;

namespace Application.Features.Products.Rules;

public class ProductBusinessRules : BaseBusinessRules, IProductBusinessRules
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISupplierRepository _supplierRepository;

    public ProductBusinessRules(IProductRepository productRepository, ICategoryRepository categoryRepository, ISupplierRepository supplierRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _supplierRepository = supplierRepository;
    }

    public Task ProductShouldExistsWhenSelected(Product? product)
    {
        if (product == null) throw new Exception(ProductBusinessErrorMessages.ProductCanNotBeFound);
        return Task.CompletedTask;
    }

    public async Task CategoryShouldExistsWhenSelected(Product product)
    {
        Category? category = await _categoryRepository.GetAsync(p => p.Id == product.CategoryId, enableTracking: false);
        if (category == null) throw new Exception(ProductBusinessErrorMessages.CategoryCanNotBeFound);
        return;
    }

    public async Task ProductNameCanNotBeDuplicatedWhenInserted(Product product)
    {
        Product? _product = await _productRepository.GetAsync(p => p.Name == product.Name, enableTracking: false);
        if (_product != null) throw new Exception(ProductBusinessErrorMessages.ProductNameAlreadyExists);
        return;
    }

    public async Task SupplierShouldExistsWhenSelected(Product product)
    {
        Supplier? supplier = await _supplierRepository.GetAsync(p => p.Id == product.SupplierId, enableTracking: false);
        if (supplier == null) throw new Exception(ProductBusinessErrorMessages.SupplierCanNotBeFound);
        return;

    }

    public async Task ProductNameCanNotBeDuplicatedWhenUpdated(Product product)
    {
        Product? _product = await _productRepository.GetAsync(p => p.Name == product.Name && p.Id != product.Id, enableTracking: false);
        if (_product != null) throw new Exception(ProductBusinessErrorMessages.ProductNameAlreadyExists);
        return;
    }
}
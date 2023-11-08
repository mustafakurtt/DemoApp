using System.Linq.Expressions;
using Application.Features.Products.Constants;
using Application.Features.Products.Rules;
using Application.Repositories;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace ProductCommandTests;

public class ProductBusinessRulesTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<ISupplierRepository> _supplierRepositoryMock;
    private ProductBusinessRules _productBusinessRules;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _productBusinessRules = new ProductBusinessRules(_productRepositoryMock.Object, _categoryRepositoryMock.Object, _supplierRepositoryMock.Object);
    }

    [Test]
    public async Task ProductShouldExistsWhenSelected_WithExistingProduct_ShouldNotThrowException()
    {
        //Arrange
        var product = new Product { Id = 1, Name = "Existing Product" };
        _productRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        //Act
        Func<Task> act = async () => await _productBusinessRules.ProductShouldExistsWhenSelected(product);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task ProductShouldExistsWhenSelected_WithNullProduct_ShouldThrowException()
    {
        //Arrange
        Product product = null;

        //Act
        Func<Task> act = async () => await _productBusinessRules.ProductShouldExistsWhenSelected(product);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(ProductBusinessErrorMessages.ProductCanNotBeFound);
    }

    [Test]
    public async Task CategoryShouldExistsWhenSelected_WithExistingCategory_ShouldNotThrowException()
    {
        //Arrange
        var product = new Product { Id = 1, Name = "Existing Product", CategoryId = 1 };
        var category = new Category { Id = 1, Name = "Existing Category" };
        _categoryRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        //Act
        Func<Task> act = async () => await _productBusinessRules.CategoryShouldExistsWhenSelected(product);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task CategoryShouldExistsWhenSelected_WithNullCategory_ShouldThrowException()
    {
        //Arrange
        var product = new Product { Id = 1, Name = "Existing Product", CategoryId = 1 };
        _categoryRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null);

        //Act
        Func<Task> act = async () => await _productBusinessRules.CategoryShouldExistsWhenSelected(product);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(ProductBusinessErrorMessages.CategoryCanNotBeFound);
    }

    [Test]
    public async Task SupplierShouldExistsWhenSelected_WithExistingSupplier_ShouldNotThrowException()
    {
        //Arrange
        var product = new Product { Id = 1, Name = "Existing Product", SupplierId = 1 };
        var supplier = new Supplier { Id = 1, Name = "Existing Supplier" };
        _supplierRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Supplier, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);
        //Act
        Func<Task> act = async () => await _productBusinessRules.SupplierShouldExistsWhenSelected(product);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task SupplierShouldExistsWhenSelected_WithNullSupplier_ShouldThrowException()
    {
        //Arrange
        var product = new Product { Id = 1, Name = "Existing Product", SupplierId = 1 };
        _supplierRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Supplier, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier)null);

        //Act
        Func<Task> act = async () => await _productBusinessRules.SupplierShouldExistsWhenSelected(product);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(ProductBusinessErrorMessages.SupplierCanNotBeFound);
    }

    [Test]
    public async Task ProductNameCanNotBeDuplicatedWhenInserted_WithUniqueName_ShouldNotThrowException()
    {
        //Arrange
        string uniqueName = "Unique Product Name";
        var product = new Product { Id = 1, Name = uniqueName };
        _productRepositoryMock.Setup(repo => repo.GetAsync(
            It.IsAny<Expression<Func<Product, bool>>>(),
            null,
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Product)null);

        //Act
        Func<Task> act = async () => await _productBusinessRules.ProductNameCanNotBeDuplicatedWhenInserted(product);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task ProductNameCanNotBeDuplicatedWhenInserted_WithDuplicatedName_ShouldThrowException()
    {
        //Arrange
        string duplicatedName = "Duplicated Product Name";
        var product = new Product { Id = 1, Name = duplicatedName };
        _productRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        //Act
        Func<Task> act = async () => await _productBusinessRules.ProductNameCanNotBeDuplicatedWhenInserted(product);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(ProductBusinessErrorMessages.ProductNameAlreadyExists);
    }


}
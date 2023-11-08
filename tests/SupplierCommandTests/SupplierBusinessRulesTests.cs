using System.Linq.Expressions;
using Application.Features.Suppliers.Constants;
using Application.Features.Suppliers.Rules;
using Application.Repositories;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace SupplierCommandTests;

public class SupplierBusinessRulesTests
{
    private Mock<ISupplierRepository> _supplierRepositoryMock;
    private SupplierBusinessRules _supplierBusinessRules;

    [SetUp]
    public void Setup()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _supplierBusinessRules = new SupplierBusinessRules(_supplierRepositoryMock.Object);
    }

    [Test]
    public async Task SupplierShouldExistsWhenSelected_WithExistingCategory_ShouldNotThrowException()
    {
        //Arrange
        var supplier = new Supplier { Id = 1, Name = "Existing Supplier" };
        _supplierRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Supplier, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);
        //Act
        Func<Task> act = async () => await _supplierBusinessRules.SupplierShouldExistsWhenSelected(supplier);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task SupplierShouldExistsWhenSelected_WithNullSupplier_ShouldThrowException()
    {
        //Arrange
        Supplier supplier = null;

        //Act
        Func<Task> act = async () => await _supplierBusinessRules.SupplierShouldExistsWhenSelected(supplier);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(SupplierBusinessErrorMesages.SupplierCanNotBeFound);
    }

    [Test]
    public async Task SupplierNameCanNotBeDuplicatedWhenInserted_WithUniqueName_ShouldNotThrowException()
    {
        //Arrange
        string uniqueName = "Unique Supplier Name";
        _supplierRepositoryMock.Setup(repo => repo.GetAsync(
            It.IsAny<Expression<Func<Supplier, bool>>>(),
            null,
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()));

        //Act
        Func<Task> act = async () => await _supplierBusinessRules.SupplierNameCanNotBeDuplicatedWhenInserted(uniqueName);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task SupplierNameCanNotBeDuplicatedWhenInserted_WithDuplicatedName_ShouldThrowException()
    {
        //Arrange
        string duplicatedName = "Duplicated Supplier Name";
        _supplierRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Supplier, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Supplier() { Name = duplicatedName });

        //Act
        Func<Task> act = async () => await _supplierBusinessRules.SupplierNameCanNotBeDuplicatedWhenInserted(duplicatedName);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(SupplierBusinessErrorMesages.SupplierNameCanNotBeDuplicated);
    }
}
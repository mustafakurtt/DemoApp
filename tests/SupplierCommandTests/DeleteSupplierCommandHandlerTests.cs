using Application.Features.Suppliers.Commands.Delete;
using Application.Features.Suppliers.Constants;
using Application.Features.Suppliers.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace SupplierCommandTests;

public class DeleteSupplierCommandHandlerTests
{
    private Mock<ISupplierRepository> _supplierRepositoryMock;
    private Mock<IUnitOfWork<ISupplierRepository>> _unitOfWorkMock;
    private Mock<ISupplierBusinessRules> _supplierBusinessRules;
    private Mock<IMapper> _mapperMock;
    private DeleteSupplierCommand.DeleteSupplierCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork<ISupplierRepository>>();
        _supplierBusinessRules = new Mock<ISupplierBusinessRules>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteSupplierCommand.DeleteSupplierCommandHandler(_supplierRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _supplierBusinessRules.Object);
    }

    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnDeletedSupplierCommandResponse()
    {
        //Arrange
        var request = new DeleteSupplierCommand { Id = 1 };
        var supplier = new Supplier { Id = 1, Name = "Supplier Name" };
        var response = new DeletedSupplierCommandResponse { Id = 1, Name = "Supplier Name" };

        _supplierRepositoryMock.Setup(repo =>
            repo.GetAsync(p => p.Id == request.Id,
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(supplier);

        _supplierBusinessRules.Setup(rules => rules.SupplierShouldExistsWhenSelected(supplier)).Returns(Task.CompletedTask);
        _supplierRepositoryMock.Setup(repo => repo.DeleteAsync(supplier, false));
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<DeletedSupplierCommandResponse>(supplier)).Returns(response);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        _supplierRepositoryMock.Verify(repo =>
            repo.GetAsync(p => p.Id == request.Id,
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);
        _supplierBusinessRules.Verify(rules => rules.SupplierShouldExistsWhenSelected(supplier), Times.Once);
        _supplierRepositoryMock.Verify(repo => repo.DeleteAsync(supplier, false), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<DeletedSupplierCommandResponse>(supplier), Times.Once);

        result.Should().BeOfType<DeletedSupplierCommandResponse>();
        result.Should().BeEquivalentTo(response);
    }

    [Test]
    public void Given_ValidId_ShouldPassValidation()
    {
        // Arrange
        var command = new DeleteSupplierCommand { Id = 1 };
        var validator = new DeleteSupplierCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Given_InvalidId_ShouldFailValidation()
    {
        // Arrange
        var command = new DeleteSupplierCommand { Id = 0 };
        var validator = new DeleteSupplierCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == SupplierValidationErrorMessages.IdIsRequired);
    }

}
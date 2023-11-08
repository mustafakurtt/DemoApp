using Application.Features.Suppliers.Commands.Update;
using Application.Features.Suppliers.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace SupplierCommandTests;

public class UpdateSupplierCommandHandlerTests
{
    private Mock<ISupplierRepository> _supplierRepositoryMock;
    private Mock<IUnitOfWork<ISupplierRepository>> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ISupplierBusinessRules> _supplierBusinessRules;
    private UpdateSupplierCommand.UpdateSupplierCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork<ISupplierRepository>>();
        _mapperMock = new Mock<IMapper>();
        _supplierBusinessRules = new Mock<ISupplierBusinessRules>();
        _handler = new UpdateSupplierCommand.UpdateSupplierCommandHandler(_supplierRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _supplierBusinessRules.Object);
    }

    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnUpdatedSupplierCommandResponse()
    {
        //Arrange
        var request = new UpdateSupplierCommand { Id = 1, Name = "Supplier Name" };
        var supplier = new Supplier { Id = 1, Name = "Supplier Name" };
        var response = new UpdatedSupplierCommandResponse { Id = 1, Name = "Supplier Name" };

        _supplierRepositoryMock.Setup(repo =>
                repo.GetAsync(s => s.Id == request.Id,
                    null,
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        _supplierBusinessRules.Setup(rules => rules.SupplierShouldExistsWhenSelected(supplier)).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map(request, supplier)).Returns(supplier);
        _supplierRepositoryMock.Setup(repo => repo.UpdateAsync(supplier));
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<UpdatedSupplierCommandResponse>(supplier)).Returns(response);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        _supplierRepositoryMock.Verify(repo =>
            repo.GetAsync(s => s.Id == request.Id,
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);

        _supplierBusinessRules.Verify(rules => rules.SupplierShouldExistsWhenSelected(supplier), Times.Once);
        _supplierRepositoryMock.Verify(repo => repo.UpdateAsync(supplier), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map(request, supplier), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<UpdatedSupplierCommandResponse>(supplier), Times.Once);

        result.Should().BeOfType<UpdatedSupplierCommandResponse>();
        result.Should().BeEquivalentTo(response);

    }

    [Test]
    public void Given_ValidName_ShouldPassValidation()
    {
        // Arrange
        var command = new UpdateSupplierCommand { Id = 1, Name = "Supplier Name" };
        var validator = new UpdateSupplierCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Given_EmptyName_ShouldFailValidation()
    {
        // Arrange
        var command = new UpdateSupplierCommand { Id = 1, Name = "" };
        var validator = new UpdateSupplierCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public void Given_InvalidId_ShouldFailValidation()
    {
        // Arrange
        var command = new UpdateSupplierCommand { Id = 0, Name = "Supplier Name" };
        var validator = new UpdateSupplierCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public void Given_NullName_ShouldFailValidation()
    {
        // Arrange
        var command = new UpdateSupplierCommand { Id = 1, Name = null };
        var validator = new UpdateSupplierCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
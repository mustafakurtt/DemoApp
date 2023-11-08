using Application.Features.Suppliers.Commands.Create;
using Application.Features.Suppliers.Constants;
using Application.Features.Suppliers.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace SupplierCommandTests;

public class CreateSupplierCommandHandlerTests
{
    private Mock<ISupplierRepository> _supplierRepositoryMock;
    private Mock<IUnitOfWork<ISupplierRepository>> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ISupplierBusinessRules> _supplierBusinessRules;
    private CreateSupplierCommand.CreateSupplierCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork<ISupplierRepository>>();
        _mapperMock = new Mock<IMapper>();
        _supplierBusinessRules = new Mock<ISupplierBusinessRules>();
        _handler = new CreateSupplierCommand.CreateSupplierCommandHandler(_supplierRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _supplierBusinessRules.Object);
    }


    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnCreatedSupplierCommandResponse()
    {
        //Arrange
        var request = new CreateSupplierCommand { Name = "Supplier Name" };
        var supplier = new Supplier { Id = 1, Name = "Supplier Name" };
        var response = new CreatedSupplierCommandResponse { Id = 1, Name = "Supplier Name" };

        _mapperMock.Setup(mapper => mapper.Map<Supplier>(request)).Returns(supplier);
        _supplierBusinessRules.Setup(rules => rules.SupplierNameCanNotBeDuplicatedWhenInserted(supplier.Name)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<CreatedSupplierCommandResponse>(supplier)).Returns(response);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        _supplierRepositoryMock.Verify(repo => repo.AddAsync(supplier), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<Supplier>(request), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CreatedSupplierCommandResponse>(supplier), Times.Once);

        result.Should().BeOfType<CreatedSupplierCommandResponse>();
        result.Should().BeEquivalentTo(response);
    }

    [Test]
    public void Given_ValidName_ShouldPassValidation()
    {
        // Arrange
        var command = new CreateSupplierCommand { Name = "Supplier Name" };
        var validator = new CreateSupplierCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Given_EmptyName_ShouldFailValidation()
    {
        // Arrange
        var command = new CreateSupplierCommand { Name = "" };
        var validator = new CreateSupplierCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(x => x.ErrorMessage == SupplierValidationErrorMessages.NameLengthMustBeGreaterThan3);
    }
}
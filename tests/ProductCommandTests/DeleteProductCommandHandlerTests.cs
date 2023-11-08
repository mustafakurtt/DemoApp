using Application.Features.Products.Commands.Delete;
using Application.Features.Products.Constants;
using Application.Features.Products.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace ProductCommandTests;

public class DeleteProductCommandHandlerTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IUnitOfWork<IProductRepository>> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IProductBusinessRules> _businessRulesMock;
    private DeleteProductCommand.DeleteProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork<IProductRepository>>();
        _mapperMock = new Mock<IMapper>();
        _businessRulesMock = new Mock<IProductBusinessRules>();
        _handler = new DeleteProductCommand.DeleteProductCommandHandler(_productRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _businessRulesMock.Object);
    }

    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnDeletedProductCommandResponse()
    {
        //Arrange
        var request = new DeleteProductCommand { Id = 1 };
        var product = new Product { Id = 1, Name = "Product Name" };
        var response = new DeletedProductCommandResponse { Id = 1 };

        _productRepositoryMock.Setup(repo => repo.GetAsync(p => p.Id == request.Id, null, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(product);
        _businessRulesMock.Setup(rules => rules.ProductShouldExistsWhenSelected(product)).Returns(Task.CompletedTask);
        _productRepositoryMock.Setup(repo => repo.DeleteAsync(product, false));
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<DeletedProductCommandResponse>(product)).Returns(response);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        _productRepositoryMock.Verify(repo => repo.GetAsync(p => p.Id == request.Id, null, It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        _businessRulesMock.Verify(rules => rules.ProductShouldExistsWhenSelected(product), Times.Once);
        _productRepositoryMock.Verify(repo => repo.DeleteAsync(product, false), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<DeletedProductCommandResponse>(product), Times.Once);
        result.Should().BeEquivalentTo(response);
    }

    [Test]
    public void Given_ValidId_ShouldPassValidation()
    {
        //Arrange
        var command = new DeleteProductCommand { Id = 1 };
        var validator = new DeleteProductCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Given_InvalidId_ShouldFailValidation()
    {
        //Arrange
        var command = new DeleteProductCommand { Id = 0 };
        var validator = new DeleteProductCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ProductValidationErrorMessages.ProductIdMustBeGreaterThan0);
    }
}
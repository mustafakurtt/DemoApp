using Application.Features.Products.Commands.Update;
using Application.Features.Products.Constants;
using Application.Features.Products.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace ProductCommandTests;

public class UpdateProductCommandHandlerTests
{
    private Mock<IProductRepository> _productRepository;
    private Mock<IUnitOfWork<IProductRepository>> _unitOfWork;
    private Mock<IProductBusinessRules> _productBusinessRules;
    private Mock<IMapper> _mapper;
    private Mock<UpdateProductCommand.UpdateProductCommandHandler> _handler;

    [SetUp]
    public void Setup()
    {
        _productRepository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork<IProductRepository>>();
        _productBusinessRules = new Mock<IProductBusinessRules>();
        _mapper = new Mock<IMapper>();
        _handler = new Mock<UpdateProductCommand.UpdateProductCommandHandler>(_productRepository.Object, _unitOfWork.Object, _mapper.Object, _productBusinessRules.Object);
    }

    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnUpdatedProductCommandResponse()
    {
        //Arrange
        var request = new UpdateProductCommand { Id = 1, Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };
        var product = new Product { Id = 1, Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };
        var response = new UpdatedProductCommandResponse { Id = 1, Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };

        _productRepository.Setup(repo => repo.GetAsync(
                p => p.Id == request.Id,
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _productBusinessRules.Setup(rules => rules.ProductShouldExistsWhenSelected(product)).Returns(Task.CompletedTask);
        _productBusinessRules.Setup(rules => rules.CategoryShouldExistsWhenSelected(product)).Returns(Task.CompletedTask);
        _productBusinessRules.Setup(rules => rules.SupplierShouldExistsWhenSelected(product)).Returns(Task.CompletedTask);
        _productBusinessRules.Setup(rules => rules.ProductNameCanNotBeDuplicatedWhenUpdated(product)).Returns(Task.CompletedTask);

        _mapper.Setup(mapper => mapper.Map(request, product)).Returns(product);
        _productRepository.Setup(repo => repo.UpdateAsync(product));
        _unitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapper.Setup(mapper => mapper.Map<UpdatedProductCommandResponse>(product)).Returns(response);

        //Act
        var result = await _handler.Object.Handle(request, CancellationToken.None);

        //Assert
        result.Should().BeOfType<UpdatedProductCommandResponse>();
        result.Should().BeEquivalentTo(response);

        _productRepository.Verify(repo => repo.GetAsync(p => p.Id == request.Id,
            null,
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()), Times.Once);
        _productRepository.Verify(repo => repo.UpdateAsync(product), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapper.Verify(mapper => mapper.Map(request, product), Times.Once);
        _mapper.Verify(mapper => mapper.Map<UpdatedProductCommandResponse>(product), Times.Once);
        _productBusinessRules.Verify(rules => rules.ProductShouldExistsWhenSelected(product), Times.Once);
        _productBusinessRules.Verify(rules => rules.CategoryShouldExistsWhenSelected(product), Times.Once);
        _productBusinessRules.Verify(rules => rules.SupplierShouldExistsWhenSelected(product), Times.Once);
        _productBusinessRules.Verify(rules => rules.ProductNameCanNotBeDuplicatedWhenUpdated(product), Times.Once);

    }

    [Test]
    public void Given_ValidRequest_ShouldPassValidation()
    {
        //Arrange
        var command = new UpdateProductCommand { Id = 1, Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };
        var validator = new UpdateProductCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeTrue();

    }

    [Test]
    public void Given_EmptyName_ShouldFailValidation()
    {
        //Arrange
        var command = new UpdateProductCommand { Id = 1, Name = "", Price = 10, CategoryId = 1, SupplierId = 1 };
        var validator = new UpdateProductCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ProductValidationErrorMessages.ProductNameMustBeAtLeast2Characters);
    }

    [Test]
    public void Given_PriceLessThanZero_ShouldFailValidation()
    {
        //Arrange
        var command = new UpdateProductCommand { Id = 1, Name = "Product Name", Price = -1, CategoryId = 1, SupplierId = 1 };
        var validator = new UpdateProductCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ProductValidationErrorMessages.ProductPriceMustBeGreaterThan0);
    }

    [Test]
    public void Given_InvalidId_ShouldFailValidation()
    {
        //Arrange
        var command = new UpdateProductCommand { Id = 0, Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };
        var validator = new UpdateProductCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ProductValidationErrorMessages.ProductIdMustBeGreaterThan0);
    }

    [Test]
    public void Given_NullName_ShouldFailValidation()
    {
        //Arrange
        var command = new UpdateProductCommand { Id = 1, Name = null, Price = 10, CategoryId = 1, SupplierId = 1 };
        var validator = new UpdateProductCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ProductValidationErrorMessages.ProductNameCanNotBeEmpty);
    }
}
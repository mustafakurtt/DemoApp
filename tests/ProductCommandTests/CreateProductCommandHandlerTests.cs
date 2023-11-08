using Application.Features.Products.Commands.Create;
using Application.Features.Products.Constants;
using Application.Features.Products.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace ProductCommandTests;

public class CreateProductCommandHandlerTests
{
    private Mock<IProductRepository> _productRepository;
    private Mock<IUnitOfWork<IProductRepository>> _unitOfWork;
    private Mock<IProductBusinessRules> _productBusinessRules;
    private Mock<IMapper> _mapper;
    private Mock<CreateProductCommand.CreateProductCommandHandler> _handler;

    [SetUp]
    public void Setup()
    {
        _productRepository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork<IProductRepository>>();
        _productBusinessRules = new Mock<IProductBusinessRules>();
        _mapper = new Mock<IMapper>();
        _handler = new Mock<CreateProductCommand.CreateProductCommandHandler>(_productRepository.Object, _unitOfWork.Object, _mapper.Object, _productBusinessRules.Object);
    }

    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnCreatedProductCommandResponse()
    {
        //Arrange
        var request = new CreateProductCommand { Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };
        var product = new Product { Id = 1, Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };
        var response = new CreatedProductCommandResponse { Id = 1, Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };

        _mapper.Setup(mapper => mapper.Map<Product>(request)).Returns(product);
        _productBusinessRules.Setup(rules => rules.CategoryShouldExistsWhenSelected(product)).Returns(Task.CompletedTask);
        _productBusinessRules.Setup(rules => rules.SupplierShouldExistsWhenSelected(product)).Returns(Task.CompletedTask);
        _productBusinessRules.Setup(rules => rules.ProductNameCanNotBeDuplicatedWhenInserted(product)).Returns(Task.CompletedTask);
        _productRepository.Setup(repo => repo.AddAsync(product));
        _unitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapper.Setup(mapper => mapper.Map<CreatedProductCommandResponse>(product)).Returns(response);

        //Act
        var result = await _handler.Object.Handle(request, CancellationToken.None);

        //Assert
        result.Should().BeOfType<CreatedProductCommandResponse>();
        result.Should().BeEquivalentTo(response);

        _productRepository.Verify(repo => repo.AddAsync(product), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapper.Verify(mapper => mapper.Map<Product>(request), Times.Once);
        _mapper.Verify(mapper => mapper.Map<CreatedProductCommandResponse>(product), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);


    }

    [Test]
    public async Task Given_ValidName_ShouldPassValidation()
    {
        //Arrange 
        var command = new CreateProductCommand { Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 1 };
        var validator = new CreateProductCommandValidator();

        //Act
        var result = await validator.ValidateAsync(command);


        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task Given_EmptyName_ShouldFailValidation()
    {
        //Arrange
        var command = new CreateProductCommand { Name = "", Price = 10, CategoryId = 1, SupplierId = 1 };
        var validator = new CreateProductCommandValidator();

        //Act
        var result = await validator.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductValidationErrorMessages.ProductNameCanNotBeEmpty);
    }

    [Test]
    public async Task Given_InvalidPrice_ShouldFailValidation()
    {
        //Arrange
        var command = new CreateProductCommand { Name = "Product Name", Price = 0, CategoryId = 1, SupplierId = 1 };
        var validator = new CreateProductCommandValidator();

        //Act
        var result = await validator.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductValidationErrorMessages.ProductPriceMustBeGreaterThan0);
    }

    [Test]
    public async Task Given_InvalidCategoryId_ShouldFailValidation()
    {
        //Arrange
        var command = new CreateProductCommand { Name = "Product Name", Price = 10, CategoryId = 0, SupplierId = 1 };
        var validator = new CreateProductCommandValidator();

        //Act
        var result = await validator.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductValidationErrorMessages.ProductCategoryIdMustBeGreaterThan0);
    }

    [Test]
    public async Task Given_InvalidSupplierId_ShouldFailValidation()
    {
        //Arrange
        var command = new CreateProductCommand { Name = "Product Name", Price = 10, CategoryId = 1, SupplierId = 0 };
        var validator = new CreateProductCommandValidator();

        //Act
        var result = await validator.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductValidationErrorMessages.ProductSupplierIdMustBeGreaterThan0);
    }

}
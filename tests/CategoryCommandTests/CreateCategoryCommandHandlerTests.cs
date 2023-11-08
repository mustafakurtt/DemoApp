using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Constants;
using Application.Features.Categories.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace CategoryCommandTests;

public class CreateCategoryCommandHandlerTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IUnitOfWork<ICategoryRepository>> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ICategoryBusinessRules> _categoryBusinessRules;
    private CreateCategoryCommand.CreateCategoryCommandHandler _handler;
    [SetUp]
    public void Setup()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork<ICategoryRepository>>();
        _mapperMock = new Mock<IMapper>();
        _categoryBusinessRules = new Mock<ICategoryBusinessRules>();
        _handler = new CreateCategoryCommand.CreateCategoryCommandHandler(_categoryRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _categoryBusinessRules.Object);
    }

    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnCreatedCommandResponse()
    {
        //Arrange
        var request = new CreateCategoryCommand { Name = "Category Name" };
        var category = new Category { Id = 1, Name = "Category Name" };
        var response = new CreatedCategoryCommandResponse { Id = 1, Name = "Category Name" };

        _mapperMock.Setup(mapper => mapper.Map<Category>(request)).Returns(category);
        _categoryBusinessRules.Setup(rules => rules.CategoryNameCanNotBeDuplicatedWhenInserted(category.Name)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<CreatedCategoryCommandResponse>(category)).Returns(response);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        _categoryRepositoryMock.Verify(repo => repo.AddAsync(category), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<Category>(request), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CreatedCategoryCommandResponse>(category), Times.Once);

        result.Should().BeOfType<CreatedCategoryCommandResponse>();
        result.Should().BeEquivalentTo(response);
    }

    [Test]
    public void Given_ValidName_ShouldPassValidation()
    {
        //Arrange 
        var command = new CreateCategoryCommand { Name = "Category Name" };
        var validator = new CreateCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Given_EmptyName_ShouldFailValidation()
    {
        //Arrange
        var command = new CreateCategoryCommand { Name = "" };
        var validator = new CreateCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryValidationErrorMesages.CategoryNameMustNotBeEmpty);
    }

    [Test]
    public void Given_NameWithLessThan2Characters_ShouldFailValidation()
    {
        //Arrange
        var command = new CreateCategoryCommand { Name = "A" };
        var validator = new CreateCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryValidationErrorMesages.CategoryNameMustBeBetween2And100Characters);
    }

    [Test]
    public void Given_NameWithMoreThan100Characters_ShouldFailValidation()
    {
        //Arrange
        var command = new CreateCategoryCommand { Name = "A".PadLeft(101, 'A') };
        var validator = new CreateCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryValidationErrorMesages.CategoryNameMustBeBetween2And100Characters);
    }
}
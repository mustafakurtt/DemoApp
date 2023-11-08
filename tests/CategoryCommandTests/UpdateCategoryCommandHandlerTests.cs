using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Constants;
using Application.Features.Categories.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace CategoryCommandTests;

public class UpdateCategoryCommandHandlerTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IUnitOfWork<ICategoryRepository>> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ICategoryBusinessRules> _categoryBusinessRules;
    private UpdateCategoryCommand.UpdateCategoryCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork<ICategoryRepository>>();
        _mapperMock = new Mock<IMapper>();
        _categoryBusinessRules = new Mock<ICategoryBusinessRules>();
        _handler = new UpdateCategoryCommand.UpdateCategoryCommandHandler(_categoryRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _categoryBusinessRules.Object);
    }

    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnUpdatedCategoryCommandResponse()
    {
        //Arrange
        var request = new UpdateCategoryCommand { Id = 1, Name = "Category Name" };
        var category = new Category { Id = 1, Name = "Category Name" };
        var response = new UpdatedCategoryCommandResponse { Id = 1, Name = "Category Name" };

        _categoryRepositoryMock.Setup(repo =>
                repo.GetAsync(s => s.Id == request.Id,
                    null,
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _categoryBusinessRules.Setup(rules => rules.CategoryShouldExistsWhenSelected(category)).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map(request, category)).Returns(category);
        _categoryRepositoryMock.Setup(repo => repo.UpdateAsync(category));
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<UpdatedCategoryCommandResponse>(category)).Returns(response);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        _categoryRepositoryMock.Verify(repo =>
            repo.GetAsync(s => s.Id == request.Id,
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);

        _categoryBusinessRules.Verify(rules => rules.CategoryShouldExistsWhenSelected(category), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map(request, category), Times.Once);
        _categoryRepositoryMock.Verify(repo => repo.UpdateAsync(category), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<UpdatedCategoryCommandResponse>(category), Times.Once);
        result.Should().BeEquivalentTo(response);
    }

    [Test]
    public void Given_ValidName_ShouldPassValidation()
    {
        //Arrange
        var command = new UpdateCategoryCommand { Id = 1, Name = "Category Name" };
        var validator = new UpdateCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Given_EmptyName_ShouldFailValidation()
    {
        //Arrange
        var command = new UpdateCategoryCommand { Id = 1, Name = "" };
        var validator = new UpdateCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryValidationErrorMesages.CategoryNameMustNotBeEmpty);
    }

    [Test]
    public void Given_Invalid_ShouldFailValidation()
    {
        //Arrange
        var command = new UpdateCategoryCommand { Id = 0, Name = "Category Name" };
        var validator = new UpdateCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryValidationErrorMesages.CategoryIdMustBeGreaterThanZero);

    }

    [Test]
    public void Given_NullName_ShouldFailValidation()
    {
        //Arrange
        var command = new UpdateCategoryCommand { Id = 1, Name = null };
        var validator = new UpdateCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryValidationErrorMesages.CategoryNameMustNotBeEmpty);
    }

}
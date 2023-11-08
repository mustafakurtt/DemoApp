using Application.Features.Categories.Commands.Delete;
using Application.Features.Categories.Constants;
using Application.Features.Categories.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Persistence;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace CategoryCommandTests;

public class DeleteCategoryCommandHandlerTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IUnitOfWork<ICategoryRepository>> _unitOfWorkMock;
    private Mock<ICategoryBusinessRules> _categoryBusinessRules;
    private Mock<IMapper> _mapperMock;
    private DeleteCategoryCommand.DeleteCategoryCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork<ICategoryRepository>>();
        _categoryBusinessRules = new Mock<ICategoryBusinessRules>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteCategoryCommand.DeleteCategoryCommandHandler(_categoryRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _categoryBusinessRules.Object);
    }

    [Test]
    public async Task Handle_WithValidRequest_ShouldReturnDeletedSupplierCommandResponse()
    {
        //Arrange
        var request = new DeleteCategoryCommand { Id = 1 };
        var category = new Category { Id = 1, Name = "Category Name" };
        var response = new DeletedCategoryCommandResponse { Id = 1 };

        _categoryRepositoryMock.Setup(repo =>
            repo.GetAsync(p => p.Id == request.Id,
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(category);

        _categoryBusinessRules.Setup(rules => rules.CategoryShouldExistsWhenSelected(category)).Returns(Task.CompletedTask);
        _categoryRepositoryMock.Setup(repo => repo.DeleteAsync(category, false));
        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mapperMock.Setup(mapper => mapper.Map<DeletedCategoryCommandResponse>(category)).Returns(response);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        _categoryRepositoryMock.Verify(repo =>
            repo.GetAsync(p => p.Id == request.Id,
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);
        _categoryBusinessRules.Verify(rules => rules.CategoryShouldExistsWhenSelected(category), Times.Once);
        _categoryRepositoryMock.Verify(repo => repo.DeleteAsync(category, false), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<DeletedCategoryCommandResponse>(category), Times.Once);
        result.Should().BeOfType<DeletedCategoryCommandResponse>();
    }

    [Test]
    public void Given_ValidId_ShouldPassValidation()
    {
        //Arrange
        var command = new DeleteCategoryCommand { Id = 1 };
        var validator = new DeleteCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public void Given_InvalidId_ShouldFailValidation()
    {
        //Arrange
        var command = new DeleteCategoryCommand { Id = 0 };
        var validator = new DeleteCategoryCommandValidator();

        //Act
        var result = validator.Validate(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == CategoryValidationErrorMesages.CategoryIdMustBeGreaterThanZero);
    }
}
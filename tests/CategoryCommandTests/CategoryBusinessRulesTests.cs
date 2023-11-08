using System.Linq.Expressions;
using Application.Features.Categories.Constants;
using Application.Features.Categories.Rules;
using Application.Repositories;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace CategoryCommandTests;

public class CategoryBusinessRulesTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private CategoryBusinessRules _categoryBusinessRules;

    [SetUp]
    public void Setup()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryBusinessRules = new CategoryBusinessRules(_categoryRepositoryMock.Object);
    }

    [Test]
    public async Task CategoryNameCanNotBeDuplicatedWhenInserted_WithUniqueName_ShouldNotThrowException()
    {
        //Arrange
        string uniqueName = "Unique Category Name";
        _categoryRepositoryMock.Setup(repo => repo.GetAsync(
                       It.IsAny<Expression<Func<Category, bool>>>(),
                                  null,
                                  It.IsAny<bool>(),
                                  It.IsAny<bool>(),
                                  It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null);

        //Act
        Func<Task> act = async () => await _categoryBusinessRules.CategoryNameCanNotBeDuplicatedWhenInserted(uniqueName);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task CategoryNameCanNotBeDuplicatedWhenInserted_WithDuplicatedName_ShouldThrowException()
    {
        //Arrange
        string duplicatedName = "Duplicated Category Name";
        _categoryRepositoryMock.Setup(repo => repo.GetAsync(
                                  It.IsAny<Expression<Func<Category, bool>>>(),
                                                                   null,
                                                                   It.IsAny<bool>(),
                                                                   It.IsAny<bool>(),
                                                                   It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = 1, Name = duplicatedName });

        //Act
        Func<Task> act = async () => await _categoryBusinessRules.CategoryNameCanNotBeDuplicatedWhenInserted(duplicatedName);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(CategoryBusinessErrorMessages.CategoryNameAlreadyExists);
    }

    [Test]
    public async Task CategoryShouldExistsWhenSelected_WithExistingCategory_ShouldNotThrowException()
    {
        //Arrange
        var category = new Category { Id = 1, Name = "Existing Category" };
        _categoryRepositoryMock.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                null,
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        //Act
        Func<Task> act = async () => await _categoryBusinessRules.CategoryShouldExistsWhenSelected(category);

        //Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task CategoryShouldExistsWhenSelected_WithNullCategory_ShouldThrowException()
    {
        //Arrange
        Category category = null;

        //Act
        Func<Task> act = async () => await _categoryBusinessRules.CategoryShouldExistsWhenSelected(category);

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage(CategoryBusinessErrorMessages.CategoryCanNotBeFound);
    }
}
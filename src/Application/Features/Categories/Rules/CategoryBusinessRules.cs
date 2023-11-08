using Application.Features.Categories.Constants;
using Application.Repositories;
using Core.Application.Rules;
using Domain.Entities;

namespace Application.Features.Categories.Rules;

public class CategoryBusinessRules : BaseBusinessRules, ICategoryBusinessRules
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryBusinessRules(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task CategoryShouldExistsWhenSelected(Category? category)
    {
        if (category == null)
            throw new Exception(CategoryBusinessErrorMessages.CategoryCanNotBeFound);
        return Task.CompletedTask;
    }

    public async Task CategoryNameCanNotBeDuplicatedWhenInserted(string name)
    {
        Category? category = await _categoryRepository.GetAsync(p => p.Name == name);
        if (category != null) throw new Exception(CategoryBusinessErrorMessages.CategoryNameAlreadyExists);
        return;
    }

}
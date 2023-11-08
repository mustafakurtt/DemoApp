using Domain.Entities;

namespace Application.Features.Categories.Rules;

public interface ICategoryBusinessRules
{
    public Task CategoryShouldExistsWhenSelected(Category? category);
    public Task CategoryNameCanNotBeDuplicatedWhenInserted(string name);
}
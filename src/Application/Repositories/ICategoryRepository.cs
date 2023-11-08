using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Repositories;

public interface ICategoryRepository : IAsyncRepository<Category, int>
{

}
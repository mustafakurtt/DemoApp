using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class CategoryRepository : EfRepositoryBase<Category, int, MysqlDbContext>, ICategoryRepository
{
    public CategoryRepository(MysqlDbContext context) : base(context)
    {
    }
}
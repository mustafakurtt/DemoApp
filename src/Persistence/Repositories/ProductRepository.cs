using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ProductRepository : EfRepositoryBase<Product, int, MysqlDbContext>, IProductRepository
{
    public ProductRepository(MysqlDbContext context) : base(context)
    {
    }
}
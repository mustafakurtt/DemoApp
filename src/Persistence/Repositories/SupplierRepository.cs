using Application.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class SupplierRepository : EfRepositoryBase<Supplier, int, MysqlDbContext>, ISupplierRepository
{
    public SupplierRepository(MysqlDbContext context) : base(context)
    {
    }
}
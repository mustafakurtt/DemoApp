using Core.Persistence.Repositories;
using Domain.Entities;

namespace Application.Repositories;

public interface ISupplierRepository : IAsyncRepository<Supplier, int>
{

}
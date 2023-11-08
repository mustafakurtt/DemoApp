using Core.Persistence.Repositories;

namespace Core.Persistence;

public interface IUnitOfWork<T> where T : IRepository
{
    public Task SaveChangesAsync();
}
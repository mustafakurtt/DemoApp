using Core.Persistence.Repositories;

namespace Core.Persistence;

public class UnitOfWork<TRepository> : IUnitOfWork<TRepository> where TRepository : IRepository
{
    private readonly TRepository _repository;

    public UnitOfWork(TRepository repository)
    {
        _repository = repository;
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
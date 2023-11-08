namespace Core.Persistence.Repositories;

public interface IRepository
{
    Task SaveChangesAsync();
}
using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Category : Entity<int>
{
    public string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; } = null!;

    public Category()
    {
    }

    public Category(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }
}
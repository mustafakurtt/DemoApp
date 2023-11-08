using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Supplier : Entity<int>
{
    public string Name { get; set; }
    public HashSet<Product> Products { get; set; } // Navigation property

    public Supplier()
    {
        Products = new HashSet<Product>();
    }
}
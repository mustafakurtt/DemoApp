using Core.Persistence.Repositories;

namespace Domain.Entities;

public class Product : Entity<int>
{
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public virtual Category Category { get; set; }
    public virtual Supplier Supplier { get; set; }

    public Product()
    {

    }

    public Product(string name, decimal price, int categoryId, int supplierId)
    {
        Name = name;
        Price = price;
        CategoryId = categoryId;
        SupplierId = supplierId;
    }
}
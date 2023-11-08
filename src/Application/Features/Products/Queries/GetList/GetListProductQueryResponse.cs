namespace Application.Features.Products.Queries.GetList;

public class GetListProductQueryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }
    public string SupplierName { get; set; }

}
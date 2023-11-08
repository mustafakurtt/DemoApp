namespace Application.Features.Suppliers.Commands.Create;

public class CreatedSupplierCommandResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
}
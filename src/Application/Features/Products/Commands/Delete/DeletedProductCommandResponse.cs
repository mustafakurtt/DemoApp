namespace Application.Features.Products.Commands.Delete;

public class DeletedProductCommandResponse
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime DeletedDate { get; set; }
}
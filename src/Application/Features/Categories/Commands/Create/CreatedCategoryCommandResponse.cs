namespace Application.Features.Categories.Commands.Create;

public class CreatedCategoryCommandResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
}
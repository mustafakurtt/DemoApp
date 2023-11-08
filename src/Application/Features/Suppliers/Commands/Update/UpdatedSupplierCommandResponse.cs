﻿namespace Application.Features.Suppliers.Commands.Update;

public class UpdatedSupplierCommandResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
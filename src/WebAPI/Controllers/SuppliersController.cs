using Application.Features.Suppliers.Commands.Create;
using Application.Features.Suppliers.Commands.Delete;
using Application.Features.Suppliers.Commands.Update;
using Application.Features.Suppliers.Queries.GetById;
using Application.Features.Suppliers.Queries.GetList;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuppliersController : BaseController
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdSupplierQuery getByIdSupplierQuery)
    {
        GetByIdSupplierQueryResponse response = await Mediator.Send(getByIdSupplierQuery);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        List<GetListSupplierQueryResponse> responses = await Mediator.Send(new GetListSupplierQuery());
        return Ok(responses);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateSupplierCommand supplierCommand)
    {
        CreatedSupplierCommandResponse response = await Mediator.Send(supplierCommand);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateSupplierCommand supplierCommand)
    {
        UpdatedSupplierCommandResponse response = await Mediator.Send(supplierCommand);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteSupplierCommand supplierCommand)
    {
        DeletedSupplierCommandResponse response = await Mediator.Send(supplierCommand);
        return Ok(response);
    }
}
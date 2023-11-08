using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Delete;
using Application.Features.Products.Commands.Update;
using Application.Features.Products.Queries.GetById;
using Application.Features.Products.Queries.GetList;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : BaseController
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdProductQuery getByIdProductQuery)
    {
        GetByIdProductQueryResponse response = await Mediator.Send(getByIdProductQuery);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        List<GetListProductQueryResponse> responses = await Mediator.Send(new GetListProductQuery());
        return Ok(responses);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateProductCommand productCommand)
    {
        CreatedProductCommandResponse response = await Mediator.Send(productCommand);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand productCommand)
    {
        UpdatedProductCommandResponse response = await Mediator.Send(productCommand);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteProductCommand productCommand)
    {
        DeletedProductCommandResponse response = await Mediator.Send(productCommand);
        return Ok(response);
    }
}
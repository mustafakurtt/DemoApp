using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Commands.Delete;
using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Queries.GetById;
using Application.Features.Categories.Queries.GetList;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CategoriesController : BaseController
{
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdCategoryQuery getByIdCategoryQuery)
    {
        GetByIdCategoryQueryResponse response = await Mediator.Send(getByIdCategoryQuery);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        List<GetListCategoryQueryResponse> responses = await Mediator.Send(new GetListCategoryQuery());
        return Ok(responses);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCategoryCommand categoryCommand)
    {
        CreatedCategoryCommandResponse response = await Mediator.Send(categoryCommand);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand categoryCommand)
    {
        UpdatedCategoryCommandResponse response = await Mediator.Send(categoryCommand);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteCategoryCommand categoryCommand)
    {
        DeletedCategoryCommandResponse response = await Mediator.Send(categoryCommand);
        return Ok(response);
    }

}
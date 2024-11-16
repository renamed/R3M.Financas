using Microsoft.AspNetCore.Mvc;
using R3M.Financas.Shared.Dtos;

namespace R3M.Financas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Category : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListAsync()
    {
        await Task.Delay(100);
        return Ok();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAncestorsAsync(int id)
    {
        await Task.Delay(100);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync(CategoryRequest request)
    {
        await Task.Delay(100);
        return Ok();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutAsync(int id, CategoryRequest request)
    {
        await Task.Delay(100);
        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServerResponse<CategoryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await Task.Delay(100);
        return Ok();
    }

}

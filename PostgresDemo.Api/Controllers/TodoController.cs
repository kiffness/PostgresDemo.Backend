using Microsoft.AspNetCore.Mvc;
using PostgresDemo.Api.Dtos;
using PostgresDemo.Api.Mappers;
using PostgresDemo.Database.Data;
using PostgresDemo.Library.Commands;
using PostgresDemo.Library.Interfaces;

namespace PostgresDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController(ITodoService todoService) : ControllerBase
{
    // Get api/todo
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await todoService.GetAllAsync(ct);
        return result.ToActionResult(this);
    }
    
    // Get api/todo/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var result = await todoService.GetAsync(id, ct);
        return result.ToActionResult(this);
    }
    
    // Post api/todo
    public async Task<IActionResult> Post([FromBody] CreateTodoRequest request, CancellationToken ct)
    {
        var command = request.MapToCommand();
        var result = await todoService.CreateAsync(command, ct);
        return result.ToActionResult(this);
    }
    
    // Put api/todo/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTodoRequest request, CancellationToken ct)
    {
        var command = request.MapToCommand(id);
        var result = await todoService.UpdateAsync(command, ct);
        return result.ToActionResult(this);
    }
    
    // Delete api/todo/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await todoService.DeleteAsync(id, ct);
        return result.ToActionResult(this);
    }
}
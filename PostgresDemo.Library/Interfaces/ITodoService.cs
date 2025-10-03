using PostgresDemo.Library.Commands;
using PostgresDemo.Library.Models;

namespace PostgresDemo.Library.Interfaces;

public interface ITodoService
{
    Task<Result<TodoItem>> GetAsync(int id, CancellationToken ct);
    Task<Result<TodoItem>> CreateAsync(CreateTodoCommand command, CancellationToken ct);
    Task<Result<TodoItem>> UpdateAsync(UpdateTodoCommand command, CancellationToken ct);
    Task<Result<bool>>  DeleteAsync(int id, CancellationToken ct);
 }
using Microsoft.EntityFrameworkCore;
using PostgresDemo.Database.Data;
using PostgresDemo.Library.Commands;
using PostgresDemo.Library.Enums;
using PostgresDemo.Library.Interfaces;
using PostgresDemo.Library.Models;
using PostgresDemo.Library.Records;

namespace PostgresDemo.Database.Services;

public class EfTodoService(AppDbContext db) : ITodoService
{
    public async Task<Result<List<TodoItem>>> GetAllAsync(CancellationToken ct)
    {
        var todos = await db.TodoItems.ToListAsync();
        
        return Result<List<TodoItem>>.Ok(todos);
    }

    public async Task<Result<TodoItem>> GetAsync(int id, CancellationToken ct)
    {
        var todo = await db.TodoItems.FindAsync(new object[] { id }, ct);

        if (todo == null)
            return Result<TodoItem>.Fail(new Error("Todo not found", ErrorType.NotFound));

        return Result<TodoItem>.Ok(todo);
    }

    public async Task<Result<TodoItem>> CreateAsync(CreateTodoCommand command, CancellationToken ct)
    {
        if (command.Title == "error") // special test trigger
            return Result<TodoItem>.Fail(new Error("Forced error", ErrorType.Unexpected));
        
        if (string.IsNullOrWhiteSpace(command.Title))
        {
            return Result<TodoItem>.Fail(new Error("Title is required", ErrorType.Validation));
        }

        var todo = new TodoItem
        {
            Title = command.Title,
            IsCompleted = command.IsCompleted
        };

        db.TodoItems.Add(todo);
        await db.SaveChangesAsync(ct);

        return Result<TodoItem>.Ok(todo);
    }

    public async Task<Result<TodoItem>> UpdateAsync(UpdateTodoCommand command, CancellationToken ct)
    {
        var todo = await db.TodoItems.FindAsync(new object[] { command.Id }, ct);

        if (todo == null)
            return Result<TodoItem>.Fail(new Error("Todo not found", ErrorType.NotFound));

        todo.Title = command.Title;
        todo.IsCompleted = command.IsCompleted;

        await db.SaveChangesAsync(ct);

        return Result<TodoItem>.Ok(todo);
    }

    public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        var todo = await db.TodoItems.FindAsync(new object[] { id }, ct);

        if (todo == null)
            return Result<bool>.Fail(new Error("Todo not found", ErrorType.NotFound));

        db.TodoItems.Remove(todo);
        await db.SaveChangesAsync(ct);

        return Result<bool>.Ok(true);
    }
}
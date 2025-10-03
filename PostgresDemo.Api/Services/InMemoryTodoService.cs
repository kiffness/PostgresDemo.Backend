using PostgresDemo.Library.Commands;
using PostgresDemo.Library.Enums;
using PostgresDemo.Library.Interfaces;
using PostgresDemo.Library.Models;
using PostgresDemo.Library.Records;

namespace PostgresDemo.Api.Services;

public class InMemoryTodoService : ITodoService
{
    private readonly Dictionary<int, TodoItem> _todos = new();
    private int _nextId = 1;

    public Task<Result<TodoItem>> GetAsync(int id, CancellationToken ct)
    {
        if (_todos.TryGetValue(id, out var item))
            return Task.FromResult(Result<TodoItem>.Ok(item));

        return Task.FromResult(Result<TodoItem>.Fail(new Error("Todo not found", ErrorType.NotFound)));
    }

    public Task<Result<TodoItem>> CreateAsync(CreateTodoCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
        {
            return Task.FromResult(Result<TodoItem>.Fail(
                new Error("Title is required", ErrorType.Validation)
            ));
        }

        var todo = new TodoItem
        {
            Id = _nextId++,
            Title = command.Title,
            IsCompleted = command.IsCompleted
        };

        _todos[todo.Id] = todo;

        return Task.FromResult(Result<TodoItem>.Ok(todo));
    }

    public Task<Result<TodoItem>> UpdateAsync(UpdateTodoCommand command, CancellationToken ct)
    {
        if (!_todos.ContainsKey(command.Id))
            return Task.FromResult(Result<TodoItem>.Fail(new Error("Todo not found", ErrorType.NotFound)));

        var todo = new TodoItem
        {
            Id = command.Id,
            Title = command.Title,
            IsCompleted = command.IsCompleted
        };

        _todos[todo.Id] = todo;

        return Task.FromResult(Result<TodoItem>.Ok(todo));
    }

    public Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
    {
        if (!_todos.Remove(id))
            return Task.FromResult(Result<bool>.Fail(new Error("Todo not found", ErrorType.NotFound)));

        return Task.FromResult(Result<bool>.Ok(true));
    }
}
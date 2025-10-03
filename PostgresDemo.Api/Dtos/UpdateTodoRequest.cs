namespace PostgresDemo.Api.Dtos;

public record UpdateTodoRequest(string Title, bool IsCompleted);
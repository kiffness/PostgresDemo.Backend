namespace PostgresDemo.Api.Dtos;

public record CreateTodoRequest(string Title, bool IsCompleted);
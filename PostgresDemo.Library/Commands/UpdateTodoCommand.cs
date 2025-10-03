namespace PostgresDemo.Library.Commands;

public record UpdateTodoCommand(int Id, string Title, bool IsCompleted);
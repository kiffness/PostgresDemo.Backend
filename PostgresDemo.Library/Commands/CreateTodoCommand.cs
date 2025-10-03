namespace PostgresDemo.Library.Commands;

public record CreateTodoCommand(string Title, bool IsCompleted);
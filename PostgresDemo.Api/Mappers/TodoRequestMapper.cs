using PostgresDemo.Api.Dtos;
using PostgresDemo.Library.Commands;

namespace PostgresDemo.Api.Mappers;

public static class TodoRequestMapper
{
    public static CreateTodoCommand MapToCommand(this CreateTodoRequest request)
        => new(request.Title, request.IsCompleted);
    
    public static UpdateTodoCommand MapToCommand(this UpdateTodoRequest request, int id)
        => new(id, request.Title, request.IsCompleted );
}
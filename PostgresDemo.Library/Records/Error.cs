using PostgresDemo.Library.Enums;

namespace PostgresDemo.Library.Records;

public record Error(string Description, ErrorType Type);
using Microsoft.AspNetCore.Mvc;
using PostgresDemo.Library.Enums;
using PostgresDemo.Library.Models;

namespace PostgresDemo.Api.Mappers;

public static class ErrorResultMapper
{
    // Define error severity for status code selection (higher index = more severe)
    private static readonly ErrorType[] SeverityPriority = new[]
    {
        ErrorType.Validation,
        ErrorType.Failure,
        ErrorType.Conflict,
        ErrorType.NotFound,
        ErrorType.Unauthorized,
        ErrorType.Forbidden,
        ErrorType.Unexpected
    };

    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        return result.Match<IActionResult>(
            value => controller.Ok(value),
            errors =>
            {
                // Pick the "most severe" error according to SeverityPriority
                var first = errors
                    .OrderByDescending(e => Array.IndexOf(SeverityPriority, e.Type))
                    .First();

                // Map to HTTP status code
                var statusCode = first.Type switch
                {
                    ErrorType.Validation   => 400,
                    ErrorType.Conflict     => 409,
                    ErrorType.NotFound     => 404,
                    ErrorType.Unauthorized => 401,
                    ErrorType.Forbidden    => 403,
                    ErrorType.Failure      => 400,
                    ErrorType.Unexpected   => 500,
                    _ => 500
                };

                // Build ProblemDetails
                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = first.Type.ToString(),
                    Detail = first.Description,
                    Extensions =
                    {
                        ["errors"] = errors.Select(e => new { e.Type, e.Description })
                    }
                };

                return controller.StatusCode(statusCode, problem);
            }
        );
    }
}
using PostgresDemo.Library.Records;

namespace PostgresDemo.Library.Models;

public class Result<T>
{
    public T? Value { get; }
    public List<Error>? Errors { get; }
    public bool IsSuccess => Errors == null || Errors.Count == 0;

    private Result(T? value, List<Error>? errors)
    {
        Value = value;
        Errors = errors;
    }

    public static Result<T> Ok(T value) => new(value, null);

    public static Result<T> Fail(params Error[] errors) => new(default, errors.ToList());

    // Generic functional match
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<List<Error>, TResult> onFailure)
        => IsSuccess ? onSuccess(Value!) : onFailure(Errors!);
}
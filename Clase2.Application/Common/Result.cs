namespace Clase2.Application.Common;

public sealed class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public IReadOnlyList<ValidationFailureItem> Errors { get; }

    private Result(bool b, T? value, IReadOnlyList<ValidationFailureItem> errors) 
        => (IsSuccess, Value, Errors) = (b, value, errors); 
    
    public static Result<T> Success(T value) => new(true, value, []);
    
    public static Result<T> Failure(IReadOnlyList<ValidationFailureItem> errors)
        => new(false, default, errors);
}
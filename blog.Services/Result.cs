using System.Diagnostics.CodeAnalysis;

namespace blog.Services;

public class Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    [MemberNotNullWhen(true, nameof(_error))]
    [MemberNotNullWhen(false, nameof(_value))]
    public bool IsErrorResult { get; }
    
    private Result(TValue value)
    {
        _value = value;
        IsErrorResult = false;
    }
    
    private Result(TError error)
    {
        _error = error;
        IsErrorResult = true;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public void Match(Action<TValue> success, Action<TError> error)
    {
        if (IsErrorResult)
        {
            error(_error);
        }
        else
        {
            success(_value);
        }
    }
}
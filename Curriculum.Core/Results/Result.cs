using System.Diagnostics.CodeAnalysis;

namespace Curriculum.Core.Results;

public class Result
{
    public virtual bool IsSuccess { get; init; } = true;    
    public virtual bool IsFailure => !IsSuccess;
    
    public BaseError? Error { get; init; }

    public Result(bool isSuccess = true, BaseError? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Success 
        => new();
    
    public static Result Failure(BaseError error) =>
        new(false, error: error);

    public static implicit operator Result(BaseError error)
        => Failure(error);
}

public sealed class Result<TValue> : Result
{
    private readonly bool _isSuccess;

    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsFailure => !_isSuccess;

    [MemberNotNullWhen(true, nameof(Value))]
    public override bool IsSuccess => _isSuccess;

    public TValue? Value { get; }

    public Result(bool isSuccess, TValue? value = default, BaseError? error = null) : base(isSuccess, error)
    {
        _isSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    new public static Result<TValue> Success(TValue value)
        => new(true, value);

    new public static Result<TValue> Failure(BaseError error)
        => new(false, error: error);

    public static implicit operator Result<TValue>(BaseError error)
        => Failure(error);

    public static implicit operator Result<TValue>(TValue value)
        => Success(value);
}


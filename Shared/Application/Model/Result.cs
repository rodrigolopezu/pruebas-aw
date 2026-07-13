namespace eb17953u202421866.Shared.Application.Model;

/// <summary>
///     Generic Result wrapper for Command/Query handlers in the Application layer.
///     Avoids throwing exceptions for expected business-rule failures; the Interfaces layer
///     (via an ActionResultAssembler) maps a failure + its error enum to the right HTTP status.
/// </summary>
/// <remarks>
///     Author: __YOUR_NAME__
/// </remarks>
/// <typeparam name="T">The type of the value returned on success.</typeparam>
public class Result<T>
{
    protected Result(bool isSuccess, T? value, string message, Enum? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Message = message;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string Message { get; }
    public Enum? Error { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty, null);
    }

    public static Result<T> Failure(Enum error, string message)
    {
        return new Result<T>(false, default, message, error);
    }
}

/// <summary>
///     Non-generic Result for handlers that don't return a value (e.g. delete operations).
/// </summary>
public class Result : Result<object>
{
    private Result(bool isSuccess, string message, Enum? error) : base(isSuccess, null, message, error)
    {
    }

    public static Result Success()
    {
        return new Result(true, string.Empty, null);
    }

    public new static Result Failure(Enum error, string message)
    {
        return new Result(false, message, error);
    }
}

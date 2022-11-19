using PaymentsTracker.Common.Extensions;

namespace PaymentsTracker.Common.DTOs;

public record OperationResult<T>
{
    public T Data { get; set; } = default!;
    public bool IsSuccess { get; set; }
    public bool IsFailed => !IsSuccess;
    public ErrorDto? Error { get; set; }

    public static OperationResult<T> WithSuccess(T input) => new() { Data = input, IsSuccess = true };

    public static OperationResult<T> WithError(Enum @enum) => new()
    {
        IsSuccess = false, Error = new(Convert.ToInt32(@enum), @enum.ToString())
    };

    public static OperationResult<T> WithError(ErrorDto error) => new()
    {
        IsSuccess = false, Error = error
    };

    public static implicit operator OperationResult<T>(ErrorDto error) => WithError(error);
    public static implicit operator OperationResult<T>(T input) => WithSuccess(input);
}

public record ErrorDto
{
    public int Code { get; set; }
    public string Message { get; set; } = null!;

    public ErrorDto(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public static ErrorDto Factory(int code, string message) =>
        new(code, message);

    public static ErrorDto Factory(Enum @enum) => new(Convert.ToInt32(@enum), @enum.GetDescription());
}
using Domain.Enums;
using Presentation.ViewModels.ValidationVMs;

namespace Presentation.ViewModels.ResponseVMs;

public class ResponseVM<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public ErrorCode errorCode { get; set; }
    public string Message { get; set; }

    public IEnumerable<ValidationErrorVM>? Errors { get; set; } 
    public static ResponseVM<T> Success(T data, string message = "Completed Successfully")
    {
        return new ResponseVM<T>
        {
            Data = data,
            IsSuccess = true,
            errorCode = ErrorCode.NoError,
            Message = message
        };
    }

    public static ResponseVM<T> Fail(ErrorCode errorCode, string message, IEnumerable<ValidationErrorVM>? errors = null)
    {
        return new ResponseVM<T>
        {
            Data = default,
            IsSuccess = false,
            errorCode = errorCode,
            Message = message,
            Errors = errors
        };
    }
}


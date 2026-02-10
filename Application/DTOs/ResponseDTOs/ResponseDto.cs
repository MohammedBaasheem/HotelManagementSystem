using Application.DTOs.ValidationDTOs;
using Domain.Enums;

namespace Application.DTOs.ResponseDTOs;

public class ResponseDto<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public ErrorCode errorCode { get; set; }
    public string Message { get; set; }

    public IEnumerable<ValidationErrorDto>? Errors { get; set; } 
    public static ResponseDto<T> Success(T data, string message = "Completed Successfully")
    {
        return new ResponseDto<T>
        {
            Data = data,
            IsSuccess = true,
            errorCode = ErrorCode.NoError,
            Message = message
        };
    }

    public static ResponseDto<T> Fail(ErrorCode errorCode, string message, IEnumerable<ValidationErrorDto>? errors = null)
    {
        return new ResponseDto<T>
        {
            Data = default,
            IsSuccess = false,
            errorCode = errorCode,
            Message = message,
            Errors = errors
        };
    }
}


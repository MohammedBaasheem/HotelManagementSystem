using Application.DTOs.ValidationDTOs;

namespace Application.Interface;

public interface IValidationService
{
    Task<List<ValidationErrorDto>> ValidateAsync<T>(T model);
}

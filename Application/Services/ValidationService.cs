using Application.DTOs.ValidationDTOs;
using Application.Interface;
using Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class ValidationService : IValidationService
    {
        protected IServiceProvider _serviceProvider;
        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<List<ValidationErrorDto>> ValidateAsync<T>(T model)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator == null)
                return new List<ValidationErrorDto>();

            var ValidationResult = await validator.ValidateAsync(model);

            if (ValidationResult.IsValid)
                return new List<ValidationErrorDto>();

            var CustomError = ValidationResult.Errors.Select(e => new ValidationErrorDto
            {
                Message = e.ErrorMessage,
                Code = e.CustomState is ErrorCode code ? (int)code : (int)ErrorCode.ValidationError
            }).ToList();

            return CustomError;
        }
    }
}

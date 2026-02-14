using Application.DTOs.ValidationDTOs;
using AutoMapper;

namespace Presentation.ViewModels.ValidationVMs
{
    public class ValidationProfile : Profile
    {
        public ValidationProfile()
        {
            CreateMap<ValidationErrorDto, ValidationErrorVM>();
        }
    }
}

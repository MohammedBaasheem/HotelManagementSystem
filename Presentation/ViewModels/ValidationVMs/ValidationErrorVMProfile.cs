using Application.DTOs.ValidationDTOs;
using AutoMapper;

namespace Presentation.ViewModels.ValidationVMs
{
    public class ValidationErrorVMProfile:Profile
    {
        public ValidationErrorVMProfile()
        {
            CreateMap<ValidationErrorDto, ValidationErrorVM>();
        }
        
    }
}

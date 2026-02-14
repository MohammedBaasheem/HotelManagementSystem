using Application.DTOs.AuthDTOs;
using AutoMapper;

namespace Presentation.ViewModels.AuthVMs
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginVM, LoginDto>();
            CreateMap<LoginResponseDto, LoginResponseVM>();
            CreateMap<RegisterVM, RegisterDto>();
            CreateMap<CreateStaffUserVM, CreateStaffUserDto>();
        }
    }
}

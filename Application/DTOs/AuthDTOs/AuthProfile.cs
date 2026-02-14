using AutoMapper;
using Domain.Models;

namespace Application.DTOs.AuthDTOs
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<CreateStaffUserDto, User>();
            
        }
    }
}

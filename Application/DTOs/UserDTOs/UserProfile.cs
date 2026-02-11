using AutoMapper;
using Domain.Models;

namespace Application.DTOs.UserDTOs
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRole.Select(ur => ur.Role.Name)));
            CreateMap<User, UserWithRolesDto>()
                .ForMember(d => d.RoleIds, opt => opt.MapFrom(s => s.UserRole.Select(ur => ur.RoleId)));

            CreateMap<UpdateUserDto, User>();

        }
    }
}

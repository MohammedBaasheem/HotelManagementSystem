using Application.DTOs.UserDTOs;
using AutoMapper;

namespace Presentation.ViewModels.UserVMs
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserVM>();
            CreateMap<UserWithRolesDto, UserWithRolesVM>();
            CreateMap<UpdateUserVM, UpdateUserDto>();
            CreateMap<AssignRoleVM, AssignRoleDto>();

        }
    }
}

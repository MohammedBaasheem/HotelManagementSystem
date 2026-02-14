using Application.DTOs.RoleDTOs;
using AutoMapper;

namespace Presentation.ViewModels.RoleVMs
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<GetRoleDto, GetRoleVM>();
            CreateMap<CreateRoleVM, CreateRoleDto>();

        }
    }
}

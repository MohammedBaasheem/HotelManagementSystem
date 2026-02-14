using AutoMapper;
using Domain.Models;

namespace Application.DTOs.RoleDTOs
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, GetRoleDto>();
            CreateMap<CreateRoleDto, Role>();

        }
    }
}

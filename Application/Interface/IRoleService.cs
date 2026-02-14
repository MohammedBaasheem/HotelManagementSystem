using Application.DTOs.ResponseDTOs;
using Application.DTOs.RoleDTOs;
using Domain.Enums;

namespace Application.Interface
{
    public interface IRoleService
    {
        Task<ResponseDto<IEnumerable<GetRoleDto>>> GetAllRolesAsync();

        Task<ResponseDto<bool>> AddRole(CreateRoleDto dto, Guid actionBy);

        Task<ResponseDto<bool>> DeleteRole(Guid roleId, Guid actionBy);

        Task<ResponseDto<bool>> AssignFeatureToRoleAsync(Guid roleId, Feature feature, Guid actionBy);

        Task<ResponseDto<bool>> UnassignFeatureFromRoleAsync(Guid roleId, Feature feature, Guid actionBy);

        Task<bool> HasPermissionAsync(IEnumerable<Guid> roleIds, Feature feature);

        Task<Guid> GetRoleIdByNameAsync(string roleName);
    }
}

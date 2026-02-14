using Application.DTOs.AuthDTOs;
using Application.DTOs.ResponseDTOs;
using Application.DTOs.UserDTOs;

namespace Application.Interface
{
    public interface IUserManagementService
    {
        Task<ResponseDto<bool>> RegisterAsync(RegisterDto requestDto);
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto requestDto);
        Task<ResponseDto<bool>> AssignRoleToUserAsync(Guid userId, Guid roleId, Guid actionBy);
        Task<ResponseDto<bool>> UnassignRoleFromUserAsync(Guid userId, Guid roleId, Guid actionBy);
        Task<ResponseDto<bool>> DeleteUser(Guid userId, Guid actionBy);
        Task<ResponseDto<UserDto>> GetUserProfileAsync(Guid userId);
        Task<ResponseDto<bool>> CreateStaffUserAsync(CreateStaffUserDto RequestDTO, Guid actionBy);
    }
}

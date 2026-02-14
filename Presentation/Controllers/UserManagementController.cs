using Application.DTOs.AuthDTOs;
using Application.Interface;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Presentation.ViewModels.AuthVMs;
using Presentation.ViewModels.ResponseVMs;
using Presentation.ViewModels.UserVMs;
using System.Security.Claims;

namespace Presentation.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userService;
        private readonly IMapper _mapper;

        private Guid CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userIdClaim != null ? Guid.Parse(userIdClaim) : Guid.Empty;
            }
        }

        public UserManagementController(IUserManagementService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [HttpPost("register")]
        public async Task<ResponseVM<bool>> Register([FromBody] RegisterVM model)
        {
            var dto = _mapper.Map<RegisterDto>(model);
            var resultDto = await _userService.RegisterAsync(dto);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }

        [HttpPost("login")]
        public async Task<ResponseVM<LoginResponseVM>> Login([FromBody] LoginVM model)
        {
            var dto = _mapper.Map<LoginDto>(model);
            var resultDto = await _userService.LoginAsync(dto);
            var VM = _mapper.Map<ResponseVM<LoginResponseVM>>(resultDto);
            return VM;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ResponseVM<UserVM>> GetProfile()
        {
            var resultDto = await _userService.GetUserProfileAsync(CurrentUserId);
            return _mapper.Map<ResponseVM<UserVM>>(resultDto);
        }

        [HttpDelete("{userId}")]
        [HasPermission(Feature.DeleteUser)]
        public async Task<ResponseVM<bool>> DeleteUser([FromRoute] Guid userId)
        {
            var resultDto = await _userService.DeleteUser(userId, CurrentUserId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }

        [HttpPost("roles")]
        [HasPermission(Feature.AssignRoleToUser)]
        public async Task<ResponseVM<bool>> AssignRole([FromBody] AssignRoleVM model)
        {
            var resultDto = await _userService.AssignRoleToUserAsync(model.UserId, model.RoleId, CurrentUserId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }


        [HttpDelete("roles")]
        [HasPermission(Feature.UnassignRoleFromUser)]
        public async Task<ResponseVM<bool>> UnassignRole([FromBody] UnassignRoleVM model)
        {
            var resultDto = await _userService.UnassignRoleFromUserAsync(model.UserId, model.RoleId, CurrentUserId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }

        [HttpPost("staff")]
        [HasPermission(Feature.CreateStaffUser)]
        public async Task<ResponseVM<bool>> CreateStaff([FromBody] CreateStaffUserVM model)
        {
            var dto = _mapper.Map<CreateStaffUserDto>(model);
            var resultDto = await _userService.CreateStaffUserAsync(dto, CurrentUserId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }
    }
}

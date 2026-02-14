using Application.DTOs.RoleDTOs;
using Application.Interface;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Presentation.ViewModels.ResponseVMs;
using Presentation.ViewModels.RoleVMs;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private Guid CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userIdClaim != null ? Guid.Parse(userIdClaim) : Guid.Empty;
            }
        }

        public RolesController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        [HasPermission(Feature.GetAllRoles)]
        public async Task<ResponseVM<IEnumerable<GetRoleVM>>> GetAll()
        {
            var resultDto = await _roleService.GetAllRolesAsync();
            return _mapper.Map<ResponseVM<IEnumerable<GetRoleVM>>>(resultDto);
        }

        [HttpPost]
        [HasPermission(Feature.CreateRole)]
        public async Task<ResponseVM<bool>> Create([FromBody] CreateRoleVM model)
        {
            var dto = _mapper.Map<CreateRoleDto>(model);
            var resultDto = await _roleService.AddRole(dto, CurrentUserId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }

        [HttpDelete("{roleId}")]
        [HasPermission(Feature.DeleteRole)]
        public async Task<ResponseVM<bool>> Delete([FromRoute] Guid roleId)
        {
            var resultDto = await _roleService.DeleteRole(roleId, CurrentUserId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }


        [HttpPost("features/assign")]
        [HasPermission(Feature.AssignFeatureToRole)]
        public async Task<ResponseVM<bool>> AssignFeature([FromBody] AssignFeatureVM model)
        {
            var resultDto = await _roleService.AssignFeatureToRoleAsync(model.RoleId, model.Feature, CurrentUserId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }

        [HttpDelete("features/Unassign")]
        [HasPermission(Feature.UnassignFeatureFromRole)]
        public async Task<ResponseVM<bool>> UnassignFeature([FromBody] UnassignFeatureVM model)
        {
            var resultDto = await _roleService.UnassignFeatureFromRoleAsync(model.RoleId, model.Feature, CurrentUserId);
            return _mapper.Map<ResponseVM<bool>>(resultDto);
        }

    }
}

using Application.DTOs.ResponseDTOs;
using Application.DTOs.RoleDTOs;
using Application.Interface;
using Application.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        protected readonly IRoleRepository _roleRepo;
        protected readonly IMapper _mapper;
        protected readonly IGeneralRepository<RoleFeature> _roleFeatureRepo;
        protected readonly IUserRepository _userRepo;

        public RoleService(IRoleRepository roleRepo, IMapper mapper, IGeneralRepository<RoleFeature> roleFeatureRepo, IUserRepository userRepo)
        {
            _roleRepo = roleRepo;
            _mapper = mapper;
            _roleFeatureRepo = roleFeatureRepo;
            _userRepo = userRepo;
        }

        public async Task<ResponseDto<IEnumerable<GetRoleDto>>> GetAllRolesAsync()
        {
            var roles = await _roleRepo.GetAll()
                               .ProjectTo<GetRoleDto>(_mapper.ConfigurationProvider)
                               .ToListAsync();

            return ResponseDto<IEnumerable<GetRoleDto>>.Success(roles);
        }
        public async Task<ResponseDto<bool>> AddRole(CreateRoleDto Dto, Guid ActionBy)
        {
            var Exist = await _roleRepo.IsExistAsync(r => r.Name == Dto.Name.ToUpperInvariant());
            if (Exist)
                return ResponseDto<bool>.Fail(ErrorCode.RoleAlreadyExists, $"Role: '{Dto.Name}' Already Exists");

            var role = _mapper.Map<Role>(Dto);
            role.CreatedBy = ActionBy;

            await _roleRepo.AddAsync(role);
            await _roleRepo.SaveChanges();

            return ResponseDto<bool>.Success(true);
        }
        public async Task<ResponseDto<bool>> DeleteRole(Guid roleId, Guid ActionBy)
        {
            var Role = await _roleRepo.GetWithTracking(x =>  x.Id == roleId);
            if (Role == null)
                return ResponseDto<bool>.Fail(ErrorCode.RoleNotFound, $"Role Not Found");

            Role.IsDeleted = true;
            Role.UpdatedAt = DateTime.UtcNow;
            Role.UpdatedBy = ActionBy;

            await _roleRepo.SaveChanges();
            return ResponseDto<bool>.Success(true);
        }
        public async Task<ResponseDto<bool>> AssignFeatureToRoleAsync(Guid roleId, Feature feature, Guid ActionBy)
        {
            var roleExist = await _roleRepo.IsExistAsync(x => x.Id == roleId);

            if (!roleExist)
                return ResponseDto<bool>.Fail(ErrorCode.RoleNotFound, "Role Not Found");

            var Exists = await _roleFeatureRepo.IsExistAsync(rf => rf.RoleId == roleId && rf.Feature == feature);
            if(Exists)
                return ResponseDto<bool>.Fail(ErrorCode.DuplicateEntry, "Feature already assigned to this role");

            var roleFeature = new RoleFeature
            {
                RoleId = roleId,
                Feature = feature,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = ActionBy
            };

            await _roleFeatureRepo.AddAsync(roleFeature);
            await _roleFeatureRepo.SaveChanges();

            return ResponseDto<bool>.Success(true);
        }
        public async Task<ResponseDto<bool>> UnassignFeatureFromRoleAsync(Guid roleId, Feature feature, Guid ActionBy)
        {
            var roleFeature = await _roleFeatureRepo.GetWithTracking(rf => rf.RoleId == roleId && rf.Feature == feature);

            if (roleFeature == null)
                return ResponseDto<bool>.Fail(ErrorCode.RoleFeatureNotFound, "Feature is not assigned to this role");

            roleFeature.IsDeleted = true;
            roleFeature.UpdatedBy = ActionBy;
            roleFeature.UpdatedAt = DateTime.UtcNow;

            await _roleFeatureRepo.SaveChanges();
            return ResponseDto<bool>.Success(true);
        }
        public async Task<bool> HasPermissionAsync(IEnumerable<Guid> roleIds, Feature feature)
        {
            if (roleIds == null)
                return false;

            return await _roleFeatureRepo.IsExistAsync(rf => roleIds.Contains(rf.RoleId) && rf.Feature == feature);
        }
        public async Task<Guid> GetRoleIdByNameAsync(string roleName)
        {
            return await _roleRepo.GetAll().Where(x => x.Name == roleName.ToUpperInvariant())
                                           .Select(x => x.Id).FirstOrDefaultAsync();
        }

    }
}

using Application.DTOs.ResponseDTOs;
using Application.DTOs.RoleDTOs;
using Application.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class RoleService
    {
        protected readonly IRoleRepository _roleRepo;
        protected readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepo, IMapper mapper)
        {
            _roleRepo = roleRepo;
            _mapper = mapper;
        }

        public async Task<ResponseDto<IEnumerable<GetRoleDto>>> GetAllRolesAsync()
        {
            var roles = await _roleRepo.GetAll()
                               .ProjectTo<GetRoleDto>(_mapper.ConfigurationProvider)
                               .ToListAsync();

            return ResponseDto<IEnumerable<GetRoleDto>>.Success(roles);
        }
        public async Task<ResponseDto<bool>> AddRole(CreateRoleDto Dto)
        {
            var Exist = await _roleRepo.IsExistAsync(r => r.Name == Dto.Name.ToUpperInvariant());
            if (Exist)
                return ResponseDto<bool>.Fail(ErrorCode.RoleAlreadyExists, $"Role: '{Dto.Name}' Already Exists");

            var role = _mapper.Map<Role>(Dto);

            await _roleRepo.AddAsync(role);
            await _roleRepo.SaveChanges();

            return ResponseDto<bool>.Success(true);
        }
        

    }
}

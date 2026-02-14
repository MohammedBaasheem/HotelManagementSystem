using Application.DTOs.AuthDTOs;
using Application.DTOs.ResponseDTOs;
using Application.DTOs.RoleDTOs;
using Application.DTOs.UserDTOs;
using Application.Helpers;
using Application.Interface;
using Application.IRepositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class UserManagmentService : IUserManagementService
    {
        protected readonly IUserRepository _UserRepo;
        protected readonly IRoleRepository _RoleRepo;
        protected readonly IGeneralRepository<UserRole> _UserRoleRepo;
        protected readonly IValidationService _ValidationService;
        protected readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;

        public UserManagmentService(IUserRepository UserRepo, IRoleRepository RoleRepo, IGeneralRepository<UserRole> userRoleRepo,IValidationService ValidationService, IMapper mapper, IOptions<JwtOptions> jwtOptions)
        {
            _UserRepo = UserRepo;
            _RoleRepo = RoleRepo;
            _UserRoleRepo = userRoleRepo;
            _ValidationService = ValidationService;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }


        public async Task<ResponseDto<bool>> RegisterAsync(RegisterDto RequestDTO)
        {
            var errors = await _ValidationService.ValidateAsync(RequestDTO);
            if (errors.Any())
            {
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "Validation Error", errors);
            }
            var Exist = await _UserRepo.IsExistAsync(u => u.Email == RequestDTO.Email.ToLower());

            if (Exist)
                return ResponseDto<bool>.Fail(ErrorCode.EmailAlreadyExists, "Email Already Exist");

            var user = _mapper.Map<User>(RequestDTO);
            user.PasswordHash = PasswordHasher.HashPassword(RequestDTO.Password);

            await _UserRepo.AddAsync(user);

            var roleId = await _RoleRepo.GetRoleIdByNameAsync(Roles.CUSTOMER.ToString());

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow,
            };

            await _UserRepo.AddUserRoleAsync(userRole);
            await _UserRepo.SaveChanges();
            return ResponseDto<bool>.Success(true, "Registered successfully");
        }

        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto RequestDTO)
        {
            var errors = await _ValidationService.ValidateAsync(RequestDTO);
            if (errors.Any())
            {
                return ResponseDto<LoginResponseDto>.Fail(ErrorCode.ValidationError, "Validation Error", errors);
            }
            var user = await _UserRepo.GetAll().Where(u => u.Email == RequestDTO.Email.ToLower())
                .ProjectTo<UserWithRolesDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (user == null || !PasswordHasher.VerifyPassword(RequestDTO.Password, user.PasswordHash))
                return ResponseDto<LoginResponseDto>.Fail(ErrorCode.InvalidCredentials, "Invalid email or password");


            var Response = new LoginResponseDto()
            {
                Token = GenerateToken(user),
                Name = user.UserName
            };

            return ResponseDto<LoginResponseDto>.Success(Response);

        }

        private string GenerateToken(UserWithRolesDto user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            foreach (var role in user.RoleIds)
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTime),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public async Task<ResponseDto<bool>> AssignRoleToUserAsync (Guid userId, Guid roleId, Guid actionBy)
        {
            var userExist = await _UserRepo.IsExistAsync(x => x.Id == userId);
            if(!userExist)
                return ResponseDto<bool>.Fail(ErrorCode.UserNotFound, "User Not Found");

            var roleExist = await _RoleRepo.IsExistAsync (x => x.Id == roleId);
            if(!roleExist)
                return ResponseDto<bool>.Fail(ErrorCode.RoleNotFound, "Role Not Found");

            var roleUserExist = await _UserRepo.IsUserInRoleAsync(userId, roleId);
            if(roleUserExist)
                return ResponseDto<bool>.Fail(ErrorCode.DuplicateEntry, "User already has this role");

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = actionBy
            };
            await _UserRoleRepo.AddAsync(userRole);

            await _UserRoleRepo.SaveChanges();
            return ResponseDto<bool>.Success(true);
        }

        public async Task<ResponseDto<bool>> UnassignRoleFromUserAsync(Guid userId, Guid roleId, Guid actionBy)
        {
            var roleUser = await _UserRoleRepo.GetWithTracking(x => x.RoleId == roleId && x.UserId == userId);
            if (roleUser == null)
                return ResponseDto<bool>.Fail(ErrorCode.UserRoleNotFound, "This role is not assigned to this user");

            roleUser.IsDeleted = true;
            roleUser.UpdatedAt = DateTime.UtcNow;
            roleUser.UpdatedBy = actionBy;

            await _UserRoleRepo.SaveChanges();
            return ResponseDto<bool>.Success(true);
        }

        public async Task<ResponseDto<bool>> DeleteUser(Guid userId, Guid actionBy)
        {
            var user = await _UserRepo.GetWithTracking(x => x.Id == userId);
            if (user == null)
                return ResponseDto<bool>.Fail(ErrorCode.UserNotFound, "User Not Found");

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = actionBy;

            await _UserRepo.SaveChanges();
            return ResponseDto<bool>.Success(true);
        }

        public async Task<ResponseDto<UserDto>> GetUserProfileAsync(Guid userId)
        {
            var user = await _UserRepo.GetAll().Where(u => u.Id == userId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (user == null)
                return ResponseDto<UserDto>.Fail(ErrorCode.UserNotFound, "User Not Found");

            return ResponseDto<UserDto>.Success(user);
        }

        public async Task<ResponseDto<bool>> CreateStaffUserAsync(CreateStaffUserDto RequestDTO, Guid actionBy)
        {
            var errors = await _ValidationService.ValidateAsync(RequestDTO);
            if (errors.Any())
            {
                return ResponseDto<bool>.Fail(ErrorCode.ValidationError, "Validation Error", errors);
            }

            var user = _mapper.Map<User>(RequestDTO);
            user.PasswordHash = PasswordHasher.HashPassword(RequestDTO.Password);
            user.CreatedBy = actionBy;

            await _UserRepo.AddAsync(user);

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = RequestDTO.RoleId,
                CreatedBy = actionBy
            };

            await _UserRepo.AddUserRoleAsync(userRole);

            await _UserRepo.SaveChanges();
            return ResponseDto<bool>.Success(true, "Staff user created successfully");
        }

    }

}
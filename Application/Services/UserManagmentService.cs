using Application.DTOs.AuthDTOs;
using Application.DTOs.ResponseDTOs;
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
    public class UserManagmentService
    {
        protected readonly IUserRepository _UserRepo;
        protected readonly IRoleRepository _RoleRepo;
        protected readonly IValidationService _ValidationService;
        protected readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;
        public UserManagmentService(IUserRepository UserRepo, IRoleRepository RoleRepo, IValidationService ValidationService, IMapper mapper, IOptions<JwtOptions> jwtOptions)
        {
            _UserRepo = UserRepo;
            _RoleRepo = RoleRepo;
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
            var Exist = await _UserRepo.IsExistAsync(u => u.Email.ToLower() == RequestDTO.Email.ToLower());
            if (Exist)
                return ResponseDto<bool>.Fail(ErrorCode.EmailAlreadyExists, "Email Already Exist");


            var user = _mapper.Map<User>(RequestDTO);

            user.Email = RequestDTO.Email.Trim().ToLower();
            user.PasswordHash = PasswordHasher.HashPassword(RequestDTO.Password);

            await _UserRepo.AddAsync(user);
           
            await AssignRoleToUserAsync(user.Id, Roles.CUSTOMER.ToString());

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

        public async Task AssignRoleToUserAsync (Guid userId, string roleName)
        {
            var roleId = await _RoleRepo.GetRoleIdByNameAsync(roleName);

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow,
            };
            await _UserRepo.AddUserRoleAsync(userRole);
        }


    }

}
using Application.DTOs.UserDTOs;
using Application.IRepositories;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators.UserValidator
{
    public class AssignRoleValidator : AbstractValidator<AssignRoleDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        public AssignRoleValidator(IUserRepository userRepo, IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;


            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role Can't be Empty").WithState(_ => ErrorCode.RoleRequired)
                      .MustAsync(BeExistingRole).WithMessage("Role Not Found").WithState((_ => ErrorCode.RoleNotFound));

            RuleFor(x => x.UserId).NotEmpty().WithMessage("User Can't be Empty").WithState(_ => ErrorCode.UserRequired)
                      .MustAsync(BeExistingUser).WithMessage("User Not Found").WithState((_ => ErrorCode.UserNotFound));

            RuleFor(dto => dto).MustAsync(BeUserNotHasRoleAsync).WithMessage("User already has this role").WithState(_ => ErrorCode.DuplicateEntry)
            .OverridePropertyName("General");
        }
        private async Task<bool> BeExistingRole(Guid roleId, CancellationToken token)
        {
            var Exist = await _roleRepo.IsExistAsync(r => r.Id == roleId);
            return Exist;
        }

        private async Task<bool> BeExistingUser(Guid userId, CancellationToken token)
        {
            var Exist = await _userRepo.IsExistAsync(r => r.Id == userId);
            return Exist;
        }
        private async Task<bool> BeUserNotHasRoleAsync(AssignRoleDto dto, CancellationToken token)
        {
            var Exist = await _userRepo.IsUserInRoleAsync(dto.UserId, dto.RoleId);
            return !Exist;
        }


    }
}

using Application.DTOs.AuthDTOs;
using Application.IRepositories;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators.AuthValidator
{
    public class CreateUserStaffValidator : AbstractValidator<CreateStaffUserDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        public CreateUserStaffValidator(IUserRepository userRepo, IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;


            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Can't be Empty").WithState(_ => ErrorCode.EmailRequired)
                        .EmailAddress().WithMessage("Invalid Email Format").WithState(_ => ErrorCode.InvalidEmailFormat)
                        .MustAsync(BeUniqueEmail).WithMessage("Email Already Exist").WithState((_ => ErrorCode.EmailAlreadyExists));

            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName Can't be Empty").WithState(_ => ErrorCode.UserNameRequired);

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number Can't be Empty").WithState(_ => ErrorCode.PhoneNumberRequired);

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password Can't be Empty").WithState(_ => ErrorCode.PasswordRequired)
                                    .MaximumLength(100).WithMessage("Password is too long").WithState(_ => ErrorCode.PasswordTooLong)
                                    .MinimumLength(8).WithMessage("Password must be at least 8 characters").WithState(_ => ErrorCode.PasswordTooShort);

            RuleFor(x => x.Country).NotEmpty().WithMessage("Country Can't be Empty").WithState(_ => ErrorCode.CountryRequired);

            RuleFor(x => x.RoleId).NotEmpty().WithMessage("Role Can't be Empty").WithState(_ => ErrorCode.RoleNotFound)
                                  .MustAsync(BeExistingRole).WithMessage("Role Not Found").WithState((_ => ErrorCode.RoleNotFound));


        }

        private async Task<bool> BeUniqueEmail(string Email, CancellationToken token)
        {
            var email = Email.Trim().ToLowerInvariant();
            var exist = await _userRepo.IsExistAsync(u => u.Email == email);
            return !exist;
        }

        private async Task<bool> BeExistingRole(Guid roleId, CancellationToken token)
        {
            var Exist = await _roleRepo.IsExistAsync(r => r.Id == roleId);
            return Exist;
        }
    }
}

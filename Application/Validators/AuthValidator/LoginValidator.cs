using Application.DTOs.AuthDTOs;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators.AuthValidator;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email Can't be Empty").WithState(_ => ErrorCode.EmailRequired)
                                .EmailAddress().WithMessage("Invalid Email Format").WithState(_ => ErrorCode.InvalidEmailFormat);

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password Can't be Empty").WithState(_ => ErrorCode.PasswordRequired);
    }
}

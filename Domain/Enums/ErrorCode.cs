
namespace Domain.Enums
{
    public enum ErrorCode
    {
        NoError = 0,



        ValidationError = 101,

        UserNameRequired = 102,
        EmailRequired = 103,
        InvalidEmailFormat = 104,
        EmailAlreadyExists = 105,
        EmailTooLong = 106,

        PasswordRequired = 107,
        PasswordTooLong = 108,
        PasswordTooShort = 109,
        PasswordMismatch = 110,
        ConfirmPasswordRequired = 111,

        PhoneNumberRequired = 112,
        CountryRequired = 113
    }
}

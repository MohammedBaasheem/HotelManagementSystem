
namespace Domain.Enums
{
    public enum ErrorCode
    {
        NoError = 0,



        ValidationError = 101,
        InvalidCredentials = 102,

        UserNameRequired = 110,
        EmailRequired = 111,
        InvalidEmailFormat = 112,
        EmailAlreadyExists = 113,
        EmailTooLong = 114,

        PasswordRequired = 120,
        PasswordTooLong = 121,
        PasswordTooShort = 122,
        PasswordMismatch = 123,
        ConfirmPasswordRequired = 124,

        PhoneNumberRequired = 130,
        CountryRequired = 131,

        RoleNotFound = 140,
        RoleAlreadyExists = 141,
        UserNotFound = 142,
        DuplicateEntry = 143,
        RoleFeatureNotFound = 144,
        UserRoleNotFound = 145,


    }
}

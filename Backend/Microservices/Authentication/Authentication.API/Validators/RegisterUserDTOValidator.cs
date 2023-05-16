using Authentication.API.DataTransferObjects;
using FluentValidation;

namespace Authentication.API.Validators;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserDTOValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.EmailAddress).EmailAddress();
        RuleFor(x => x.Password).NotNull();
    }
}
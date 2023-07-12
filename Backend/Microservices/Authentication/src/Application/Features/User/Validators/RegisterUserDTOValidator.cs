using Authentication.Application.Features.User.DataTransferObjects;
using FluentValidation;

namespace Authentication.Application.Features.User.Validators;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDTOValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.EmailAddress).EmailAddress();
        RuleFor(x => x.Password).NotNull();
    }
}
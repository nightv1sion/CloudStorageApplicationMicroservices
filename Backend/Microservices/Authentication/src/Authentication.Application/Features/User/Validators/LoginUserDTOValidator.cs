using Authentication.Application.Features.User.DataTransferObjects;
using FluentValidation;

namespace Authentication.Application.Features.User.Validators;

public class LoginUserDTOValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDTOValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
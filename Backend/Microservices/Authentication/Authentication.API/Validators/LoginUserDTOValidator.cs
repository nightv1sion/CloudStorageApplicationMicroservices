using System.Data;
using Authentication.API.DataTransferObjects;
using FluentValidation;

namespace Authentication.API.Validators;

public class LoginUserDTOValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserDTOValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
using System.Data;
using AuthenticationMicroservice.DataTransferObjects;
using FluentValidation;

namespace AuthenticationMicroservice.Validators;

public class LoginUserDTOValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserDTOValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
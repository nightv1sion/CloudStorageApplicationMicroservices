using Authentication.Application.Features.User.DataTransferObjects;
using FluentValidation;

namespace Authentication.Application.Features.User.Validators;

public class TokenDTOValidator : AbstractValidator<TokenDto>
{
    public TokenDTOValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
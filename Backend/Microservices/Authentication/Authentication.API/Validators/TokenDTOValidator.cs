using Authentication.API.DataTransferObjects;
using FluentValidation;

namespace Authentication.API.Validators;

public class TokenDTOValidator : AbstractValidator<TokenDTO>
{
    public TokenDTOValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
using AuthenticationMicroservice.DataTransferObjects;
using FluentValidation;

namespace AuthenticationMicroservice.Validators;

public class TokenDTOValidator : AbstractValidator<TokenDTO>
{
    public TokenDTOValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
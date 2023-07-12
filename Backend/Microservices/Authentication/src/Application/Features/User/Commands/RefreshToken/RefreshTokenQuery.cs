using Authentication.Application.Features.User.DataTransferObjects;
using MediatR;

namespace Authentication.Application.Features.User.Commands.RefreshToken;

public class RefreshTokenQuery : IRequest<TokenDto>
{
    public RefreshTokenQuery(
        TokenDto tokenDto)
    {
        TokenDto = tokenDto;
    }
    public TokenDto TokenDto { get; }

}
using Authentication.Application.Extensions.Services.Contracts;
using Authentication.Application.Features.User.DataTransferObjects;
using MediatR;

namespace Authentication.Application.Features.User.Commands.RefreshToken;

public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, TokenDto>
{
    private readonly IAuthenticationService _authenticationService;

    public RefreshTokenQueryHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public async Task<TokenDto> Handle(
        RefreshTokenQuery request, 
        CancellationToken cancellationToken)
    {
        var newTokenDto = await _authenticationService.GetRefreshTokenAsync(request.TokenDto);
        return newTokenDto;
    }
}
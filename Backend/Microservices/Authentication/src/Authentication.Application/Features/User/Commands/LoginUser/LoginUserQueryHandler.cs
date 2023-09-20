using Authentication.Application.Extensions.Services.Contracts;
using Authentication.Application.Features.User.DataTransferObjects;
using MediatR;

namespace Authentication.Application.Features.User.Commands.LoginUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, SuccessfulAuthenticationDto>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginUserQueryHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public async Task<SuccessfulAuthenticationDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.LoginUserAsync(request.Dto);
        return result;
    }
}
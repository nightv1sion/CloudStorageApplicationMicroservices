using Authentication.Application.Extensions.Services.Contracts;
using MediatR;

namespace Authentication.Application.Features.User.Queries.ValidateUserAccessToken;

public class ValidateUserAccessTokenQueryHandler : IRequestHandler<ValidateUserAccessTokenQuery, Guid?>
{
    private readonly IAuthenticationService _authenticationService;

    public ValidateUserAccessTokenQueryHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    public Task<Guid?> Handle(
        ValidateUserAccessTokenQuery request, 
        CancellationToken cancellationToken)
    {
        var (result, userId) = _authenticationService.ValidateUser(request.AccessToken);
        if (result)
        {
            return Task.FromResult<Guid?>(userId);
        }

        return Task.FromResult<Guid?>(null);
    }
}
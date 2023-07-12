using MediatR;

namespace Authentication.Application.Features.User.Queries.ValidateUserAccessToken;

public class ValidateUserAccessTokenQuery : IRequest<Guid?>
{
    public ValidateUserAccessTokenQuery(
        string accessToken)
    {
        AccessToken = accessToken;
    }
    public string AccessToken { get; }

}
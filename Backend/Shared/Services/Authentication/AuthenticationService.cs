using Microsoft.AspNetCore.Http;

namespace Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    public Guid GetUserIdFromHeaders(HttpContext context)
    {
        var userIdHeader = context.Request.Headers
            .FirstOrDefault(x => x.Key == "UserID").Value.FirstOrDefault();
        if (userIdHeader is null)
        {
            throw new InvalidUserIdHeaderBadRequestException();
        }
        return Guid.Parse(userIdHeader);
    }
}
using Microsoft.AspNetCore.Http;

namespace Services.Authentication;

public interface IAuthenticationService
{
    Guid GetUserIdFromHeaders(HttpContext context);
}
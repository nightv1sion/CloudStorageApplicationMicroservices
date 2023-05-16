using Middlewares.ExceptionHandling.Exceptions;

namespace Authentication.API.Exceptions.BadRequest;

public class RefreshTokenIsExpiredBadRequest : BadRequestException 
{
    public RefreshTokenIsExpiredBadRequest(string username) : base(
        $"User {username}: Refresh token is expired")
    {
        
    }
}
using Middlewares.ExceptionHandling.Exceptions;

namespace AuthenticationMicroservice.Exceptions;

public class RefreshTokenIsExpiredBadRequest : BadRequestException 
{
    public RefreshTokenIsExpiredBadRequest() : base("Refresh token is expired")
    {
        
    }
}
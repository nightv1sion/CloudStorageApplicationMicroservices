using Middlewares.ExceptionHandling.Exceptions;

namespace AuthenticationMicroservice.Exceptions;

public class InvalidRefreshTokenBadRequestException : BadRequestException
{
    public InvalidRefreshTokenBadRequestException() : base("Invalid refresh token")
    {
        
    }
}
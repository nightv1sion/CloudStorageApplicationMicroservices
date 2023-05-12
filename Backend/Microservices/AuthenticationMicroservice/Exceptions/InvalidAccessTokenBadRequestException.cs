using Middlewares.ExceptionHandling.Exceptions;

namespace AuthenticationMicroservice.Exceptions;

public class InvalidAccessTokenBadRequestException : BadRequestException
{
    public InvalidAccessTokenBadRequestException() : base("Invalid access token")
    {
        
    }
}
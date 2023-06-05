using Middlewares.ExceptionHandling.Exceptions;

namespace Services.Authentication;

public class InvalidUserIdHeaderBadRequestException : BadRequestException
{
    public InvalidUserIdHeaderBadRequestException() : base("User id header is not valid or present")
    {
    }
}
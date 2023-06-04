using Middlewares.ExceptionHandling.Exceptions;

namespace FileStorage.API.Exceptions;

public class InvalidUserIdHeaderBadRequestException : BadRequestException
{
    public InvalidUserIdHeaderBadRequestException() : base("User id header is not valid or present")
    {
    }
}
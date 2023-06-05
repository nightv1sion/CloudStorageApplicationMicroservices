using Middlewares.ExceptionHandling.Exceptions;

namespace Files.API.Exceptions;

public class InvalidFileBadRequestException : BadRequestException
{
    public InvalidFileBadRequestException() : base("There is no file in form data")
    {
    }
}
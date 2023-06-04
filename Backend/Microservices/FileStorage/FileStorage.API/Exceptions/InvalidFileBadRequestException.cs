using Middlewares.ExceptionHandling.Exceptions;

namespace FileStorage.API.Exceptions;

public class InvalidFileBadRequestException : BadRequestException
{
    public InvalidFileBadRequestException() : base("There is no file in form data")
    {
    }
}
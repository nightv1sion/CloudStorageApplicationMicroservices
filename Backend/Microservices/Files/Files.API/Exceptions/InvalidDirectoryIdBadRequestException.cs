using Middlewares.ExceptionHandling.Exceptions;

namespace Files.API.Exceptions;

public class InvalidDirectoryIdBadRequestException : BadRequestException
{
    public InvalidDirectoryIdBadRequestException(Guid id) : base($"Directory with '{id}' does not exist")
    {
    }
}
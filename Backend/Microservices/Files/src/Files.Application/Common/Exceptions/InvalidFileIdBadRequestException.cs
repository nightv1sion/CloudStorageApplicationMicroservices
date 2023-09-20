using Middlewares.ExceptionHandling.Exceptions;

namespace Files.Application.Common.Exceptions;

public class InvalidFileIdBadRequestException : BadRequestException
{
    public InvalidFileIdBadRequestException(Guid id) : base($"File with '{id}' does not exist")
    {
    }
}
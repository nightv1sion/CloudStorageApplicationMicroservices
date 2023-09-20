
using Middlewares.ExceptionHandling.Exceptions;

namespace Files.Application.Common.Exceptions;

public class InvalidDirectoryIdBadRequestException : BadRequestException
{
    public InvalidDirectoryIdBadRequestException(Guid id) : base($"Directory with '{id}' does not exist")
    {
    }
}
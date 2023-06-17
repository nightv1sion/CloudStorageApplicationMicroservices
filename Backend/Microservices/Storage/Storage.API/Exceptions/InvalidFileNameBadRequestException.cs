using Middlewares.ExceptionHandling.Exceptions;

namespace Storage.API.Exceptions;

public class InvalidFileNameBadRequestException : BadRequestException
{
    public InvalidFileNameBadRequestException(string fileName) : base($"File with name '{fileName}' does not exist") {}
}
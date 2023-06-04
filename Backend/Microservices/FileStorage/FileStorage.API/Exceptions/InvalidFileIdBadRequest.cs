using Middlewares.ExceptionHandling.Exceptions;

namespace FileStorage.API.Exceptions;

public class InvalidFileIdBadRequest : BadRequestException
{
    public InvalidFileIdBadRequest(Guid id) : base($"File with '{id}' does not exist")
    {
    }
}
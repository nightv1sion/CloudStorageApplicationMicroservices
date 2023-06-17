using Middlewares.ExceptionHandling.Exceptions;

namespace Storage.API.Exceptions;

public class DirectoryExistsBadRequestException : BadRequestException
{
    public DirectoryExistsBadRequestException(string path) : base($"Directory '{path}' already exists")
    {

    }
}
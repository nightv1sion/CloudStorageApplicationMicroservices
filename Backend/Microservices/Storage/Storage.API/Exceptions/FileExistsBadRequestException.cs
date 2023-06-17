using Middlewares.ExceptionHandling.Exceptions;

namespace Storage.API.Exceptions;

public class FileExistsBadRequestException : BadRequestException
{
    public FileExistsBadRequestException(string path) : base($"File '{path}' already exists")
    {

    }
}
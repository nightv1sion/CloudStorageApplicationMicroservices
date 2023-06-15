namespace Storage.API.Exceptions;

public class InvalidFileNameBadRequestException : Exception
{
    public InvalidFileNameBadRequestException(string fileName) : base($"File with name '{fileName}' does not exist") {}
}
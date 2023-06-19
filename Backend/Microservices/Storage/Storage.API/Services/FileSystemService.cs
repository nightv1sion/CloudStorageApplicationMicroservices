using Storage.API.Exceptions;
using Storage.API.Services.Contracts;

namespace Storage.API.Services;

public class FileSystemService : IFileSystemService
{
    private readonly ILogger<FileSystemService> _logger;

    public FileSystemService(
        ILogger<FileSystemService> logger)
    {
        _logger = logger;
        _logger.LogInformation($"File system service was called");
    }
    public async Task<byte[]> GetFileBytesAsync(string filePath)
    {
        if (CheckIfFileExists(filePath) is false)
        {
            throw new InvalidFileNameBadRequestException(filePath);
        }
        var bytes = await File.ReadAllBytesAsync(filePath);
        _logger.LogInformation($"Bytes of file {filePath} were read");
        return bytes;
    }
    public async Task SaveFileBytesAsync(byte[] fileBytes, string filePath)
    {
        if (CheckIfFileExists(filePath) is true)
        {
            throw new FileExistsBadRequestException(filePath);
        }
        _logger.LogInformation($"Bytes of {filePath} file were wrote");
        await File.WriteAllBytesAsync(filePath, fileBytes);
    }
    public void DeleteFile(string filePath)
    {
        if (CheckIfFileExists(filePath) is false)
        {
            throw new InvalidFileNameBadRequestException(filePath);
        }
        File.Delete(filePath);
    }
    public bool CheckIfFileExists(string filePath)
    {
        return File.Exists(filePath);
    }
    public bool CheckIfDirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public void CreateDirectory(string path)
    {
        if (CheckIfDirectoryExists(path) is true)
        {
            throw new DirectoryExistsBadRequestException(path);
        }
        Directory.CreateDirectory(path);
    }
}
using Storage.API.Exceptions;
using Storage.API.Services.Contracts;

namespace Storage.API.Services;

public class FileSystemService : IFileSystemService
{
    public FileSystemService()
    {
    }
    public async Task SaveFileBytesAsync(byte[] fileBytes, string filePath)
    {
        if (CheckIfFileExists(filePath) is true)
        {
            throw new FileExistsBadRequestException(filePath);
        }
        await File.WriteAllBytesAsync(filePath, fileBytes);
    }
    public async Task<byte[]> GetFileBytesAsync(string filePath)
    {
        var bytes = await File.ReadAllBytesAsync(filePath);
        return bytes;
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
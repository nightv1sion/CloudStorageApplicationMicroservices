using Storage.API.DataTransferObjects;
using Storage.API.Exceptions;
using Storage.API.Services.Contracts;

namespace Storage.API.Services;

public class StorageService : IStorageService
{
    private readonly IConfiguration _configuration;
    private readonly string _storagePath;
    private readonly IFileSystemService _fileSystemService;

    public StorageService(
        IConfiguration configuration, 
        IFileSystemService fileSystemService)
    {
        _configuration = configuration;
        _fileSystemService = fileSystemService;
        _storagePath = GetFolderPath();
    }
    public async Task<byte[]> GetFileBytesAsync(string fileNameWithExtension)
    {
        var filePath = GetFilePath(fileNameWithExtension);
        var bytes = await _fileSystemService.GetFileBytesAsync(filePath);
        return bytes;
    }
    public async Task SaveFormFileAsync(FormFileDto dto)
    {
        var file = dto.File;
        var extension = Path.GetExtension(file.FileName);
        var name = dto.Name;
        var path = GetFilePath(name + extension);
        await using var fileStream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }
    public async Task SaveFileBytesAsync(byte[] fileBytes, string fileName, string extension)
    {
        var path = GetFilePath(fileName + extension);
        await _fileSystemService.SaveFileBytesAsync(fileBytes, path);
    }
    public void DeleteFile(string fileName, string extension)
    {
        var path = GetFilePath(fileName + extension);
        _fileSystemService.DeleteFile(path);
    }
    private string GetFilePath(string fileName)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        return filePath;
    }
    private string GetFolderPath()
    {
        var storagePath = _configuration["STORAGEPATH"];
        if (_fileSystemService.CheckIfDirectoryExists(storagePath) is false)
        {
            _fileSystemService.CreateDirectory(storagePath);
        }
        return Path.Combine(Directory.GetCurrentDirectory(), storagePath);
    }
}
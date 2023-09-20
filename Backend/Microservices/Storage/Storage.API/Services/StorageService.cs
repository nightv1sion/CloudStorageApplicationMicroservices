using Storage.API.DataTransferObjects;
using Storage.API.Exceptions;
using Storage.API.Services.Contracts;

namespace Storage.API.Services;

public class StorageService : IStorageService
{
    private readonly IConfiguration _configuration;
    private readonly string _storagePath;
    private readonly IFileSystemService _fileSystemService;
    private readonly ILogger<StorageService> _logger;

    public StorageService(
        IConfiguration configuration, 
        IFileSystemService fileSystemService,
        ILogger<StorageService> logger)
    {
        _configuration = configuration;
        _fileSystemService = fileSystemService;
        _storagePath = GetFolderPath();
        _logger = logger;
        _logger.LogInformation("Storage service was called");
    }
    public async Task<byte[]> GetFileBytesAsync(string fileNameWithExtension)
    {
        var filePath = GetFilePath(fileNameWithExtension);
        var bytes = await _fileSystemService.GetFileBytesAsync(filePath);
        _logger.LogInformation($"File '{filePath}' was took successfully");
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
        _logger.LogInformation($"File '{path}' was saved successfully");
    }
    public async Task SaveFileBytesAsync(byte[] fileBytes, string fileName, string extension)
    {
        var path = GetFilePath(fileName + extension);
        await _fileSystemService.SaveFileBytesAsync(fileBytes, path);
        _logger.LogInformation($"File '{path}' was saved successfully");
    }
    public void DeleteFile(string fileName, string extension)
    {
        var path = GetFilePath(fileName + extension);
        _fileSystemService.DeleteFile(path);
        _logger.LogInformation($"File '{path}' was removed successfully");
    }
    private string GetFilePath(string fileName)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        return filePath;
    }
    private string GetFolderPath()
    {
        var storagePath = _configuration["Storage:Path"]!;
        if (_fileSystemService.CheckIfDirectoryExists(storagePath) is false)
        {
            _fileSystemService.CreateDirectory(storagePath);
        }
        return Path.Combine(Directory.GetCurrentDirectory(), storagePath);
    }
}
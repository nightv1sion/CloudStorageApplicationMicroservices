using FileStorage.API.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.API.Services;

public class StorageService : IStorageService
{
    private readonly IConfiguration _configuration;

    public StorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<byte[]> GetFileAsync(string systemFileNameWithExtension, string fileName)
    {
        var folderPath = GetFolderPath();
        var filePath = Path.Combine(folderPath, systemFileNameWithExtension);
        var bytes = await File.ReadAllBytesAsync(filePath);
        return bytes;
    }
    public async Task SaveFormFileAsync(IFormFile file, string name)
    {
        var extension = Path.GetExtension(file.FileName);
        var folderPath = GetFolderPath();
        CheckAndCreateFolder(folderPath);
        var path = Path.Combine(folderPath, name + extension);
        await using var fileStream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }

    private string GetFolderPath()
    {
        var storagePath = _configuration["FILESTORAGEPATH"];
        return Path.Combine(Directory.GetCurrentDirectory(), storagePath);
    }

    private void CheckAndCreateFolder(string path)
    {
        if (Directory.Exists(path) is false)
        {
            Directory.CreateDirectory(path);
        }
    }
}
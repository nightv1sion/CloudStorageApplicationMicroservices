using Storage.API.DataTransferObjects;
using Storage.API.Services.Contracts;

namespace Storage.API.Services;

public class StorageService : IStorageService
{
    private readonly IConfiguration _configuration;

    public StorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<byte[]> GetFileBytesAsync(string fileNameWithExtension)
    {
        var folderPath = GetFolderPath();
        var filePath = Path.Combine(folderPath, fileNameWithExtension);
        var bytes = await File.ReadAllBytesAsync(filePath);
        return bytes;
    }
    public async Task SaveFormFileAsync(FormFileDto dto)
    {
        var file = dto.File;
        var extension = Path.GetExtension(file.FileName);
        var name = dto.Name;
        var folderPath = GetFolderPath();
        CheckAndCreateFolder(folderPath);
        var path = Path.Combine(folderPath, name + extension);
        await using var fileStream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }
    private string GetFolderPath()
    {
        var storagePath = _configuration["STORAGEPATH"];
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
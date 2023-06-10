using Storage.API.DataTransferObjects;
using Storage.API.Services.Contracts;

namespace Storage.API.Services;

public class StorageService : IStorageService
{
    private readonly IConfiguration _configuration;
    private readonly string _storagePath;

    public StorageService(IConfiguration configuration)
    {
        _configuration = configuration;
        _storagePath = GetFolderPath();
    }
    public async Task<byte[]> GetFileBytesAsync(string fileNameWithExtension)
    {
        var filePath = Path.Combine(_storagePath, fileNameWithExtension);
        var bytes = await File.ReadAllBytesAsync(filePath);
        return bytes;
    }
    public async Task SaveFormFileAsync(FormFileDto dto)
    {
        var file = dto.File;
        var extension = Path.GetExtension(file.FileName);
        var name = dto.Name;
        var path = Path.Combine(_storagePath, name + extension);
        await using var fileStream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }
    public async Task SaveFileBytesAsync(byte[] fileBytes, string fileName, string extension)
    {
        var path = Path.Combine(_storagePath, fileName + extension);
        await File.WriteAllBytesAsync(path, fileBytes);
    }
    public void DeleteFile(string fileName, string extension)
    {
        var path = Path.Combine(_storagePath, fileName + extension);
        File.Delete(path);
    }
    private string GetFolderPath()
    {
        var storagePath = _configuration["STORAGEPATH"];
        CheckAndCreateFolder(storagePath);
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
namespace FileStorage.API.Services.Contracts;

public interface IStorageService
{
    public Task SaveFormFileAsync(IFormFile file, string name);
    public Task<byte[]> GetFileAsync(string systemFileNameWithExtension, string fileName);

}
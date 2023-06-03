namespace Audio.API.Services.Contracts;

public interface IFileService
{
    public Task SaveFormFileAsync(IFormFile file, string name);
    public Task<byte[]> GetFormFileAsync(string systemFileNameWithExtension, string fileName);

}
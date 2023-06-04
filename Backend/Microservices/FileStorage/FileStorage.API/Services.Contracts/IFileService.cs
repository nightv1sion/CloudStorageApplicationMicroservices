using FileStorage.API.Model;
using FileStorage.API.DataTransferObjects;

namespace FileStorage.API.Services.Contracts;

public interface IFileService
{
    public Task<(byte[] file, string fileName)> GetFileBytesAsync(Guid fileId);
    public Task SaveFileToStorageAsync(FileDto dto, Guid userId);
}
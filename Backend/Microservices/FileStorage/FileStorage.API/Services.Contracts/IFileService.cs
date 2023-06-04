using FileStorage.API.Model;
using FileStorage.API.DataTransferObjects;
using File = FileStorage.API.Model.File;

namespace FileStorage.API.Services.Contracts;

public interface IFileService
{
    public Task<File> GetFileAsync(Guid userId, Guid fileId);
    public Task<ICollection<File>> GetFilesByUserIdAsync(Guid userId);
    public Task UpdateFileAsync(Guid userId, UpdateFileDto dto);
    public Task<(byte[] file, string fileName)> GetFileBytesAsync(Guid userId, Guid fileId);
    public Task SaveFileToStorageAsync(FormFileDto dto, Guid userId);
}
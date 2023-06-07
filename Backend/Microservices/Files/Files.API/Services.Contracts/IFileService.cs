using Files.API.DataTransferObjects;
using Files.API.Model;
using File = Files.API.Model.File;

namespace Files.API.Services.Contracts;

public interface IFileService
{
    public Task<Model.File> GetFileAsync(Guid userId, Guid fileId);
    public Task<ICollection<Model.File>> GetFilesByUserIdAsync(Guid userId);
    public Task UpdateFileAsync(Guid userId, UpdateFileDto dto);
    public Task<File> CreateFileAsync(CreateFileDto dto);
}
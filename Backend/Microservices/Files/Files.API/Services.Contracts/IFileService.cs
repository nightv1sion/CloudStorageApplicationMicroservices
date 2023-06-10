using Files.API.DataTransferObjects;
using Files.API.Model;
using File = Files.API.Model.File;

namespace Files.API.Services.Contracts;

public interface IFileService
{
    Task<Model.File> GetFileAsync(Guid userId, Guid fileId);
    Task<ICollection<Model.File>> GetFilesByUserIdAsync(Guid userId);
    Task UpdateFileAsync(Guid userId, UpdateFileDto dto);
    Task DeleteFileAsync(Guid userId, Guid fileId);
    Task<File> CreateFileAsync(CreateFileDto dto);
    Task UploadFileAsync(FormFileDto dto, Guid userId);
}
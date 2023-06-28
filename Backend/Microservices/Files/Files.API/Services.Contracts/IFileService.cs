using Files.API.DataTransferObjects;
using Files.API.DataTransferObjects.File;
using Files.API.Model.Database;
using File = Files.API.Model.File;

namespace Files.API.Services.Contracts;

public interface IFileService
{
    Task<FileDto> GetFileAsync(Guid userId, Guid fileId);
    Task<ICollection<FileDto>> GetFilesByUserIdAsync(Guid userId);
    Task UpdateFileAsync(Guid userId, UpdateFileDto dto);
    Task DeleteFileAsync(Guid userId, Guid fileId);
    Task<FileDto> CreateFileAsync(CreateFileDto dto);
    Task UploadFileAsync(FormFileDto dto, Guid userId);
    Task<DownloadFileDto> DownloadFileAsync(Guid userId, Guid id);
}
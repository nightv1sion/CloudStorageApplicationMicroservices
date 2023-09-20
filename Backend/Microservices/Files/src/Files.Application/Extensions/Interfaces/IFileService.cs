using Files.Application.Features.File.DataTransferObjects;

namespace Files.Application.Extensions.Interfaces;
public interface IFileService
{
    Task<FileDto> GetFileAsync(Guid userId, Guid? directoryId, Guid fileId, 
        CancellationToken cancellationToken = default);
    Task<ICollection<FileDto>> GetFilesAsync(Guid userId, Guid? directoryId, bool trackChanges = false, 
        CancellationToken cancellationToken = default);
    Task<FileDto> UpdateFileAsync(Guid userId, UpdateFileDto dto, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(Guid userId, Guid? directoryId, Guid fileId,
        CancellationToken cancellationToken = default);
    Task<FileDto> UploadFileAsync(Guid userId, FormFileDto dto, CancellationToken cancellationToken = default);
    Task<DownloadFileDto> DownloadFileAsync(Guid userId, Guid? directoryId, Guid id, 
        CancellationToken cancellationToken = default);
}
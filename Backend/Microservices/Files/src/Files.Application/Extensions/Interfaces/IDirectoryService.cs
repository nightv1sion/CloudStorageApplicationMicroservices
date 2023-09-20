using Files.Application.Features.Directory.DataTransferObjects;

namespace Files.Application.Extensions.Interfaces;

public interface IDirectoryService
{
    Task<DirectoryDto> GetDirectoryAsync(Guid userId, Guid? parentDirectoryId, Guid directoryId,
        CancellationToken cancellationToken = default);

    Task<ICollection<DirectoryDto>> GetDirectoriesAsync(Guid userId, Guid? parentDirectoryId, bool trackChanges = false,
        CancellationToken cancellationToken = default);

    Task<DirectoryDto> CreateDirectoryAsync(Guid userId, CreateDirectoryDto dto,
        CancellationToken cancellationToken = default);

    Task<DirectoryDto> UpdateDirectoryAsync(Guid userId, UpdateDirectoryDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteDirectoryAsync(Guid userId, Guid? parentDirectoryId, Guid directoryId,
        CancellationToken cancellationToken = default);
}
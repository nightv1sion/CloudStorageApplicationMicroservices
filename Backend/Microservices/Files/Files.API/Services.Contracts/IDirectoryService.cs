using Files.API.DataTransferObjects.Directory;

namespace Files.API.Services.Contracts;

public interface IDirectoryService
{
    Task<DirectoryDto> GetDirectoryAsync(Guid userId, Guid directoryId);
    Task<ICollection<DirectoryDto>> GetDirectoriesAsync(Guid userId);
    Task<DirectoryDto> CreateDirectoryAsync(Guid userId, CreateDirectoryDto dto);
    Task UpdateDirectoryAsync(Guid userId, UpdateDirectoryDto dto);
    Task DeleteDirectoryAsync(Guid userId, Guid directoryId);
}
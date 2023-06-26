using Files.API.DataTransferObjects.Directory;

namespace Files.API.Services.Contracts;

public interface IDirectoryService
{
    Task<ICollection<DirectoryDto>> GetDirectoriesAsync(Guid userId);
    Task<DirectoryDto> CreateDirectoryAsync(Guid userId, CreateDirectoryDto dto);
}
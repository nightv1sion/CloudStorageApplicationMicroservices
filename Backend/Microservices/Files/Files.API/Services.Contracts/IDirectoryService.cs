using Files.API.DataTransferObjects;

namespace Files.API.Services.Contracts;

public interface IDirectoryService
{
    public Task<Files.API.Model.Directory> CreateDirectoryAsync(CreateDirectoryDto dto, Guid userId);
}
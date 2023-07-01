using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Commands.UpdateUserDirectory;

public class UpdateUserDirectoryCommand : IRequest<DirectoryDto>
{
    public UpdateUserDirectoryCommand(
        Guid userId,
        UpdateDirectoryDto dto)
    {
        UserId = userId;
        Dto = dto;
    }
    public Guid UserId { get; }
    public UpdateDirectoryDto Dto { get; }
}
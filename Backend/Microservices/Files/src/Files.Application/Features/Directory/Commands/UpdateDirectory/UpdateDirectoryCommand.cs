using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Commands.UpdateDirectory;

public class UpdateDirectoryCommand : IRequest<DirectoryDto>
{
    public UpdateDirectoryCommand(
        Guid userId,
        UpdateDirectoryDto dto)
    {
        UserId = userId;
        Dto = dto;
    }
    public Guid UserId { get; }
    public UpdateDirectoryDto Dto { get; }
}
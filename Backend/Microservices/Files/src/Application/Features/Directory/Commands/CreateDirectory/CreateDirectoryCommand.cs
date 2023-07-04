using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Commands.CreateDirectory;

public class CreateDirectoryCommand : IRequest<DirectoryDto>
{
    public CreateDirectoryCommand(
        Guid userId, 
        CreateDirectoryDto dto)
    {
        UserId = userId;
        Dto = dto;
    }
    public Guid UserId { get; }
    public CreateDirectoryDto Dto { get; }
}
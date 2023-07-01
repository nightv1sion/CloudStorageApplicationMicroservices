using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Commands.CreateUserDirectory;

public class CreateUserDirectoryCommand : IRequest<DirectoryDto>
{
    public CreateUserDirectoryCommand(
        Guid userId, 
        CreateDirectoryDto dto)
    {
        UserId = userId;
        Dto = dto;
    }
    public Guid UserId { get; }
    public CreateDirectoryDto Dto { get; }
}
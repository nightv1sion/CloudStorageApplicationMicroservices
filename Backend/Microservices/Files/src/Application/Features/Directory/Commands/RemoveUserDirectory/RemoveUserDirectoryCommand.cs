using MediatR;

namespace Files.Application.Features.Directory.Commands.RemoveUserDirectory;

public class RemoveUserDirectoryCommand : IRequest
{
    public RemoveUserDirectoryCommand(
        Guid userId,
        Guid directoryId)
    {
        UserId = userId;
        DirectoryId = directoryId;
    }
    public Guid UserId { get; }
    public Guid DirectoryId { get; }
}
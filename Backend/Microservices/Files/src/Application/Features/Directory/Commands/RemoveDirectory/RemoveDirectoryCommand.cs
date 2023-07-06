using MediatR;

namespace Files.Application.Features.Directory.Commands.RemoveDirectory;

public class RemoveDirectoryCommand : IRequest
{
    public RemoveDirectoryCommand(
        Guid userId,
        Guid? parentDirectoryId,
        Guid directoryId)
    {
        UserId = userId;
        ParentDirectoryId = parentDirectoryId;
        DirectoryId = directoryId;
    }
    public Guid UserId { get; }
    public Guid? ParentDirectoryId { get; }
    public Guid DirectoryId { get; }
}
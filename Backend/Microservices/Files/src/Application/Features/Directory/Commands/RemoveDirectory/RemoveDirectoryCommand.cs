using MediatR;

namespace Files.Application.Features.Directory.Commands.RemoveDirectory;

public class RemoveDirectoryCommand : IRequest
{
    public RemoveDirectoryCommand(
        Guid userId,
        Guid directoryId)
    {
        UserId = userId;
        DirectoryId = directoryId;
    }
    public Guid UserId { get; }
    public Guid DirectoryId { get; }
}
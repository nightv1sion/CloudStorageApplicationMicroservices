using MediatR;

namespace Files.Application.Features.File.Commands.RemoveFile;

public class RemoveFileCommand : IRequest
{
    public RemoveFileCommand(
        Guid userId,
        Guid? directoryId,
        Guid fileId)
    {
        UserId = userId;
        DirectoryId = directoryId;
        FileId = fileId;
    }
    public Guid UserId { get; }
    public Guid? DirectoryId { get; }
    public Guid FileId { get; }
}
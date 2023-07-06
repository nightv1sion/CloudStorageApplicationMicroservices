using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetDirectory;

public class GetDirectoryQuery : IRequest<DirectoryDto>
{
    public GetDirectoryQuery(
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
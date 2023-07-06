using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetDirectories;

public class GetDirectoriesQuery : IRequest<ICollection<DirectoryDto>>
{
    public GetDirectoriesQuery(
        Guid userId,
        Guid? parentDirectoryId)
    {
        UserId = userId;
        ParentDirectoryId = parentDirectoryId;
    }
    public Guid UserId { get; }
    public Guid? ParentDirectoryId { get; }
}
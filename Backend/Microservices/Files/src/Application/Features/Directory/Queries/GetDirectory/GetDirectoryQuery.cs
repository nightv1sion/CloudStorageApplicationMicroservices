using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetDirectory;

public class GetDirectoryQuery : IRequest<DirectoryDto>
{
    public GetDirectoryQuery(
        Guid userId,
        Guid directoryId)
    {
        UserId = userId;
        DirectoryId = directoryId;
    }
    public Guid UserId { get; }
    public Guid DirectoryId { get; }

}
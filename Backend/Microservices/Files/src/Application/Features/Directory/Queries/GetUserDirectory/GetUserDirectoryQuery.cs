using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetUserDirectory;

public class GetUserDirectoryQuery : IRequest<DirectoryDto>
{
    public GetUserDirectoryQuery(
        Guid userId,
        Guid directoryId)
    {
        UserId = userId;
        DirectoryId = directoryId;
    }
    public Guid UserId { get; }
    public Guid DirectoryId { get; }

}
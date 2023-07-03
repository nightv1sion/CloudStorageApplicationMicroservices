using MediatR;

namespace Files.Application.Features.File.Queries.GetUserDirectoryFiles;

public class GetUserDirectoryFilesQuery : IRequest<ICollection<Domain.Entities.File.File>>
{
    public GetUserDirectoryFilesQuery(Guid userId, Guid directoryId)
    {
        UserId = userId;
        DirectoryId = directoryId;
    }
    public Guid UserId { get; }
    public Guid DirectoryId { get; }
}
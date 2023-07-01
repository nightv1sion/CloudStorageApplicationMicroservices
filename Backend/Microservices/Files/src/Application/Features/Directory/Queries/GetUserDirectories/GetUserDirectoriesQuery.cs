using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetUserDirectories;

public class GetUserDirectoriesQuery : IRequest<ICollection<DirectoryDto>>
{
    public GetUserDirectoriesQuery(
        Guid userId)
    {
        UserId = userId;
    }
    public Guid UserId { get; }
}
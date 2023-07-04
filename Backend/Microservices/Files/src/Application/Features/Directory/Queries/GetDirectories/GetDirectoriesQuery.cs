using Files.Application.Features.Directory.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetDirectories;

public class GetDirectoriesQuery : IRequest<ICollection<DirectoryDto>>
{
    public GetDirectoriesQuery(
        Guid userId)
    {
        UserId = userId;
    }
    public Guid UserId { get; }
}
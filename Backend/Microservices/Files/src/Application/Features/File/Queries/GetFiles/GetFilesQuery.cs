using Files.Application.Features.File.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.File.Queries.GetFiles;

public class GetFilesQuery : IRequest<ICollection<FileDto>>
{
    public GetFilesQuery(
        Guid userId, 
        Guid? directoryId)
    {
        UserId = userId;
        DirectoryId = directoryId;
    }
    public Guid UserId { get; }
    public Guid? DirectoryId { get; }
}
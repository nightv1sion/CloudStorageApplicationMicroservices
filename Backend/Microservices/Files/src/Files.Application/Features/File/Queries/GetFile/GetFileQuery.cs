using Files.Application.Features.File.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.File.Queries.GetFile;

public class GetFileQuery : IRequest<FileDto>
{
    public GetFileQuery(
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
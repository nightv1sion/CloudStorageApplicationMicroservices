using Files.Application.Features.File.DataTransferObjects;
using MediatR;

namespace Files.Application.Features.File.Queries.DownloadFile;

public class DownloadFileQuery : IRequest<DownloadFileDto>
{
    public DownloadFileQuery(
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
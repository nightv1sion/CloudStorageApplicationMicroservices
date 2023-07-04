using Files.Application.Features.File.DataTransferObjects;
using Files.Application.Features.File.Services;
using MediatR;

namespace Files.Application.Features.File.Queries.DownloadFile;

public class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, DownloadFileDto>
{
    private readonly IFileService _fileService;

    public DownloadFileQueryHandler(
        IFileService fileService)
    {
        _fileService = fileService;
    }
    public async Task<DownloadFileDto> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        var dto = await _fileService.DownloadFileAsync(
            request.UserId, request.DirectoryId, request.FileId, cancellationToken);
        return dto;
    }
}
using Files.Application.Features.File.DataTransferObjects;
using Files.Application.Features.File.Services;
using MediatR;

namespace Files.Application.Features.File.Queries.GetFile;

public class GetFileQueryHandler : IRequestHandler<GetFileQuery, FileDto>
{
    private readonly IFileService _fileService;

    public GetFileQueryHandler(
        IFileService fileService)
    {
        _fileService = fileService;
    }
    public async Task<FileDto> Handle(
        GetFileQuery request, CancellationToken cancellationToken)
    {
        var file = await _fileService.GetFileAsync(
            request.UserId, request.DirectoryId, request.FileId, cancellationToken);

        return file;
    }
}
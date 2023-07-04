using Files.Application.Features.File.DataTransferObjects;
using Files.Application.Features.File.Services;
using MediatR;

namespace Files.Application.Features.File.Queries.GetFiles;

public class GetFilesQueryHandler 
    : IRequestHandler<GetFilesQuery, ICollection<FileDto>>
{
    private readonly IFileService _fileService;

    public GetFilesQueryHandler(
        IFileService fileService)
    {
        _fileService = fileService;
    }
    public async Task<ICollection<FileDto>> Handle(
        GetFilesQuery request, 
        CancellationToken cancellationToken)
    {
        var files = await _fileService.GetFilesAsync(
            request.UserId, request.DirectoryId, false, cancellationToken);
        return files;
    }
}
using Files.Application.Features.File.Services;
using MediatR;

namespace Files.Application.Features.File.Commands.RemoveFile;

public class RemoveFileCommandHandler : IRequestHandler<RemoveFileCommand>
{
    private readonly IFileService _fileService;

    public RemoveFileCommandHandler(
        IFileService fileService)
    {
        _fileService = fileService;
    }
    public async Task Handle(RemoveFileCommand request, CancellationToken cancellationToken)
    {
        await _fileService.DeleteFileAsync(request.UserId, request.DirectoryId, request.FileId, cancellationToken);
    }
}
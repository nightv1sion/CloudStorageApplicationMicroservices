using Files.Application.Features.File.DataTransferObjects;
using Files.Application.Features.File.Services;
using MediatR;

namespace Files.Application.Features.File.Commands.CreateFile;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileDto>
{
    private readonly IFileService _fileService;

    public UploadFileCommandHandler(
        IFileService fileService)
    {
        _fileService = fileService;
    }
    public async Task<FileDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var dto = await _fileService.UploadFileAsync(request.UserId, request.Dto, cancellationToken);
        return dto;
    }
}
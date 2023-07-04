using Files.Application.Features.File.DataTransferObjects;
using Files.Application.Features.File.Services;
using MediatR;

namespace Files.Application.Features.File.Commands.UpdateFile;

public class UpdateFileCommandHandler : IRequestHandler<UpdateFileCommand, FileDto>
{
    private readonly IFileService _fileService;

    public UpdateFileCommandHandler(
        IFileService fileService)
    {
        _fileService = fileService;
    }
    public async Task<FileDto> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
    {
        var dto = await _fileService.UpdateFileAsync(request.UserId, request.Dto, cancellationToken);

        return dto;
    }
}
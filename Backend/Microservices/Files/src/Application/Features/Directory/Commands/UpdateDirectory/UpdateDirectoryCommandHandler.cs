using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Commands.UpdateDirectory;

public class UpdateDirectoryCommandHandler : IRequestHandler<UpdateDirectoryCommand, DirectoryDto>
{
    private readonly IDirectoryService _directoryService;

    public UpdateDirectoryCommandHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task<DirectoryDto> Handle(
        UpdateDirectoryCommand request, CancellationToken cancellationToken)
    {
        var directory = await _directoryService.UpdateDirectoryAsync(
            request.UserId, request.Dto, cancellationToken);
        return directory;
    }
}
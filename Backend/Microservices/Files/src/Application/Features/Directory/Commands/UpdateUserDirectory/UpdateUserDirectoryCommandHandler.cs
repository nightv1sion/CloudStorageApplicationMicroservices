using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Commands.UpdateUserDirectory;

public class UpdateUserDirectoryCommandHandler : IRequestHandler<UpdateUserDirectoryCommand, DirectoryDto>
{
    private readonly IDirectoryService _directoryService;

    public UpdateUserDirectoryCommandHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task<DirectoryDto> Handle(
        UpdateUserDirectoryCommand request, CancellationToken cancellationToken)
    {
        var directory = await _directoryService.UpdateDirectoryAsync(
            request.UserId, request.Dto, cancellationToken);
        return directory;
    }
}
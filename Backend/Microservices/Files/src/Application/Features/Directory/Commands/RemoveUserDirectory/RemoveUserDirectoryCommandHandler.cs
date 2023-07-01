using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Commands.RemoveUserDirectory;

public class RemoveUserDirectoryCommandHandler : IRequestHandler<RemoveUserDirectoryCommand>
{
    private readonly IDirectoryService _directoryService;

    public RemoveUserDirectoryCommandHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task Handle(
        RemoveUserDirectoryCommand request, 
        CancellationToken cancellationToken)
    {
        await _directoryService.DeleteDirectoryAsync(request.UserId, request.DirectoryId, cancellationToken);
    }
}
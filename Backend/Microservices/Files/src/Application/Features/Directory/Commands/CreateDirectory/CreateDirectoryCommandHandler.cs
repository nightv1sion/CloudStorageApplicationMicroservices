using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Commands.CreateDirectory;

public class CreateDirectoryCommandHandler : IRequestHandler<CreateDirectoryCommand, DirectoryDto>
{
    private readonly IDirectoryService _directoryService;

    public CreateDirectoryCommandHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task<DirectoryDto> Handle(
        CreateDirectoryCommand request, 
        CancellationToken cancellationToken)
    {
        var directory = await _directoryService.CreateDirectoryAsync(
            request.UserId, request.Dto, cancellationToken);

        return directory;
    }
}
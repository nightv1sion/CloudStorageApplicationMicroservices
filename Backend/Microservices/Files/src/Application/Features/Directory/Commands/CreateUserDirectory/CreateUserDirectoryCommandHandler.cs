using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Commands.CreateUserDirectory;

public class CreateUserDirectoryCommandHandler : IRequestHandler<CreateUserDirectoryCommand, DirectoryDto>
{
    private readonly IDirectoryService _directoryService;

    public CreateUserDirectoryCommandHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task<DirectoryDto> Handle(
        CreateUserDirectoryCommand request, 
        CancellationToken cancellationToken)
    {
        var directory = await _directoryService.CreateDirectoryAsync(
            request.UserId, request.Dto, cancellationToken);

        return directory;
    }
}
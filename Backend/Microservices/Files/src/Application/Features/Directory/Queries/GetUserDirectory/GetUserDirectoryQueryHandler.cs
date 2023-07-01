using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetUserDirectory;

public class GetUserDirectoryQueryHandler : IRequestHandler<GetUserDirectoryQuery, DirectoryDto>
{
    private readonly IDirectoryService _directoryService;

    public GetUserDirectoryQueryHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task<DirectoryDto> Handle(
        GetUserDirectoryQuery request, 
        CancellationToken cancellationToken)
    {
        var directory = await _directoryService.GetDirectoryAsync(
            request.UserId, request.DirectoryId, cancellationToken);
        return directory;
    }
}
using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetDirectory;

public class GetDirectoryQueryHandler : IRequestHandler<GetDirectoryQuery, DirectoryDto>
{
    private readonly IDirectoryService _directoryService;

    public GetDirectoryQueryHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task<DirectoryDto> Handle(
        GetDirectoryQuery request, 
        CancellationToken cancellationToken)
    {
        var directory = await _directoryService.GetDirectoryAsync(
            request.UserId, request.DirectoryId, cancellationToken);
        return directory;
    }
}
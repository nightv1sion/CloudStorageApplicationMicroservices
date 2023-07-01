using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetUserDirectories;

public class GetUserDirectoriesQueryHandler
    : IRequestHandler<GetUserDirectoriesQuery, ICollection<DirectoryDto>>
{
    private readonly IDirectoryService _directoryService;

    public GetUserDirectoriesQueryHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task<ICollection<DirectoryDto>> Handle(
        GetUserDirectoriesQuery request, CancellationToken cancellationToken)
    {
        var directories = await _directoryService.GetDirectoriesAsync(
            request.UserId, false, cancellationToken);

        return directories;
    }
}
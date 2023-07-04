using Files.Application.Features.Directory.DataTransferObjects;
using Files.Application.Features.Directory.Services;
using MediatR;

namespace Files.Application.Features.Directory.Queries.GetDirectories;

public class GetDirectoriesQueryHandler
    : IRequestHandler<GetDirectoriesQuery, ICollection<DirectoryDto>>
{
    private readonly IDirectoryService _directoryService;

    public GetDirectoriesQueryHandler(
        IDirectoryService directoryService)
    {
        _directoryService = directoryService;
    }
    public async Task<ICollection<DirectoryDto>> Handle(
        GetDirectoriesQuery request, CancellationToken cancellationToken)
    {
        var directories = await _directoryService.GetDirectoriesAsync(
            request.UserId, false, cancellationToken);

        return directories;
    }
}
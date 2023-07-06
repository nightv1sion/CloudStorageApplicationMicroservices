using Files.Application.Extensions.Interfaces;
using Files.Application.Features.Directory.DataTransferObjects;
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
            request.UserId, request.ParentDirectoryId,false, cancellationToken);

        return directories;
    }
}
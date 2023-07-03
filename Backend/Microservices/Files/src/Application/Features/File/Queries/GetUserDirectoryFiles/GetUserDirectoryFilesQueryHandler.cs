using MediatR;

namespace Files.Application.Features.File.Queries.GetUserDirectoryFiles;

public class GetUserDirectoryFilesQueryHandler 
    : IRequestHandler<GetUserDirectoryFilesQuery, ICollection<Domain.Entities.File.File>>
{
    public GetUserDirectoryFilesQueryHandler()
    {
        
    }
    public Task<ICollection<Domain.Entities.File.File>> Handle(
        GetUserDirectoryFilesQuery request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
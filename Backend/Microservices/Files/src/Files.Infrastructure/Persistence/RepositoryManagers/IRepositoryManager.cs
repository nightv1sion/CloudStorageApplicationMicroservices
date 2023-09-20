using Files.Infrastructure.Persistence.Repositories;
using Files.Infrastructure.Persistence.Repository.Interfaces;

namespace Files.Infrastructure.Persistence.RepositoryManagers;

public interface IRepositoryManager
{
    IFileRepository FileRepository { get; }
    IDirectoryRepository DirectoryRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
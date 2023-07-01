using Files.Infrastructure.Persistence.Repositories;

namespace Files.Infrastructure.Persistence.RepositoryManagers;

public interface IRepositoryManager
{
    FileRepository FileRepository { get; }
    DirectoryRepository DirectoryRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
using Directory = Files.Domain.Entities.Directory.Directory;

namespace Files.Infrastructure.Persistence.Repositories;

public class DirectoryRepository : RepositoryBase<Directory>
{
    public DirectoryRepository(
        ApplicationDatabaseContext context) : base(context)
    {
    }

    public override Task<Directory> CreateAsync(Directory entity, CancellationToken cancellationToken = default)
    {
        entity.Created = DateTime.Now;
        return base.CreateAsync(entity, cancellationToken);
    }

    public override Directory Update(Directory entity)
    {
        entity.Updated = DateTime.Now;
        return base.Update(entity);
    }
}
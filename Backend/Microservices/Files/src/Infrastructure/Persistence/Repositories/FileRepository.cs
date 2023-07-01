using File = Files.Domain.Entities.File.File;

namespace Files.Infrastructure.Persistence.Repositories;

public class FileRepository : RepositoryBase<File>
{
    public FileRepository(
        ApplicationDatabaseContext context) : base(context)
    {
    }

    public override Task<File> CreateAsync(File entity, CancellationToken cancellationToken = default)
    {
        entity.Created = DateTime.Now;
        return base.CreateAsync(entity, cancellationToken);
    }

    public override File Update(File entity)
    {
        entity.Updated = DateTime.Now;
        return base.Update(entity);
    }
}
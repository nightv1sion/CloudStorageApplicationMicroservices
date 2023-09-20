using System.Linq.Expressions;
using File = Files.Domain.Entities.File.File;


namespace Files.Infrastructure.Persistence.Repository.Interfaces;

public interface IFileRepository
{
    IQueryable<File> FindAll(bool trackChanges);

    IQueryable<File> FindByCondition(Expression<Func<File, bool>> expression,
        bool trackChanges);

    Task<File> FindSingleAsync(
        Expression<Func<File, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<File> CreateAsync(File entity,
        CancellationToken cancellationToken = default);

    File Update(File entity);

    void Remove(File entity);
}
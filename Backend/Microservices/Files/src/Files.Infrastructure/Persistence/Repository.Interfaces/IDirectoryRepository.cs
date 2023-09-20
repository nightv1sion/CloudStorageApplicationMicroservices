using System.Linq.Expressions;
using Directory = Files.Domain.Entities.Directory.Directory;

namespace Files.Infrastructure.Persistence.Repository.Interfaces;

public interface IDirectoryRepository
{
    IQueryable<Directory> FindAll(bool trackChanges);

    IQueryable<Directory> FindByCondition(Expression<Func<Directory, bool>> expression,
        bool trackChanges);

    Task<Directory> FindSingleAsync(
        Expression<Func<Directory, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<Directory> CreateAsync(Directory entity,
        CancellationToken cancellationToken = default);

    Directory Update(Directory entity);

    void Remove(Directory entity);
}
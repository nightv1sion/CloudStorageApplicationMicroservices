using System.Linq.Expressions;

namespace Files.Domain.Repository;

public interface IRepositoryBase<T> where T : class
{
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
    Task<T> FindSingleAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
    T Update(T entity);
    void Remove(T entity);
}
using System.Linq.Expressions;
using Files.Domain.Repository;
using Files.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Files.Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly ApplicationDatabaseContext _context;

    public RepositoryBase(ApplicationDatabaseContext context)
    {
        _context = context;
    }

    public virtual IQueryable<T> FindAll(bool trackChanges)
        =>  trackChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();

    public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
    {
        var query = _context.Set<T>().Where(expression);
        return trackChanges ? query : query.AsNoTracking();
    }

    public virtual async Task<T> FindSingleAsync(
        Expression<Func<T, bool>> expression, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public virtual void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}
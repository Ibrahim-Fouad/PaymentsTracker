using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PaymentsTracker.Common.Extensions;
using PaymentsTracker.Models.Data;
using PaymentsTracker.Repositories.Interfaces;

namespace PaymentsTracker.Repositories.Business;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _dbContext;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().AddRange(entities);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<T>().AnyAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public Task<int> UpdateAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls,
        Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<T>().AsQueryable();
        if (predicate is not null)
            query = query.Where(predicate);

        return query.ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
    }


    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().UpdateRange(entities);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<T>().Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asTracking = true,
        CancellationToken cancellationToken = default)
    {
        return GetEntityQuery(asTracking).Where(predicate).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TResult?> GetMappedAsync<TResult>(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return GetEntityQuery(false).AsNoTracking()
            .Where(predicate)
            .MapTo<T, TResult>()
            .FirstOrDefaultAsync(cancellationToken);
    }


    #region Helper Methods

    private IQueryable<T> GetEntityQuery(bool asTracking = true)
    {
        var query = _dbContext.Set<T>().AsQueryable();
        if (asTracking is false)
            query = query.AsNoTracking();

        return query;
    }

    #endregion
}
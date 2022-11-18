using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace PaymentsTracker.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);

    Task<int> Update(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls,
        Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    Task<int> Delete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T?> GetById(Expression<Func<T, bool>> predicate, bool asTracking = true,
        CancellationToken cancellationToken = default);

    Task<TResult?> GetByIdMapped<TResult>(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);
}
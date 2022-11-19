﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace PaymentsTracker.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    void Update(T entity);

    Task<int> UpdateAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls,
        Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool asTracking = true,
        CancellationToken cancellationToken = default);

    Task<TResult?> GetMappedAsync<TResult>(Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);
}
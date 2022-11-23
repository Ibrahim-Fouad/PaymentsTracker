using AutoMapper.QueryableExtensions;
using PaymentsTracker.Common.Helpers;

namespace PaymentsTracker.Common.Extensions;

public static class QueryableEx
{
    public static IQueryable<TResult> MapTo<TResult>(this IQueryable queryable)
    {
        return queryable.ProjectTo<TResult>(CommonMapperConfiguration.Configuration);
    }
}
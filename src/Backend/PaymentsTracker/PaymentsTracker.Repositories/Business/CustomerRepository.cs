using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PaymentsTracker.Common.DTOs;
using PaymentsTracker.Common.DTOs.Customers;
using PaymentsTracker.Common.Extensions;
using PaymentsTracker.Models.Data;
using PaymentsTracker.Models.Models;
using PaymentsTracker.Repositories.Interfaces;

namespace PaymentsTracker.Repositories.Business;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ListResultDto<CustomerDto>> ListAsync(ListCriteriaDto<CustomerFilterDto?>? filter,
        CancellationToken cancellationToken = default)
    {
        filter ??= new();
        var queryable = _dbContext.Set<Customer>().AsNoTracking();
        if (filter.Data is not null)
            queryable = queryable.Where(GetFilterPredicate(filter.Data));

        var totalCount = await queryable.CountAsync(cancellationToken);
        var data = await queryable.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .OrderBy(a=>a.CustomerId)
            .MapTo<CustomerDto>()
            .ToArrayAsync(cancellationToken);

        return new ListResultDto<CustomerDto>(data, filter.PageSize, filter.PageNumber, totalCount);
    }

    private static Expression<Func<Customer, bool>> GetFilterPredicate(CustomerFilterDto? filterDto)
    {
        Expression<Func<Customer, bool>> predicate = c => true;
        if (filterDto is null)
            return predicate;

        return predicate
            .AndAlso(c =>
                string.IsNullOrEmpty(filterDto.Name) ||
                c.Name.ToUpper().Contains(filterDto.Name.ToUpper()))
            .AndAlso(c =>
                string.IsNullOrEmpty(filterDto.Phone) ||
                c.Phone.ToUpper().Contains(filterDto.Phone.ToUpper()))
            .AndAlso(c =>
                string.IsNullOrEmpty(filterDto.SecondPhone) ||
                string.IsNullOrEmpty(c.SecondPhone) ||
                c.SecondPhone.ToUpper().Contains(filterDto.SecondPhone.ToUpper()));
    }
}
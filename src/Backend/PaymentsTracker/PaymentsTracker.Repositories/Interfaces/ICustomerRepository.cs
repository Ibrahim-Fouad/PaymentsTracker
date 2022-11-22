using PaymentsTracker.Common.DTOs;
using PaymentsTracker.Common.DTOs.Customers;
using PaymentsTracker.Models.Models;

namespace PaymentsTracker.Repositories.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<ListResultDto<CustomerDto>> ListAsync(ListCriteriaDto<CustomerFilterDto?>? filter, CancellationToken cancellationToken = default);
}
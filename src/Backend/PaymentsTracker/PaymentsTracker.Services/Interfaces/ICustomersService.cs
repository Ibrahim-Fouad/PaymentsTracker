using PaymentsTracker.Common.DTOs;
using PaymentsTracker.Common.DTOs.Customers;

namespace PaymentsTracker.Services.Interfaces;

public interface ICustomersService
{
    Task<OperationResult<CustomerDto>> ManageAsync(CustomerDto customerWriterDto,
        CancellationToken cancellationToken = default);

    Task<OperationResult<CustomerDto>> GetByIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<ListResultDto<CustomerDto>> ListCustomersAsync(ListCriteriaDto<CustomerFilterDto?>? criteriaDto, CancellationToken cancellationToken = default);
}
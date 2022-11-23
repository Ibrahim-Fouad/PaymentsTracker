using AutoMapper;
using PaymentsTracker.Common.DTOs;
using PaymentsTracker.Common.DTOs.Customers;
using PaymentsTracker.Common.Enums;
using PaymentsTracker.Common.Helpers;
using PaymentsTracker.Models.Models;
using PaymentsTracker.Repositories.Interfaces;
using PaymentsTracker.Services.Interfaces;

namespace PaymentsTracker.Services.Business;

public class CustomersService : ICustomersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserIdService _userIdService;

    public CustomersService(IUnitOfWork unitOfWork, IMapper mapper, IUserIdService userIdService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userIdService = userIdService;
    }

    public async Task<OperationResult<CustomerDto>> ManageAsync(CustomerDto customerWriterDto,
        CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.AnyAsync(c =>
                c.Phone == customerWriterDto.Phone && c.CustomerId != customerWriterDto.CustomerId, cancellationToken))
            return ErrorDto.Factory(ErrorCode.CustomerPhoneAlreadyExists);

        return customerWriterDto.CustomerId is 0
            ? await AddUser(customerWriterDto, cancellationToken)
            : await UpdateUser(customerWriterDto, cancellationToken);
    }

    public async Task<OperationResult<CustomerDto>> GetByIdAsync(int customerId,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await _unitOfWork.Customers.GetMappedAsync<CustomerDto>(c => c.CustomerId == customerId, cancellationToken);
        if (customer is null)
            return ErrorDto.Factory(ErrorCode.CustomerNotFound);
        return customer;
    }

    public Task<ListResultDto<CustomerDto>> ListCustomersAsync(ListCriteriaDto<CustomerFilterDto?>? criteriaDto,
        CancellationToken cancellationToken = default)
        => _unitOfWork.Customers.ListAsync(criteriaDto, cancellationToken);

    public async Task<OperationResult<bool>> DeleteAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetAsync(c => c.CustomerId == customerId, cancellationToken);
        if (customer is null)
            return ErrorDto.Factory(ErrorCode.CustomerNotFound);

        // TODO: Perform checks if customer has relationships
        _unitOfWork.Customers.Delete(customer);
        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #region Helper Methods

    private async Task<OperationResult<CustomerDto>> AddUser(CustomerWriterDto customerWriterDto,
        CancellationToken cancellationToken = default)
    {
        var customer = _mapper.Map<Customer>(customerWriterDto);
        customer.UserId = _userIdService.UserId!.Value;
        _unitOfWork.Customers.Add(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CustomerDto>(customer);
    }

    private async Task<OperationResult<CustomerDto>> UpdateUser(CustomerDto customerWriterDto,
        CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers.GetAsync(
            customer => customer.CustomerId == customerWriterDto.CustomerId,
            cancellationToken: cancellationToken);

        if (customer is null)
            return ErrorDto.Factory(ErrorCode.CustomerNotFound);

        _mapper.Map(customerWriterDto, customer);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return customerWriterDto;
    }

    #endregion
}
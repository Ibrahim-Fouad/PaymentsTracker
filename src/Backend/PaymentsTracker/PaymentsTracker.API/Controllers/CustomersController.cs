using Microsoft.AspNetCore.Mvc;
using PaymentsTracker.API.Extensions;
using PaymentsTracker.Common.DTOs;
using PaymentsTracker.Common.DTOs.Customers;
using PaymentsTracker.Services.Interfaces;

namespace PaymentsTracker.API.Controllers;

public class CustomersController : BaseController
{
    private readonly ICustomersService _customersService;

    public CustomersController(ICustomersService customersService)
    {
        _customersService = customersService;
    }

    [HttpPost]
    public async Task<IActionResult> ManageAsync(CustomerDto customerDto, CancellationToken cancellationToken)
        => await _customersService.ManageAsync(customerDto, cancellationToken).ToActionResult();

    [HttpGet("{customerId:int}")]
    public async Task<IActionResult> GetByIdAsync(int customerId, CancellationToken cancellationToken)
        => await _customersService.GetByIdAsync(customerId, cancellationToken).ToActionResult();

    [HttpPost]
    public async Task<IActionResult> ListAsync(ListCriteriaDto<CustomerFilterDto?> filters,
        CancellationToken cancellationToken)
        => Ok(await _customersService.ListCustomersAsync(filters, cancellationToken));

    [HttpDelete("{customerId:int}")]
    public async Task<IActionResult> DeleteAsync(int customerId, CancellationToken cancellationToken)
        => await _customersService.DeleteAsync(customerId, cancellationToken).ToActionResult();
}
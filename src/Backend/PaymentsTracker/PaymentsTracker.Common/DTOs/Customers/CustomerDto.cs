using System.ComponentModel.DataAnnotations;

namespace PaymentsTracker.Common.DTOs.Customers;

public record CustomerDto([Range(0, int.MaxValue)] int CustomerId) : CustomerWriterDto;
using System.ComponentModel.DataAnnotations;

namespace PaymentsTracker.Common.DTOs.Customers;

public record CustomerWriterDto
{
    [StringLength(50)] public string Name { get; set; } = string.Empty;

    [Phone] [StringLength(15)] public string Phone { get; set; } = string.Empty;
    [Phone][StringLength(15)] public string? SecondPhone { get; set; }
    [StringLength(500)] public string? Description { get; set; }
}
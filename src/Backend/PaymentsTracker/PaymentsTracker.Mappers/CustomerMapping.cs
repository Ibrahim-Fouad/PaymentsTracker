using AutoMapper;
using PaymentsTracker.Common.DTOs.Customers;
using PaymentsTracker.Models.Models;

namespace PaymentsTracker.Mappers;

public class CustomerMapping : Profile
{
    public CustomerMapping()
    {
        CreateMap<Customer, CustomerDto>()
            .ReverseMap()
            .ForMember(x => x.CreatedAtUtc, dest => dest.Ignore());
        CreateMap<CustomerWriterDto, Customer>();


    }
}
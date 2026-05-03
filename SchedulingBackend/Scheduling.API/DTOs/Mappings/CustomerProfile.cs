using Scheduling.API.DTOs.Customer;
using Scheduling.API.Entities;

namespace Scheduling.API.DTOs.Mappings;

public static class CustomerProfile
{
    public static CustomerResponseDto ToDto(this Entities.Customer customer) =>
        new(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.IsActive
        );
}

using Scheduling.Application.DTOs.Customer;

namespace Scheduling.Application.DTOs.Mappings;

public static class CustomerProfile
{
    public static CustomerResponseDto ToDto(this global::Scheduling.Domain.Entities.Customer customer) =>
        new(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.IsActive
        );
}

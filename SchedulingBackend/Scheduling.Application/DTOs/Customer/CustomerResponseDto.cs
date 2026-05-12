namespace Scheduling.Application.DTOs.Customer;

public record CustomerResponseDto(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    DateTime CreatedAt,
    bool IsActive
);

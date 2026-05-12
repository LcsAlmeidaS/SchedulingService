using System.ComponentModel.DataAnnotations;

namespace Scheduling.Application.DTOs.Customer;

public record UpdateCustomerDto(
    [Required][EmailAddress][MaxLength(256)] string Email,
    [Phone][MaxLength(32)] string? Phone = null
);

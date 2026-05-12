using System.ComponentModel.DataAnnotations;

namespace Scheduling.Application.DTOs.Staff;

public record CreateStaffDto(
    [Required][MaxLength(128)] string Name,
    [Required][EmailAddress][MaxLength(256)] string Email,
    [Required][Phone][MaxLength(32)] string Phone
);

using System.ComponentModel.DataAnnotations;

namespace Scheduling.Application.DTOs.ServiceOffering;

public record UpdateServiceOfferingDto(
    [Required][MaxLength(128)] string Name,
    [Required][Range(0, double.MaxValue)] decimal Price,
    [Required][Range(1, int.MaxValue)] int DurationInMinutes,
    [MaxLength(512)] string? Description = null
);

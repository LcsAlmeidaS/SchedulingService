using System.ComponentModel.DataAnnotations;

namespace Scheduling.Application.DTOs.Availability;

public record AvailabilityQueryDto(
    [Required] Guid ServiceOfferingId,
    [Required] DateOnly Date,
    Guid? StaffId = null
);

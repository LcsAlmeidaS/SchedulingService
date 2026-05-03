using System.ComponentModel.DataAnnotations;

namespace Scheduling.API.DTOs.Availability;

public record AvailabilityQueryDto(
    [Required] Guid ServiceOfferingId,
    [Required] DateOnly Date,
    Guid? StaffId = null
);

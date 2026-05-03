using System.ComponentModel.DataAnnotations;

namespace Scheduling.API.DTOs.Appointment;

public record CreateAppointmentDto(
    [Required] Guid CustomerId,
    [Required] Guid StaffId,
    [Required] Guid ServiceOfferingId,
    [Required] DateTime StartTime
);

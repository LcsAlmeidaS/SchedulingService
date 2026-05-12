using Scheduling.Application.DTOs.Appointment;

namespace Scheduling.Application.DTOs.Mappings;

public static class AppointmentProfile
{
    public static AppointmentResponseDto ToDto(this global::Scheduling.Domain.Entities.Appointment appointment) =>
        new(
            appointment.Id,
            appointment.StartTime,
            appointment.EndTime,
            appointment.Status.ToString(),
            appointment.CustomerId,
            appointment.Customer?.Name ?? string.Empty,
            appointment.StaffId,
            appointment.Staff?.Name ?? string.Empty,
            appointment.ServiceOfferingId,
            appointment.ServiceOffering?.Name ?? string.Empty
        );
}

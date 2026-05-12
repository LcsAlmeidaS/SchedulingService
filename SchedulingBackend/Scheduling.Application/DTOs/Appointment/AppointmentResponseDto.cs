namespace Scheduling.Application.DTOs.Appointment;

public record AppointmentResponseDto(
    Guid Id,
    DateTime StartTime,
    DateTime EndTime,
    string Status,
    Guid CustomerId,
    string CustomerName,
    Guid StaffId,
    string StaffName,
    Guid ServiceOfferingId,
    string ServiceOfferingName
);

namespace Scheduling.Application.DTOs.Availability;

public record AvailableSlotDto(
    Guid StaffId,
    string StaffName,
    DateTime StartTime,
    DateTime EndTime
);

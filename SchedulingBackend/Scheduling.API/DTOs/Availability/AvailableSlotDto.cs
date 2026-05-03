namespace Scheduling.API.DTOs.Availability;

public record AvailableSlotDto(
    Guid StaffId,
    string StaffName,
    DateTime StartTime,
    DateTime EndTime
);

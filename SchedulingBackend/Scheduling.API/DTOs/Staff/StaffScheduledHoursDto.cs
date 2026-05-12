namespace Scheduling.API.DTOs.Staff;

public record StaffScheduledHoursDto(
    Guid StaffId,
    string StaffName,
    double TotalHours,
    DateOnly From,
    DateOnly To
);

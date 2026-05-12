namespace Scheduling.API.DTOs.Staff;

public record StaffAppointmentSummaryDto(
    Guid StaffId,
    string StaffName,
    int CompletedCount,
    double TotalHours,
    DateOnly From,
    DateOnly To
);

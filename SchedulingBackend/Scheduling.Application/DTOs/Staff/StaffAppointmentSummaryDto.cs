namespace Scheduling.Application.DTOs.Staff;

public record StaffAppointmentSummaryDto(
    Guid StaffId,
    string StaffName,
    int CompletedCount,
    double TotalHours,
    DateOnly From,
    DateOnly To
);

namespace Scheduling.API.DTOs.Staff;

public record StaffResponseDto(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    bool IsActive,
    IReadOnlyList<WorkingHoursDto> WorkingHours,
    IReadOnlyList<BreakTimeDto> BreakTimes
);

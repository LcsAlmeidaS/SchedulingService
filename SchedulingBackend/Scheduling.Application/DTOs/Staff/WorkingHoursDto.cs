namespace Scheduling.Application.DTOs.Staff;

public record WorkingHoursDto(
    Guid Id,
    string DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
);

namespace Scheduling.API.DTOs.Staff;

public record WorkingHoursDto(
    Guid Id,
    string DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
);

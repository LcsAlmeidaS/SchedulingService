namespace Scheduling.API.DTOs.Staff;

public record BreakTimeDto(
    Guid Id,
    string DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
);

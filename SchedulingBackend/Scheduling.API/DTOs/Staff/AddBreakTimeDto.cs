using System.ComponentModel.DataAnnotations;

namespace Scheduling.API.DTOs.Staff;

public record AddBreakTimeDto(
    [Required] DayOfWeek DayOfWeek,
    [Required] TimeOnly StartTime,
    [Required] TimeOnly EndTime
);

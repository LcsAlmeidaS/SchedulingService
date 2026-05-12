using System.ComponentModel.DataAnnotations;

namespace Scheduling.Application.DTOs.Staff;

public record AddWorkingHoursDto(
    [Required] DayOfWeek DayOfWeek,
    [Required] TimeOnly StartTime,
    [Required] TimeOnly EndTime
);

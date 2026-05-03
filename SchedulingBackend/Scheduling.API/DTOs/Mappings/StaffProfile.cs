using Scheduling.API.DTOs.Staff;
using Scheduling.API.Entities;

namespace Scheduling.API.DTOs.Mappings;

public static class StaffProfile
{
    public static StaffResponseDto ToDto(this Entities.Staff staff) =>
        new(
            staff.Id,
            staff.Name,
            staff.Email,
            staff.Phone,
            staff.IsActive,
            staff.WorkingHours.Select(w => w.ToDto()).ToList().AsReadOnly(),
            staff.BreakTimes.Select(b => b.ToDto()).ToList().AsReadOnly()
        );

    public static WorkingHoursDto ToDto(this WorkingHours w) =>
        new(w.Id, w.DayOfWeek.ToString(), w.StartTime, w.EndTime);

    public static BreakTimeDto ToDto(this BreakTime b) =>
        new(b.Id, b.DayOfWeek.ToString(), b.StartTime, b.EndTime);
}

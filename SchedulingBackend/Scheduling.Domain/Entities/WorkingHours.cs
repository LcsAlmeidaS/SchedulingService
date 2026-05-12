namespace Scheduling.Domain.Entities;

public class WorkingHours
{
    public Guid Id { get; private set; }
    public Guid StaffId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    private WorkingHours() { }

    public WorkingHours(Guid staffId, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("EndTime must be greater than StartTime.");

        Id = Guid.NewGuid();
        StaffId = staffId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
    }
}

namespace Scheduling.API.Entities;

public class Staff
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<WorkingHours> _workingHours = [];
    private readonly List<BreakTime> _breakTimes = [];

    public IReadOnlyCollection<WorkingHours> WorkingHours => _workingHours.AsReadOnly();
    public IReadOnlyCollection<BreakTime> BreakTimes => _breakTimes.AsReadOnly();

    private Staff() { Name = string.Empty; Email = string.Empty; Phone = string.Empty; }

    public Staff(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Email = email.Trim();
        Phone = phone.Trim();
        IsActive = true;
    }

    public void Update(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required.");

        Name = name.Trim();
        Email = email.Trim();
        Phone = phone.Trim();
    }

    public void AddWorkingHours(WorkingHours workingHours)
    {
        var conflict = _workingHours.Any(w =>
            w.DayOfWeek == workingHours.DayOfWeek &&
            w.StartTime < workingHours.EndTime &&
            w.EndTime > workingHours.StartTime);

        if (conflict)
            throw new InvalidOperationException($"Working hours overlap with an existing entry for {workingHours.DayOfWeek}.");

        _workingHours.Add(workingHours);
    }

    public void RemoveWorkingHours(Guid workingHoursId)
    {
        var entry = _workingHours.FirstOrDefault(w => w.Id == workingHoursId)
            ?? throw new InvalidOperationException("Working hours entry not found.");

        _workingHours.Remove(entry);
    }

    public void AddBreakTime(BreakTime breakTime)
    {
        var conflict = _breakTimes.Any(b =>
            b.DayOfWeek == breakTime.DayOfWeek &&
            b.OverlapsWith(breakTime.StartTime, breakTime.EndTime));

        if (conflict)
            throw new InvalidOperationException($"Break time overlaps with an existing entry for {breakTime.DayOfWeek}.");

        _breakTimes.Add(breakTime);
    }

    public void RemoveBreakTime(Guid breakTimeId)
    {
        var entry = _breakTimes.FirstOrDefault(b => b.Id == breakTimeId)
            ?? throw new InvalidOperationException("Break time entry not found.");

        _breakTimes.Remove(entry);
    }

    public void UpdateContactInfo(string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.");

        Email = email;
        Phone = phone;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}

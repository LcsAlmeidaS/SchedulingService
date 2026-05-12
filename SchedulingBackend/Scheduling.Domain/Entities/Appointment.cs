namespace Scheduling.Domain.Entities;

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed,
    NoShow
}

public class Appointment
{
    public Guid Id { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid StaffId { get; private set; }
    public Guid ServiceOfferingId { get; private set; }
    public AppointmentStatus Status { get; private set; }

    public Customer? Customer { get; private set; }
    public Staff? Staff { get; private set; }
    public ServiceOffering? ServiceOffering { get; private set; }

    private Appointment() { }

    public Appointment(
        Guid customerId,
        Guid staffId,
        Guid serviceOfferingId,
        DateTime startTime,
        DateTime endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("EndTime must be greater than StartTime.");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        StaffId = staffId;
        ServiceOfferingId = serviceOfferingId;
        StartTime = startTime;
        EndTime = endTime;
        Status = AppointmentStatus.Pending;
    }

    public void Confirm() => Transition(AppointmentStatus.Confirmed);
    public void Cancel() => Transition(AppointmentStatus.Cancelled);
    public void Complete()
    {
        if (DateTime.UtcNow < EndTime)
            throw new InvalidOperationException("Cannot complete before appointment ends.");

        Transition(AppointmentStatus.Completed);
    }
    public void MarkAsNoShow() => Transition(AppointmentStatus.NoShow);

    private void Transition(AppointmentStatus newStatus)
    {
        if (Status == AppointmentStatus.Cancelled || Status == AppointmentStatus.Completed)
            throw new InvalidOperationException($"Cannot transition from {Status} to {newStatus}.");

        Status = newStatus;
    }
}

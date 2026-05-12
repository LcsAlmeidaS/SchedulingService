using Scheduling.Domain.Entities;

namespace Scheduling.Application.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<Appointment?> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<Appointment>> GetAppointmentsByStaffOnDateAsync(Guid staffId, DateOnly date);
    Task<IEnumerable<Appointment>> GetByCustomerAsync(Guid customerId);
    Task<bool> HasConflictAsync(Guid staffId, DateTime start, DateTime end);
    Task<IEnumerable<Appointment>> GetCompletedByStaffInPeriodAsync(Guid staffId, DateTime from, DateTime to);
}

using Scheduling.API.Entities;

namespace Scheduling.API.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAppointmentsByStaffOnDateAsync(Guid staffId, DateOnly date);
    Task<IEnumerable<Appointment>> GetByCustomerAsync(Guid customerId);
    Task<bool> HasConflictAsync(Guid staffId, DateTime start, DateTime end);
}

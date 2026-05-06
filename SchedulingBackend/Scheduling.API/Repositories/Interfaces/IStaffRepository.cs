using Scheduling.API.Entities;

namespace Scheduling.API.Repositories.Interfaces;

public interface IStaffRepository : IRepository<Staff>
{
    Task<Staff?> GetByEmailAsync(string email);
    Task<IEnumerable<Staff>> GetActiveStaffAsync();
    Task<Staff?> GetWithScheduleAsync(Guid id);
}

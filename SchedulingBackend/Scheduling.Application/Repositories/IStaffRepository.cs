using Scheduling.Domain.Entities;

namespace Scheduling.Application.Repositories;

public interface IStaffRepository : IRepository<Staff>
{
    Task<Staff?> GetByEmailAsync(string email);
    Task<IEnumerable<Staff>> GetActiveStaffAsync();
    Task<Staff?> GetWithScheduleAsync(Guid id);
    Task<IEnumerable<Staff>> GetAllActiveWithScheduleAsync();
    Task AddWorkingHoursAsync(WorkingHours workingHours);
    Task AddBreakTimeAsync(BreakTime breakTime);
}

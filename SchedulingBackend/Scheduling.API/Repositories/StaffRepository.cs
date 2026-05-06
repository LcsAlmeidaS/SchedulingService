using Microsoft.EntityFrameworkCore;
using Scheduling.API.Context;
using Scheduling.API.Entities;

namespace Scheduling.API.Repositories
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(AppDbContext context) : base(context) { }

        public async Task<Staff?> GetByEmailAsync(string email)
        {
            return await _context.Staff
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<IEnumerable<Staff>> GetActiveStaffAsync()
        {
            return await _context.Staff
                .AsNoTracking()
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<Staff?> GetWithScheduleAsync(Guid id)
        {
            return await _context.Staff
                .Include(s => s.WorkingHours)
                .Include(s => s.BreakTimes)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}


using Microsoft.EntityFrameworkCore;
using Scheduling.API.Context;
using Scheduling.API.Entities;
using Scheduling.API.Repositories.Interfaces;

namespace Scheduling.API.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByStaffOnDateAsync(Guid staffId, DateOnly date)
    {
        var startOfDay = date.ToDateTime(TimeOnly.MinValue);
        var endOfDay = date.ToDateTime(TimeOnly.MaxValue);

        return await _context.Appointments
            .AsNoTracking()
            .Where(a =>
                a.StaffId == staffId &&
                a.StartTime >= startOfDay &&
                a.StartTime < endOfDay)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByCustomerAsync(Guid customerId)
    {
        return await _context.Appointments
            .AsNoTracking()
            .Include(a => a.Staff)
            .Include(a => a.ServiceOffering)
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<bool> HasConflictAsync(Guid staffId, DateTime start, DateTime end)
    {
        return await _context.Appointments
            .AnyAsync(a =>
                a.StaffId == staffId &&
                a.Status != AppointmentStatus.Cancelled &&
                a.Status != AppointmentStatus.Completed &&
                start < a.EndTime &&
                end > a.StartTime);
    }
}
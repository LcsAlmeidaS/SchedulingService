using Scheduling.Application.DTOs.Availability;
using Scheduling.Application.Repositories;
using Scheduling.Application.Services.Interfaces;
using Scheduling.Domain.Entities;

namespace Scheduling.Application.Services;

public class AvailabilityService : IAvailabilityService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IServiceOfferingRepository _serviceOfferingRepository;

    public AvailabilityService(
        IStaffRepository staffRepository,
        IAppointmentRepository appointmentRepository,
        IServiceOfferingRepository serviceOfferingRepository)
    {
        _staffRepository = staffRepository;
        _appointmentRepository = appointmentRepository;
        _serviceOfferingRepository = serviceOfferingRepository;
    }

    public async Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(AvailabilityQueryDto query)
    {
        var service = await GetServiceOrThrowAsync(query.ServiceOfferingId);
        var staffList = await GetStaffListAsync(query.StaffId);

        var slots = new List<AvailableSlotDto>();
        foreach (var staff in staffList)
            slots.AddRange(await GetSlotsForStaffAsync(staff, query.Date, service.Duration));

        return slots;
    }

    private async Task<ServiceOffering> GetServiceOrThrowAsync(Guid serviceOfferingId)
    {
        var service = await _serviceOfferingRepository.GetByIdAsync(serviceOfferingId)
            ?? throw new KeyNotFoundException("Service offering not found.");

        if (!service.IsActive)
            throw new InvalidOperationException("Service offering is not active.");

        return service;
    }

    private async Task<IEnumerable<Staff>> GetStaffListAsync(Guid? staffId)
    {
        if (staffId.HasValue)
        {
            var staff = await _staffRepository.GetWithScheduleAsync(staffId.Value)
                ?? throw new KeyNotFoundException($"Staff {staffId} not found.");
            return [staff];
        }

        return await _staffRepository.GetAllActiveWithScheduleAsync();
    }

    private async Task<IEnumerable<AvailableSlotDto>> GetSlotsForStaffAsync(Staff staff, DateOnly date, TimeSpan duration)
    {
        var workingHoursForDay = staff.WorkingHours
            .Where(w => w.DayOfWeek == date.DayOfWeek)
            .ToList();

        if (workingHoursForDay.Count == 0) return [];

        var appointments = await _appointmentRepository.GetAppointmentsByStaffOnDateAsync(staff.Id, date);
        var breaksForDay = staff.BreakTimes
            .Where(b => b.DayOfWeek == date.DayOfWeek)
            .ToList();

        return workingHoursForDay.SelectMany(window =>
            GenerateWindowSlots(staff, window, date, duration, breaksForDay, appointments));
    }

    private static IEnumerable<AvailableSlotDto> GenerateWindowSlots(
        Staff staff,
        WorkingHours window,
        DateOnly date,
        TimeSpan duration,
        IEnumerable<BreakTime> breaks,
        IEnumerable<Appointment> appointments)
    {
        var current = window.StartTime.ToTimeSpan();
        var windowEnd = window.EndTime.ToTimeSpan();

        while (current + duration <= windowEnd)
        {
            var slotEnd = current + duration;
            var slotStart = TimeOnly.FromTimeSpan(current);
            var slotEndTime = TimeOnly.FromTimeSpan(slotEnd);

            if (!OverlapsBreak(slotStart, slotEndTime, breaks) &&
                !OverlapsAppointment(date, slotStart, slotEndTime, appointments))
            {
                yield return new AvailableSlotDto(
                    staff.Id,
                    staff.Name,
                    date.ToDateTime(slotStart),
                    date.ToDateTime(slotEndTime)
                );
            }

            current = slotEnd;
        }
    }

    private static bool OverlapsBreak(TimeOnly slotStart, TimeOnly slotEnd, IEnumerable<BreakTime> breaks)
        => breaks.Any(b => slotStart < b.EndTime && slotEnd > b.StartTime);

    private static bool OverlapsAppointment(DateOnly date, TimeOnly slotStart, TimeOnly slotEnd, IEnumerable<Appointment> appointments)
    {
        var start = date.ToDateTime(slotStart);
        var end = date.ToDateTime(slotEnd);
        return appointments.Any(a => start < a.EndTime && end > a.StartTime);
    }
}

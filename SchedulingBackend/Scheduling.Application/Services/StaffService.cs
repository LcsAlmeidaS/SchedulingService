using Scheduling.Application.DTOs.Mappings;
using Scheduling.Application.DTOs.Staff;
using Scheduling.Application.Repositories;
using Scheduling.Application.Services.Interfaces;
using Scheduling.Domain.Entities;

namespace Scheduling.Application.Services;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public StaffService(IStaffRepository staffRepository, IAppointmentRepository appointmentRepository)
    {
        _staffRepository = staffRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<StaffResponseDto> CreateAsync(CreateStaffDto dto)
    {
        var existing = await _staffRepository.GetByEmailAsync(dto.Email);
        if (existing is not null)
            throw new InvalidOperationException("A staff with this email already exists.");

        var staff = new Staff(dto.Name, dto.Email, dto.Phone ?? string.Empty);
        await _staffRepository.AddAsync(staff);
        await _staffRepository.SaveChangesAsync();
        return staff.ToDto();
    }

    public async Task<StaffResponseDto> GetByIdAsync(Guid id)
    {
        var staff = await _staffRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Staff {id} not found.");
        return staff.ToDto();
    }

    public async Task<IEnumerable<StaffResponseDto>> GetActiveAsync()
    {
        var staffList = await _staffRepository.GetActiveStaffAsync();
        return staffList.Select(s => s.ToDto());
    }

    public async Task<StaffResponseDto> UpdateAsync(Guid id, UpdateStaffDto dto)
    {
        var staff = await _staffRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Staff {id} not found.");

        var emailOwner = await _staffRepository.GetByEmailAsync(dto.Email);
        if (emailOwner is not null && emailOwner.Id != id)
            throw new InvalidOperationException("This email is already in use by another staff.");

        staff.Update(dto.Name, dto.Email, dto.Phone);
        await _staffRepository.SaveChangesAsync();
        return staff.ToDto();
    }

    public async Task<StaffResponseDto> AddWorkingHoursAsync(Guid staffId, AddWorkingHoursDto dto)
    {
        var staff = await _staffRepository.GetWithScheduleAsync(staffId)
            ?? throw new KeyNotFoundException($"Staff {staffId} not found.");

        var workingHours = new WorkingHours(staffId, dto.DayOfWeek, dto.StartTime, dto.EndTime);
        staff.AddWorkingHours(workingHours);
        await _staffRepository.AddWorkingHoursAsync(workingHours);

        await _staffRepository.SaveChangesAsync();
        return staff.ToDto();
    }

    public async Task<StaffResponseDto> AddBreakTimeAsync(Guid staffId, AddBreakTimeDto dto)
    {
        var staff = await _staffRepository.GetWithScheduleAsync(staffId)
            ?? throw new KeyNotFoundException($"Staff {staffId} not found.");

        var breakTime = new BreakTime(staffId, dto.DayOfWeek, dto.StartTime, dto.EndTime);
        staff.AddBreakTime(breakTime);
        await _staffRepository.AddBreakTimeAsync(breakTime);

        await _staffRepository.SaveChangesAsync();
        return staff.ToDto();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var staff = await _staffRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Staff {id} not found.");

        staff.Deactivate();
        await _staffRepository.SaveChangesAsync();
    }

    public async Task<StaffAppointmentSummaryDto> GetAppointmentSummaryAsync(Guid staffId, DateOnly? from = null, DateOnly? to = null)
    {
        var staff = await _staffRepository.GetByIdAsync(staffId)
            ?? throw new KeyNotFoundException($"Staff {staffId} not found.");

        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var fromDate = from ?? toDate.AddDays(-30);

        var appointments = await _appointmentRepository.GetCompletedByStaffInPeriodAsync(
            staffId,
            fromDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            toDate.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc));

        var completedList = appointments.ToList();
        var totalHours = Math.Round(completedList.Sum(a => (a.EndTime - a.StartTime).TotalHours), 2);

        return new StaffAppointmentSummaryDto(staffId, staff.Name, completedList.Count, totalHours, fromDate, toDate);
    }

    public async Task<StaffScheduledHoursDto> GetScheduledHoursAsync(Guid staffId, DateOnly? from = null, DateOnly? to = null)
    {
        var staff = await _staffRepository.GetWithScheduleAsync(staffId)
            ?? throw new KeyNotFoundException($"Staff {staffId} not found.");

        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var fromDate = from ?? toDate.AddDays(-30);

        var totalMinutes = 0.0;
        for (var date = fromDate; date <= toDate; date = date.AddDays(1))
        {
            foreach (var wh in staff.WorkingHours.Where(w => w.DayOfWeek == date.DayOfWeek))
                totalMinutes += (wh.EndTime - wh.StartTime).TotalMinutes;
        }

        return new StaffScheduledHoursDto(staffId, staff.Name, Math.Round(totalMinutes / 60, 2), fromDate, toDate);
    }
}

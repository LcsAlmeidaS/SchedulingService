using Scheduling.API.Repositories.Interfaces;
using Scheduling.API.DTOs.Staff;
using Scheduling.API.Entities;
using Scheduling.API.DTOs.Mappings;
using Scheduling.API.Services.Interfaces;

namespace Scheduling.API.Repositories;

public class StaffService : IStaffService
{
    public readonly IStaffRepository _staffRepository;

    public StaffService(IStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
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
            ?? throw new KeyNotFoundException($"Staff with email '{id}' not found.");

        return staff.ToDto();
    }

    public async Task<IEnumerable<StaffResponseDto>> GetActiveAsync()
    {
        var staffs = await _staffRepository.GetActiveStaffAsync();

        return staffs.Select(s => s.ToDto());
    }

    public async Task<StaffResponseDto> UpdateAsync(Guid id, UpdateStaffDto dto)
    {
        var staff = await _staffRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Staff {id} not found.");

        var emailOwner = await _staffRepository.GetByEmailAsync(dto.Email);
        if (emailOwner is not null && emailOwner.Id != id)
            throw new InvalidOperationException("This email is already in use by another staff.");

        staff.UpdateContactInfo(dto.Email, dto.Phone ?? string.Empty);
        await _staffRepository.SaveChangesAsync();
        return staff.ToDto();
    }

    public async Task<StaffResponseDto> AddWorkingHoursAsync(Guid staffId, AddWorkingHoursDto dto)
    {
        var staff = await _staffRepository.GetWithScheduleAsync(staffId)
            ?? throw new KeyNotFoundException($"Staff {staffId} not found.");

        var workingHours = new WorkingHours(staffId, dto.DayOfWeek, dto.StartTime, dto.EndTime);
        staff.AddWorkingHours(workingHours);

        await _staffRepository.SaveChangesAsync();
        return staff.ToDto();
    }

    public async Task<StaffResponseDto> AddBreakTimeAsync(Guid staffId, AddBreakTimeDto dto)
    {
        var staff = await _staffRepository.GetWithScheduleAsync(staffId)
            ?? throw new KeyNotFoundException($"Staff {staffId} not found.");

        var breakTime = new BreakTime(staffId, dto.DayOfWeek, dto.StartTime, dto.EndTime);
        staff.AddBreakTime(breakTime);

        await _staffRepository.SaveChangesAsync();
        return staff.ToDto();
    }
    public async Task DeactivateAsync(Guid id)
    {

    }
}
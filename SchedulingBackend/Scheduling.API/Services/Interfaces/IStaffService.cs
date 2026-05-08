using Scheduling.API.DTOs.Staff;

namespace Scheduling.API.Services.Interfaces;

public interface IStaffService
{
    Task<StaffResponseDto> CreateAsync(CreateStaffDto dto);
    Task<StaffResponseDto> GetByIdAsync(Guid id);
    Task<IEnumerable<StaffResponseDto>> GetActiveAsync();
    Task<StaffResponseDto> UpdateAsync(Guid id, UpdateStaffDto dto);
    Task<StaffResponseDto> AddWorkingHoursAsync(Guid staffId, AddWorkingHoursDto dto);
    Task<StaffResponseDto> AddBreakTimeAsync(Guid staffId, AddBreakTimeDto dto);
    Task DeactivateAsync(Guid id);
}

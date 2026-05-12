using Scheduling.Application.DTOs.Availability;

namespace Scheduling.Application.Services.Interfaces;

public interface IAvailabilityService
{
    Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(AvailabilityQueryDto query);
}

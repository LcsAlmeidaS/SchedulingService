using Scheduling.API.DTOs.Availability;

namespace Scheduling.API.Services.Interfaces;

public interface IAvailabilityService
{
    Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(AvailabilityQueryDto query);
}

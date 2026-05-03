using Scheduling.API.DTOs.ServiceOffering;
using Scheduling.API.Entities;

namespace Scheduling.API.DTOs.Mappings;

public static class ServiceOfferingProfile
{
    public static ServiceOfferingResponseDto ToDto(this Entities.ServiceOffering serviceOffering) =>
        new(
            serviceOffering.Id,
            serviceOffering.Name,
            serviceOffering.Description,
            serviceOffering.Price,
            (int)serviceOffering.Duration.TotalMinutes,
            serviceOffering.IsActive
        );
}

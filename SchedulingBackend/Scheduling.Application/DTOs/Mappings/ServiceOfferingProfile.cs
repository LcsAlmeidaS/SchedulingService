using Scheduling.Application.DTOs.ServiceOffering;

namespace Scheduling.Application.DTOs.Mappings;

public static class ServiceOfferingProfile
{
    public static ServiceOfferingResponseDto ToDto(this global::Scheduling.Domain.Entities.ServiceOffering serviceOffering) =>
        new(
            serviceOffering.Id,
            serviceOffering.Name,
            serviceOffering.Description,
            serviceOffering.Price,
            (int)serviceOffering.Duration.TotalMinutes,
            serviceOffering.IsActive
        );
}

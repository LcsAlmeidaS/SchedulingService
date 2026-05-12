namespace Scheduling.Application.DTOs.ServiceOffering;

public record ServiceOfferingResponseDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int DurationInMinutes,
    bool IsActive
);

using Scheduling.API.DTOs.ServiceOffering;

namespace Scheduling.API.Services.Interfaces;

public interface IServiceOfferingService
{
    Task<ServiceOfferingResponseDto> CreateAsync(CreateServiceOfferingDto dto);
    Task<ServiceOfferingResponseDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ServiceOfferingResponseDto>> GetAllActiveAsync();
    Task<ServiceOfferingResponseDto> UpdateAsync(Guid id, UpdateServiceOfferingDto dto);
    Task<ServiceOfferingResponseDto> ActivateAsync(Guid id);
    Task<ServiceOfferingResponseDto> DeactivateAsync(Guid id);
}

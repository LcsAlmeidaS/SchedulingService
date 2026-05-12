using Scheduling.API.DTOs.Mappings;
using Scheduling.API.DTOs.ServiceOffering;
using Scheduling.API.Entities;
using Scheduling.API.Repositories.Interfaces;
using Scheduling.API.Services.Interfaces;

namespace Scheduling.API.Services;

public class ServiceOfferingService : IServiceOfferingService
{
    private readonly IServiceOfferingRepository _serviceOfferingRepository;

    public ServiceOfferingService(IServiceOfferingRepository serviceOfferingRepository)
    {
        _serviceOfferingRepository = serviceOfferingRepository;
    }

    public async Task<ServiceOfferingResponseDto> CreateAsync(CreateServiceOfferingDto dto)
    {
        var serviceOffering = new ServiceOffering(
            dto.Name,
            dto.Price,
            TimeSpan.FromMinutes(dto.DurationInMinutes),
            dto.Description);

        await _serviceOfferingRepository.AddAsync(serviceOffering);
        await _serviceOfferingRepository.SaveChangesAsync();
        return serviceOffering.ToDto();
    }

    public async Task<ServiceOfferingResponseDto> GetByIdAsync(Guid id)
    {
        var serviceOffering = await _serviceOfferingRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Service offering {id} not found.");
        return serviceOffering.ToDto();
    }

    public async Task<IEnumerable<ServiceOfferingResponseDto>> GetAllActiveAsync()
    {
        var serviceOfferings = await _serviceOfferingRepository.GetAllActiveAsync();
        return serviceOfferings.Select(s => s.ToDto());
    }

    public async Task<ServiceOfferingResponseDto> UpdateAsync(Guid id, UpdateServiceOfferingDto dto)
    {
        var serviceOffering = await _serviceOfferingRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Service offering {id} not found.");

        serviceOffering.Update(
            dto.Name,
            dto.Price,
            TimeSpan.FromMinutes(dto.DurationInMinutes),
            dto.Description);

        await _serviceOfferingRepository.SaveChangesAsync();
        return serviceOffering.ToDto();
    }

    public async Task<ServiceOfferingResponseDto> ActivateAsync(Guid id)
    {
        var serviceOffering = await _serviceOfferingRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Service offering {id} not found.");

        serviceOffering.Activate();
        await _serviceOfferingRepository.SaveChangesAsync();
        return serviceOffering.ToDto();
    }

    public async Task<ServiceOfferingResponseDto> DeactivateAsync(Guid id)
    {
        var serviceOffering = await _serviceOfferingRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Service offering {id} not found.");

        serviceOffering.Deactivate();
        await _serviceOfferingRepository.SaveChangesAsync();
        return serviceOffering.ToDto();
    }
}

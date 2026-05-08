using Scheduling.API.DTOs.Customer;

namespace Scheduling.API.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerResponseDto> GetByIdAsync(Guid id);
    Task<CustomerResponseDto> GetByEmailAsync(string email);
    Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
    Task<CustomerResponseDto> UpdateAsync(Guid id, UpdateCustomerDto dto);
}

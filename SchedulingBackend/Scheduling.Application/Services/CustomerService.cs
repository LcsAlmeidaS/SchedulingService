using Scheduling.Application.DTOs.Customer;
using Scheduling.Application.DTOs.Mappings;
using Scheduling.Application.Repositories;
using Scheduling.Application.Services.Interfaces;
using Scheduling.Domain.Entities;

namespace Scheduling.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto)
    {
        var existing = await _customerRepository.GetByEmailAsync(dto.Email);
        if (existing is not null)
            throw new InvalidOperationException("A customer with this email already exists.");

        var customer = new Customer(dto.Name, dto.Email, dto.Phone ?? string.Empty);
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();
        return customer.ToDto();
    }

    public async Task<CustomerResponseDto> GetByIdAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Customer {id} not found.");
        return customer.ToDto();
    }

    public async Task<CustomerResponseDto> GetByEmailAsync(string email)
    {
        var customer = await _customerRepository.GetByEmailAsync(email)
            ?? throw new KeyNotFoundException($"Customer with email '{email}' not found.");
        return customer.ToDto();
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => c.ToDto());
    }

    public async Task<CustomerResponseDto> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Customer {id} not found.");

        var emailOwner = await _customerRepository.GetByEmailAsync(dto.Email);
        if (emailOwner is not null && emailOwner.Id != id)
            throw new InvalidOperationException("This email is already in use by another customer.");

        customer.UpdateContactInfo(dto.Email, dto.Phone ?? string.Empty);
        await _customerRepository.SaveChangesAsync();
        return customer.ToDto();
    }
}

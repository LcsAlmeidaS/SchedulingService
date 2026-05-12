using Scheduling.Domain.Entities;

namespace Scheduling.Application.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
}

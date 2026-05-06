using Scheduling.API.Entities;

namespace Scheduling.API.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
}

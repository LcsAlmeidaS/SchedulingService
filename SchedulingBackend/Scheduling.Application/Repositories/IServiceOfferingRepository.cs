using Scheduling.Domain.Entities;

namespace Scheduling.Application.Repositories;

public interface IServiceOfferingRepository : IRepository<ServiceOffering>
{
    Task<IEnumerable<ServiceOffering>> GetAllActiveAsync();
}

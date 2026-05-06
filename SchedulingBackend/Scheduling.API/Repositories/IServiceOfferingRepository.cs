using Scheduling.API.Entities;

namespace Scheduling.API.Repositories;

public interface IServiceOfferingRepository : IRepository<ServiceOffering>
{
    Task<IEnumerable<ServiceOffering>> GetAllActiveAsync();
}

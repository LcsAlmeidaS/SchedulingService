using Scheduling.API.Entities;

namespace Scheduling.API.Repositories.Interfaces;

public interface IServiceOfferingRepository : IRepository<ServiceOffering>
{
    Task<IEnumerable<ServiceOffering>> GetAllActiveAsync();
}

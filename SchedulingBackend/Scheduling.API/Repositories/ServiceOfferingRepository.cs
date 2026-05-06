using Microsoft.EntityFrameworkCore;
using Scheduling.API.Context;
using Scheduling.API.Entities;

namespace Scheduling.API.Repositories;

public class ServiceOfferingRepository : Repository<ServiceOffering>, IServiceOfferingRepository
{
    public ServiceOfferingRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ServiceOffering>> GetAllActiveAsync()
    {
        return await _context.ServiceOfferings
            .AsNoTracking()
            .Where(s => s.IsActive)
            .ToListAsync();
    }
}

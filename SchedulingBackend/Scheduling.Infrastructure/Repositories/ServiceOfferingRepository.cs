using Microsoft.EntityFrameworkCore;
using Scheduling.Application.Repositories;
using Scheduling.Domain.Entities;
using Scheduling.Infrastructure.Context;

namespace Scheduling.Infrastructure.Repositories;

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

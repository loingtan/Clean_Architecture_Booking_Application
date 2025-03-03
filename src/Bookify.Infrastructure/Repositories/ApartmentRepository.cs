using Bookify.Domain.Entities.Apartments;
using Bookify.Infrastructure.Specifications.Apartments;
using Microsoft.EntityFrameworkCore;
namespace Bookify.Infrastructure.Repositories;
public sealed class ApartmentRepository(ApplicationDbContext dbContext)
    : Repository<Apartment, ApartmentId>(dbContext), IApartmentRepository
{
    public override async Task<Apartment> GetByIdAsync(ApartmentId id, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new ApartmentByIdSpecification(id))
            .FirstOrDefaultAsync(cancellationToken);
    }

};

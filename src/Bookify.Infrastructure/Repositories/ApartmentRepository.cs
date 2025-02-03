using Bookify.Domain.Entities.Apartments;

namespace Bookify.Infrastructure.Repositories;
public sealed class ApartmentRepository(ApplicationDbContext dbContext)
    : Repository<Apartment, ApartmentId>(dbContext), IApartmentRepository;

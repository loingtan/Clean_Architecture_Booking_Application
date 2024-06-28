﻿using Bookify.Domain.Entities.Apartments;

namespace Bookify.Infrastructure.Repositories;
internal sealed class ApartmentRepository : Repository<Apartment, ApartmentId>, IApartmentRepository
{
    public ApartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

using System;
using Bookify.Domain.Entities.Apartments;

namespace Bookify.Infrastructure.Specifications.Apartments;

public class ApartmentByIdSpecification : Specification<Apartment, ApartmentId>
{
    public ApartmentByIdSpecification(ApartmentId id)
        : base(apartment => apartment.Id == id)
    {
        AddInclude(apartment => apartment.Address);
        AddInclude(apartment => apartment.Price);
        AddInclude(apartment => apartment.CleaningFee);
        AddInclude(apartment => apartment.Amenities);
    }
}
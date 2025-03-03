using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Apartments.GetApartment;

public sealed record GetApartmentQuery(Guid Id) : IQuery<ApartmentResponse>;
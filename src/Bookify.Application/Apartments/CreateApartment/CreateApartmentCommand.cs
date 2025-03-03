using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Apartments.CreateApartment;

public record CreateApartmentCommand(
    string Name,
    string Description,
    decimal Price,
    int Rooms,
    int Beds,
    int Bathrooms,
    string Address,
    string City,
    string Country,
    decimal Latitude,
    decimal Longitude) : ICommand<Guid>;
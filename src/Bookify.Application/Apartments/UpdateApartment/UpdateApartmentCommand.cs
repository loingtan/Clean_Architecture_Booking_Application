using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Apartments.UpdateApartment;

public sealed record UpdateApartmentCommand(
    Guid Id,
    string? Name,
    string? Description,
    decimal? Price,
    int? Rooms,
    int? Beds,
    int? Bathrooms,
    string? Address,
    string? City,
    string? Country,
    decimal? Latitude,
    decimal? Longitude) : ICommand;
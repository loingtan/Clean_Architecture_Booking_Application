using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Entities.Apartments.Enums;

namespace Bookify.Application.Apartments.UpdateApartment;

public sealed record UpdateApartmentCommand(
    Guid Id,
    string? Name,
    string? Description,
    decimal? Price,
    decimal? CleaningFee,
    List<Amenity>? Amenities,  // Added Amenities parameter
    string? Address,
    string? City,
    string? State,
    string? ZipCode,
    string? Country) : ICommand;
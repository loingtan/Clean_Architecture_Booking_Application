namespace Bookify.Application.Apartments.GetApartment;

public sealed class ApartmentResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public int Rooms { get; init; }
    public int Bathrooms { get; init; }
    public int Beds { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
}
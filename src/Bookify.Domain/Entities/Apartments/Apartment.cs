using Bookify.Domain.Entities.Abstractions;
using Bookify.Domain.Entities.Apartments.Enums;
using Bookify.Domain.Entities.Apartments.ValueObjects;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Entities.Apartments;
public sealed class Apartment : AuditableEntity<ApartmentId>
{
    public Apartment(
        ApartmentId id,
        string name,
        string description,
        Address address,
        Money price,
        Money cleaningFee,
        List<Amenity> amenities)
        : base(id)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        CleaningFee = cleaningFee;
        Amenities = amenities;
    }

    private Apartment() { }
    public static Apartment Create(
        string name,
        string description,
        Address address,
        Money price,
        Money cleaningFee,
        List<Amenity> amenities)
    {
        return new Apartment(
            new ApartmentId(Guid.NewGuid()),
            name,
            description,
            address,
            price,
            cleaningFee,
            amenities);
    }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public Money CleaningFee { get; private set; }
    public DateTime? LastBookedOnUtc { get; internal set; }
    public List<Amenity> Amenities { get; private set; }

    public void Update(string? name = null,
                       string? description = null,
                       decimal? priceAmount = null,
                       int? rooms = null,
                       int? beds = null,
                       int? bathrooms = null,
                       string? street = null,
                       string? city = null,
                       string? country = null,
                       decimal? latitude = null,
                       decimal? longitude = null)
    {
        if (name is not null)
            Name = name;

        if (description is not null)
            Description = description;
        if (priceAmount is null)
        {
            throw new ArgumentNullException(nameof(priceAmount));
        }

        if (rooms is null)
        {
            throw new ArgumentNullException(nameof(rooms));
        }

        if (beds is null)
        {
            throw new ArgumentNullException(nameof(beds));
        }

        if (bathrooms is null)
        {
            throw new ArgumentNullException(nameof(bathrooms));
        }

        if (priceAmount.HasValue)
            Price = Money.From(priceAmount.Value, Price.Currency);

        // Update address if any address components are specified
        if (street is not null || city is not null || country is not null ||
            latitude.HasValue || longitude.HasValue)
        {
            Address = Address.From(
                country ?? Address.Country,
                Address.State, // Assuming state isn't updated
                Address.ZipCode, // Assuming zipcode isn't updated
                city ?? Address.City,
                street ?? Address.Street);
        }

        if (longitude is null)
        {
            throw new ArgumentNullException(nameof(longitude));
        }

        if (latitude is null)
        {
            throw new ArgumentNullException(nameof(latitude));
        }

        // Update other scalar properties if specified

    }
}

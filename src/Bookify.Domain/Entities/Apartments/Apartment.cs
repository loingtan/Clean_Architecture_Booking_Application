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
                       decimal? cleaningFeeAmount = null,
                       List<Amenity>? amenities = null,
                       string? street = null,
                       string? city = null,
                       string? state = null,
                       string? zipCode = null,
                       string? country = null
             )
    {
        if (name is not null)
            Name = name;

        if (description is not null)
            Description = description;

        if (priceAmount.HasValue)
            Price = Money.From(priceAmount.Value, Price.Currency);

        if (cleaningFeeAmount.HasValue)
            CleaningFee = Money.From(cleaningFeeAmount.Value, CleaningFee.Currency);

        if (amenities is not null)
            Amenities = amenities;

        
        if (street is not null || city is not null || state is not null ||
            zipCode is not null || country is not null)
        {
            Address = Address.From(
                country ?? Address.Country,
                state ?? Address.State,
                zipCode ?? Address.ZipCode,
                city ?? Address.City,
                street ?? Address.Street);
        }
    }
}

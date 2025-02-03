using Bookify.Domain.Entities.Apartments;
using Bookify.Domain.Entities.Apartments.Enums;
using Bookify.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;
internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    private static readonly char[] Separator = [','];

    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        var amenitiesComparer = new ValueComparer<List<Amenity>>(
            (a, b) => a.SequenceEqual(b),
            a => a.Aggregate(0, (hash, item) => HashCode.Combine(hash, ((int)item).GetHashCode())),
            a => a.ToList()
        );
        builder.ToTable("apartments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(apartmentId => apartmentId.Value, value => new ApartmentId(value));

        builder.OwnsOne(x => x.Address);
        builder.Navigation(x => x.Address).IsRequired();         
        builder.Property(x => x.Name)
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.OwnsOne(x => x.Price, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.From(code));
        });

        builder.OwnsOne(x => x.CleaningFee, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.From(code));
        });
        builder.Property(a => a.Amenities)
            .HasConversion(
                v => string.Join(",", v.Select(e => ((int)e).ToString())),
                v => v.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => (Amenity)int.Parse(x))
                    .ToList()
            ).HasColumnName("Amenities")
            .HasMaxLength(100).Metadata.SetValueComparer(amenitiesComparer);
        builder.Property<uint>("Version").IsRowVersion();
    }
}

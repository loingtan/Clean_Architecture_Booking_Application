using Bogus;
using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Entities.Apartments;
using Bookify.Domain.Entities.Apartments.Enums;
using Bookify.Domain.Entities.Apartments.ValueObjects;
using Bookify.Domain.Shared;
using Bookify.Infrastructure;
using Bookify.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Bookify.API.Extensions
{
    public static class SeedDataExtension
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            try
            {
                
                using var scope = app.ApplicationServices.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var apartmentRepository = scope.ServiceProvider.GetRequiredService<IApartmentRepository>();
                var faker = new Faker();
                if (await dbContext.Apartments.AnyAsync())
                {
                    return;
                }
                for (var i = 0; i < 100; i++)
                {
                    var amenities = new List<Amenity>
                    {
                        Amenity.SwimmingPool,
                        Amenity.Parking,
                        Amenity.Gym,
                        Amenity.WiFi
                    };
                    var apartment = Apartment.Create(
                        faker.Company.CompanyName(),
                        faker.Lorem.Sentence(),
                        Address.From(
                            faker.Address.StreetAddress(),
                            faker.Address.City(),
                            faker.Address.State(),
                            faker.Address.Country(),
                            faker.Address.ZipCode()
                        ),
                        Money.From(faker.Random.Decimal(50, 1000), Currency.Usd),
                        Money.From(faker.Random.Decimal(25, 200), Currency.Usd),
                        amenities);
                    apartmentRepository.Add(apartment);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while seeding the database", e);
            }
        }
    }
}

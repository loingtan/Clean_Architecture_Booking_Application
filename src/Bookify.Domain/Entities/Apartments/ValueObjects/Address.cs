namespace Bookify.Domain.Entities.Apartments.ValueObjects;
using Bookify.Domain.Entities.Abstractions;

public class Address: ValueObject
{
    public string Street { get; private init; }
    public string City { get; private init;}
    public string State { get; private init;}
    public string Country { get; private init;}
    public string ZipCode { get; private init;}

    private Address()
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return Country;
        yield return ZipCode;
    }

    public static Address From(string street, string city, string state, string country, string zipCode)
    {
        return new Address { Street = street, City = city, State = state, Country = country, ZipCode = zipCode };
    }
  
}


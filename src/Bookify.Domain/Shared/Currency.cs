using System.Runtime.CompilerServices;
using Bookify.Domain.Entities.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Bookify.Domain.Shared;

public class Currency : ValueObject
{
    public static readonly Currency None = new("");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");

    private Currency(string code) => Code = code;
    public string Code { get; init; }

    public static Currency From(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ApplicationException("The currency code is invalid");
        if (All.All(x => x.Code != code))
            throw new ApplicationException("The currency code is invalid");
        return All.First(x => x.Code == code);
    }

    private static readonly IReadOnlyCollection<Currency> All =
    [
        Usd, Eur
    ];

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
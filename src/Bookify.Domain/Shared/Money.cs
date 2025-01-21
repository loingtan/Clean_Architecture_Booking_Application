using Bookify.Domain.Entities.Abstractions;

namespace Bookify.Domain.Shared;

public class Money : ValueObject
{
    public decimal Amount { get; private init; }
    public Currency Currency { get; private init; }
    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
            throw new InvalidOperationException("Currencies have to be equal");

        return From(first.Amount + second.Amount, first.Currency);
    }
    
    public static Money From(decimal amount, Currency currency) => new Money
    {
        Amount = amount,
        Currency = currency
    };
    public static Money Zero() => From(0, Currency.None);
    public static Money Zero(Currency currency) => From(0, currency);
    
    public bool IsZero() => EqualOperator(this,Zero(Currency));
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
